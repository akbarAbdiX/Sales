using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using static BigBang.App.Sales.Common.Constants.SaleConstant;

namespace BigBang.App.Sales.Service.SOP.Concrete
{

    [InternalService(ServiceType = typeof(IAdjustmentService), InstanceMode = InstanceMode.Scoped)]
    internal class AdjustmentService : IAdjustmentService
    {
        private readonly IDocumentAdjustmentRepository _documentAdjustmentRepository;
        public AdjustmentService(IDocumentAdjustmentRepository documentAdjustmentRepository)
        {     
            _documentAdjustmentRepository = documentAdjustmentRepository;
        }
        public void AdjustmentOnUpdating(SALSOPAdjustment entity)
        {
            var adjustmentId = entity.ID;
            var changeType = GetChangeType(entity.AdjustmentType?.Name);
            var rows = _documentAdjustmentRepository
                .GetMany(x => x.Adjustment.ID == adjustmentId && x.DocumentItem != null)
                .Select(x => new DocumentDTO
                {
                    ID = x.ID,
                    DocumentItemID = x.DocumentItem.ID,
                    DocumentID = x.DocumentItem.Document.ID,
                    Discount = x.DocumentItem.Discount,
                    Tax = x.DocumentItem.Tax,
                    AdditionalCharge = x.DocumentItem.AdditionalCharge,
                    Quantity = x.DocumentItem.Quantity,
                    UnitPrice = x.DocumentItem.UnitPrice,
                    TotalPrice = x.DocumentItem.TotalPrice,
                    DocDiscount = x.DocumentItem.Document.DiscountAmount,
                    DocTax = x.DocumentItem.Document.TaxAmount,
                    DocAdditional = x.DocumentItem.Document.AdditionalChargesAmount,
                    DocTotalPrice = x.DocumentItem.Document.TotalAmount,
                    AdjustmentAmount = x.AdjustmentAmount,
                    ParameterAmount = x.ParameterAmount
                }).ToList();

            var contextFieldsDTO = new ContextFieldsDTO { Rate = entity.Rate, Amount = entity.Amount };

            UpdateItemsAndDocuments(rows, contextFieldsDTO, changeType);
        }
      
        private static ChangeType GetChangeType(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return ChangeType.Default;
            name = name.Trim();
            if (name.Equals(Discount, System.StringComparison.OrdinalIgnoreCase)) return ChangeType.Discount;
            if (name.Equals(Tax, System.StringComparison.OrdinalIgnoreCase)) return ChangeType.Tax;
            if (name.Equals(AdditionalCharges, System.StringComparison.OrdinalIgnoreCase)) return ChangeType.AdditionalCharge;
            return ChangeType.Default;
        }

        private void UpdateItemsAndDocuments(List<DocumentDTO> rows, ContextFieldsDTO context, ChangeType changeType)
        {
            var adjustmentsToSave = new List<SALSOPDocumentAdjustment>();
            var itemsToSave = new List<SALSOPDocumentItem>();

            var itemSnapshotsForHeader = new List<ItemForHeaderSnapshot>();

            foreach (var row in rows)
            {
                var oldAmount = row.AdjustmentAmount;
                var newAmount = context.Rate != 0m
                    ? (row.Quantity * row.UnitPrice * context.Rate) / 100m
                    : context.Amount;

                row.Delta = newAmount - oldAmount;
                row.AdjustmentAmount = newAmount;
                row.ParameterAmount = 0m;

                adjustmentsToSave.Add(new SALSOPDocumentAdjustment
                {
                    ID = row.ID,
                    AdjustmentAmount = newAmount,
                    ParameterAmount = 0m,
                    EntityState = Metadata.Models.EntityState.Modified
                });
            }

            var deltaByItem = rows
                .GroupBy(x => x.DocumentItemID)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Delta = g.Sum(x => x.Delta),
                        First = g.First()
                    });
            foreach (var Item in deltaByItem)
            {
                var first = Item.Value.First;
                var delta = Item.Value.Delta;

                decimal newDiscount = first.Discount;
                decimal newTax = first.Tax;
                decimal newAdditional = first.AdditionalCharge;

                switch (changeType)
                {
                    case ChangeType.Discount: newDiscount = first.Discount + delta; break;
                    case ChangeType.Tax: newTax = first.Tax + delta; break;
                    case ChangeType.AdditionalCharge: newAdditional = first.AdditionalCharge + delta; break;
                    case ChangeType.Default: break;
                }

                decimal newTotalPrice = first.TotalPrice
                    + (changeType == ChangeType.Discount ? -delta : 0m)
                    + (changeType == ChangeType.Tax ? +delta : 0m)
                    + (changeType == ChangeType.AdditionalCharge ? +delta : 0m);

                itemsToSave.Add(new SALSOPDocumentItem
                {
                    ID = Item.Key,
                    Discount = newDiscount,
                    Tax = newTax,
                    AdditionalCharge = newAdditional,
                    TotalPrice = newTotalPrice,
                    EntityState = Metadata.Models.EntityState.Modified
                });

                itemSnapshotsForHeader.Add(new ItemForHeaderSnapshot
                {
                    DocumentID = first.DocumentID,
                    Discount = newDiscount,
                    Tax = newTax,
                    AdditionalCharge = newAdditional,
                    TotalPrice = newTotalPrice
                });
            }
            ServiceLocator.Get<IBulkRepository<SALSOPDocumentItem>>()
                .Builder
                .Entities(itemsToSave)
                .AllowedProperties(x => x.Discount, x => x.Tax, x => x.AdditionalCharge, x => x.TotalPrice, x => x.Description)
                .Save();
            var documentsToSave = new List<SALSOPDocument>();
            var totalsByDoc = itemSnapshotsForHeader
                .GroupBy(x => x.DocumentID)
                .Select(g => new
                {                  
                    DocumentID = g.Key,
                    SumDiscount = g.Sum(i => i.Discount),
                    SumTax = g.Sum(i => i.Tax),
                    SumAdditional = g.Sum(i => i.AdditionalCharge),
                    SumTotalPrice = g.Sum(i => i.TotalPrice)
                })
                .ToList();

            foreach (var total in totalsByDoc)
            {
                documentsToSave.Add(new SALSOPDocument
                {
                    ID = total.DocumentID,
                    DiscountAmount = total.SumDiscount,
                    TaxAmount = total.SumTax,
                    AdditionalChargesAmount = total.SumAdditional,
                    TotalAmount = total.SumTotalPrice,
                    EntityState = Metadata.Models.EntityState.Modified
                });
            }
            ServiceLocator.Get<IBulkRepository<SALSOPDocument>>()
                .Builder
                .Entities(documentsToSave)
                .AllowedProperties(
                    d => d.DiscountAmount,
                    d => d.TaxAmount,
                    d => d.AdditionalChargesAmount,
                    d => d.TotalAmount
                )
                .Save();
            ServiceLocator.Get<IBulkRepository<SALSOPDocumentAdjustment>>()
                .Builder
                .Entities(adjustmentsToSave)
                .AllowedProperties(x => x.AdjustmentAmount, x => x.ParameterAmount)
                .Save();
        }
    }
    internal class DocumentDTO : DocumentItemDTO
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal ParameterAmount { get; set; }
        public decimal Delta { get; set; }
        public decimal DocDiscount { get; set; }
        public decimal DocTax { get; set; }
        public decimal DocAdditional { get; set; }
        public decimal DocTotalPrice { get; set; }
    }

    internal class DocumentItemDTO
    {
        public int DocumentItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal TotalPrice { get; set; }
        public override bool Equals(object obj) => obj is DocumentItemDTO other && other.DocumentItemID == DocumentItemID;
        public override int GetHashCode() => DocumentItemID.GetHashCode();
    }

    internal class ItemForHeaderSnapshot
    {
        public int DocumentID { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal TotalPrice { get; set; }
    }

    internal class ContextFieldsDTO
    {
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }

    internal enum ChangeType
    {
        Default = 0,
        Discount = 1,
        Tax = 2,
        AdditionalCharge = 3
    }
}
    

