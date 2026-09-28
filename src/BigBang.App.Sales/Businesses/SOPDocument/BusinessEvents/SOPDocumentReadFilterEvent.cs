using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Services;
using NHibernate;
using NHibernate.Criterion;

namespace BigBang.App.Sales.Businesses.SOPDocument.BusinessEvents
{
    public enum SOPDocumentTypeId
    {
        ProformaInvoice = 1,
        Order = 2,
        Contract = 3,
        Invoice = 4,
        InvoiceIssuanceRequest = 5,
        PriceProposal = 6
    }

    [EventSubscription(typeof(IBusinessReadFilterEvent<SALSOPDocument>))]
    public class SOPDocumentBusinessReadFilterEvent : IBusinessReadFilterEvent<SALSOPDocument>
    {
        private readonly IBusinessContext _businessContext;
        private readonly ISession _session;
        private readonly IBigBangIdentityService _bigBangIdentityService;
        private const string Key_Proforma = "DocumentTypeProformaInvoice";
        private const string Key_Order = "DocumentTypeOrder";
        private const string Key_Contract = "DocumentTypeContract";
        private const string Key_Invoice = "DocumentTypeInvoice";
        private const string Key_PriceProposal = "DocumentTypePriceProposal";
        private const string Key_InvoiceIssuanceRequest = "DocumentTypeInvoiceIssuanceRequest";
        private const string Key_DocumentType = "FilterDocumentType";
        private const string Key_CustomerId = "FilterCustomerId";
        private static readonly string[] CustomerIdKeys = { Key_CustomerId };

        public SOPDocumentBusinessReadFilterEvent(IBusinessContext businessContext, ISession session, IBigBangIdentityService bigBangIdentityService)
        {
            _bigBangIdentityService = bigBangIdentityService;
            _businessContext = businessContext;
            _session = session;
        }

        public void OnRead(IBusinessReadFilter<SALSOPDocument> filter)
        {
            var query = filter.QueryOverOf();

            if (_businessContext.HasKey("FilterDocumentId"))
            {
                query.Where(Restrictions.Or(
                            Restrictions.Where<SALSOPDocument>(x => x.StateID != Domain.Enums.SOPChangeStateHistoryEntityStateType.Discarded),
                            Restrictions.Where<SALSOPDocument>(x => x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Draft)
               ));
            }
            //TODO: shouldmove to rowlevelaccess 

            //if (HasFinancialRole())
            //{
            //    query.Where(Restrictions.Or(
            //                Restrictions.Where<SALSOPDocument>(x => x.DocumentType.ID != (int)SOPDocumentTypeId.InvoiceIssuanceRequest),
            //                Restrictions.Where<SALSOPDocument>(x => x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Sent ||
            //                                                        x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Approved ||
            //                                                        x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Confirmed ||
            //                                                        x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.ApprovedBySalesManagment)));
            //}

            //if (CurrentRoleIsSalesManagmentRole())
            //{
            //    query.Where(Restrictions.Or(
            //                Restrictions.Where<SALSOPDocument>(x => x.DocumentType.ID != (int)SOPDocumentTypeId.InvoiceIssuanceRequest),
            //                Restrictions.Where<SALSOPDocument>(x => x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Confirmed ||
            //                                                        x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Approved ||
            //                                                        x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Rejected ||
            //                                                        x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.RejectedBySalesManagment ||
            //                                                        x.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.ApprovedBySalesManagment)));
            //}

            var allowedTypeIds = GatherAllowedTypesFromContext();

            if (TryGetIntFromContext(Key_DocumentType, out var testDocTypeId))
            {
                var prevTypeIds = GetPreviousStepDocumentTypeIds(testDocTypeId);
                if (prevTypeIds.Any())
                {
                    allowedTypeIds = prevTypeIds;

                    query.Where(BuildConsumedSourceExclusion(testDocTypeId));
                }
                else
                {
                    query.Where(x => x.ID == -1);
                    return;
                }
            }

            if (allowedTypeIds.Any())
            {
                SALSOPDocumentType docTypeAlias = null;
                query.JoinAlias(x => x.DocumentType, () => docTypeAlias)
                     .WhereRestrictionOn(() => docTypeAlias.ID).IsIn(allowedTypeIds);
            }

            if (TryGetCustomerId(out var customerId))
            {
                query.Where(x => x.CustomerID.Id == customerId);
            }
        }
        //TODO: shouldmove to rowlevelaccess 
        //private bool HasFinancialRole()
        //{
        //    long currentUserId = Convert.ToInt64(_bigBangIdentityService.Identity.Actor.Id);

        //    return _session.Query<SALUserRole>()
        //        .Where(x => x.User.Id == currentUserId)
        //        .ToList()
        //        .Any(x => x.Role != null && x.Role.Name == "SalesFinance");
        //}

        //private bool CurrentRoleIsSalesManagmentRole()
        //{
        //    long currentUserId = Convert.ToInt64(_bigBangIdentityService.Identity.Actor.Id);

        //    return _session.Query<SALUserRole>()
        //                   .Where(x => x.User.Id == currentUserId)
        //                   .ToList()
        //                   .Any(x => x.Role != null && x.Role.Name == "SalesManagment");
        //}

        private ICriterion BuildConsumedSourceExclusion(int nextStepDocTypeId)
        {
            var consumed = DetachedCriteria.For<SALSOPDocument>("consumer")
                .CreateAlias("consumer.DocumentType", "docType")
                .CreateAlias("consumer.Document", "sourceDoc")
                .Add(Restrictions.Eq("docType.ID", nextStepDocTypeId))
                .Add(Restrictions.Eq("consumer.StateID", 1))
                .SetProjection(Projections.Property("sourceDoc.ID"));

            return Subqueries.PropertyNotIn("ID", consumed);
        }

        private List<int> GetPreviousStepDocumentTypeIds(int currentDocTypeId)
        {
            var currentLevel = _session.Query<SALSOPDocumentFlow>()
                .Where(x => x.DocumentType.ID == currentDocTypeId)
                .Select(x => (int?)x.Level)
                .FirstOrDefault();

            if (currentLevel is not > 1)
                return new List<int>();

            var prevLevel = currentLevel.Value - 1;

            return _session.Query<SALSOPDocumentFlow>()
                .Where(x => x.Level == prevLevel)
                .Select(x => x.DocumentType.ID)
                .ToList();
        }

        private List<int> GatherAllowedTypesFromContext()
        {
            var ids = new List<int>();
            var ctx = _businessContext.BusinessContext;

            if (ctx.ContainsKey(Key_Proforma))
                ids.Add((int)SOPDocumentTypeId.ProformaInvoice);

            if (ctx.ContainsKey(Key_Order)) ids.Add((int)SOPDocumentTypeId.Order);
            if (ctx.ContainsKey(Key_Contract)) ids.Add((int)SOPDocumentTypeId.Contract);
            if (ctx.ContainsKey(Key_Invoice)) ids.Add((int)SOPDocumentTypeId.Invoice);
            if (ctx.ContainsKey(Key_InvoiceIssuanceRequest)) ids.Add((int)SOPDocumentTypeId.InvoiceIssuanceRequest);
            if (ctx.ContainsKey(Key_PriceProposal)) ids.Add((int)SOPDocumentTypeId.PriceProposal);

            return ids;
        }

        private bool TryGetIntFromContext(string key, out int value)
        {
            value = default;
            if (!_businessContext.BusinessContext.ContainsKey(key)) return false;
            var raw = _businessContext[key]?.ToString()?.Trim();
            return int.TryParse(raw, out value);
        }

        private bool TryGetCustomerId(out int custId)
        {
            custId = default;
            foreach (var key in CustomerIdKeys)
            {
                if (TryGetIntFromContext(key, out var val) && val > 0)
                {
                    custId = val;
                    return true;
                }
            }
            return false;
        }
    }
}
