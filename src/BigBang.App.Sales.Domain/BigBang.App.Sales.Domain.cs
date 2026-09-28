using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Collections.Generic;
using BigBang.Metadata.Models;
using BigBang.Metadata.Models.Command;
using BigBang.Metadata.Models.EntityModel;
using BigBang.Metadata.Models.JsonConverters;
using BigBang.Metadata.Models.MessageCode;
using Newtonsoft.Json;

[assembly: ApplicationAttribute("SAL", "Sales", "BigBang.App.Sales.Domain")]
namespace BigBang.App.Sales.Domain
{
    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPAddendum : IEntity, IEquatable<SALSOPAddendum>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int ID { get; set; }
        public virtual DateTime AmendmentDate { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal? Amount { get; set; }
        public virtual string Notes { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual SALSOPAddendumType AddendumType { get; set; }
        public virtual SALSOPDocument Document { get; set; }

        public virtual bool Equals(SALSOPAddendum other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPAddendum);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPAddendum left, SALSOPAddendum right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPAddendum left, SALSOPAddendum right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPAddendumType : IEntity, IEquatable<SALSOPAddendumType>
    {
        public SALSOPAddendumType()
        {
            SOPAddendums = new List<SALSOPAddendum>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPAddendum> SOPAddendums { get; set; }
        public virtual int ID { get; set; }
        public virtual int Code { get; set; }
        public virtual string Title { get; set; }
        public virtual bool TimeChange { get; set; }
        public virtual bool AmountChange { get; set; }

        public virtual bool Equals(SALSOPAddendumType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPAddendumType);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPAddendumType left, SALSOPAddendumType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPAddendumType left, SALSOPAddendumType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPAdjustment : IEntity, IEquatable<SALSOPAdjustment>
    {
        public SALSOPAdjustment()
        {
            SOPDocumentAdjustments = new List<SALSOPDocumentAdjustment>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual int ID { get; set; }
        public virtual int Code { get; set; }
        public virtual decimal Rate { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual string Title { get; set; }
        public virtual bool IsAppliedTax { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual SALSOPAdjustmentType AdjustmentType { get; set; }
        public virtual IList<SALSOPDocumentAdjustment> SOPDocumentAdjustments { get; set; }

        public virtual bool Equals(SALSOPAdjustment other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPAdjustment);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPAdjustment left, SALSOPAdjustment right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPAdjustment left, SALSOPAdjustment right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPAdjustmentSign : IEntity, IEquatable<SALSOPAdjustmentSign>
    {
        public SALSOPAdjustmentSign()
        {
            SOPAdjustmentTypes = new List<SALSOPAdjustmentType>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual int ID { get; set; }
        public virtual short Value { get; set; }
        public virtual string Name { get; set; }
        public virtual string Title { get; set; }
        public virtual IList<SALSOPAdjustmentType> SOPAdjustmentTypes { get; set; }

        public virtual bool Equals(SALSOPAdjustmentSign other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPAdjustmentSign);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPAdjustmentSign left, SALSOPAdjustmentSign right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPAdjustmentSign left, SALSOPAdjustmentSign right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPAdjustmentType : IEntity, IEquatable<SALSOPAdjustmentType>
    {
        public SALSOPAdjustmentType()
        {
            SOPAdjustments = new List<SALSOPAdjustment>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPAdjustment> SOPAdjustments { get; set; }
        public virtual int ID { get; set; }
        public virtual int Code { get; set; }
        public virtual string Title { get; set; }
        public virtual string Name { get; set; }
        public virtual SALSOPAdjustmentSign AdjustmentSign { get; set; }

        public virtual bool Equals(SALSOPAdjustmentType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPAdjustmentType);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPAdjustmentType left, SALSOPAdjustmentType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPAdjustmentType left, SALSOPAdjustmentType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPChangeStateHistory : IEntity, IEquatable<SALSOPChangeStateHistory>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual BigBang.App.Sales.Domain.Enums.SOPChangeStateHistoryEntityType EntityTypeId { get; set; }
        public virtual string EntityTypeTitle { get; set; }
        public virtual long EntityId { get; set; }
        public virtual BigBang.App.Sales.Domain.Enums.SOPChangeStateHistoryEntityStateType EntityStateTypeId { get; set; }
        public virtual string EntityStateTypeTitle { get; set; }
        public virtual long UserId { get; set; }
        public virtual bool IsCurrent { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string Comment { get; set; }

        public virtual bool Equals(SALSOPChangeStateHistory other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPChangeStateHistory);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPChangeStateHistory left, SALSOPChangeStateHistory right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPChangeStateHistory left, SALSOPChangeStateHistory right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPContractAdditionalInfo : IEntity, IEquatable<SALSOPContractAdditionalInfo>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int ID { get; set; }
        public virtual string Description { get; set; }
        public virtual SALSOPContractFieldType ContractFieldType { get; set; }
        public virtual SALSOPDocument Document { get; set; }

        public virtual bool Equals(SALSOPContractAdditionalInfo other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPContractAdditionalInfo);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPContractAdditionalInfo left, SALSOPContractAdditionalInfo right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPContractAdditionalInfo left, SALSOPContractAdditionalInfo right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPContractFieldType : IEntity, IEquatable<SALSOPContractFieldType>
    {
        public SALSOPContractFieldType()
        {
            SOPContractAdditionalInfoes = new List<SALSOPContractAdditionalInfo>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPContractAdditionalInfo> SOPContractAdditionalInfoes { get; set; }
        public virtual int ID { get; set; }
        public virtual int Code { get; set; }
        public virtual string FieldName { get; set; }
        public virtual bool IsRequired { get; set; }

        public virtual bool Equals(SALSOPContractFieldType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPContractFieldType);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPContractFieldType left, SALSOPContractFieldType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPContractFieldType left, SALSOPContractFieldType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPContractStage : IEntity, IEquatable<SALSOPContractStage>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual SALSOPDocument Document { get; set; }
        public virtual SALSOPSubContractStage SubContractStage { get; set; }

        public virtual bool Equals(SALSOPContractStage other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPContractStage);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPContractStage left, SALSOPContractStage right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPContractStage left, SALSOPContractStage right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPDocument : IEntity, IEquatable<SALSOPDocument>
    {
        public SALSOPDocument()
        {
            SOPContractStages = new List<SALSOPContractStage>();
            SOPPaymentBreakdowns = new List<SALSOPPaymentBreakdown>();
            SOPPaymentTerms = new List<SALSOPPaymentTerm>();
            SOPAddendums = new List<SALSOPAddendum>();
            SOPContractAdditionalInfoes = new List<SALSOPContractAdditionalInfo>();
            SOPDocuments = new List<SALSOPDocument>();
            SOPDocumentItems = new List<SALSOPDocumentItem>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPContractStage> SOPContractStages { get; set; }
        public virtual IList<SALSOPPaymentBreakdown> SOPPaymentBreakdowns { get; set; }
        public virtual IList<SALSOPPaymentTerm> SOPPaymentTerms { get; set; }
        public virtual IList<SALSOPAddendum> SOPAddendums { get; set; }
        public virtual IList<SALSOPContractAdditionalInfo> SOPContractAdditionalInfoes { get; set; }
        public virtual int ID { get; set; }
        public virtual int No { get; set; }
        public virtual DateTime IssueDate { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal DiscountAmount { get; set; }
        public virtual decimal AdditionalChargesAmount { get; set; }
        public virtual decimal TaxAmount { get; set; }
        public virtual decimal TotalAmount { get; set; }
        public virtual string Description { get; set; }
        public virtual BigBang.App.Sales.Domain.Enums.SOPChangeStateHistoryEntityStateType? StateID { get; set; }
        public virtual string UniqueTaxIdentifier { get; set; }
        public virtual string ContractNumber { get; set; }
        public virtual string RejectionReason { get; set; }
        public virtual int? Color { get; set; }
        public virtual BigBang.App.Sales.Domain.Enums.SOPSettlementMethod? PaymentTermsType { get; set; }
        public virtual long? PaymentTermAmount { get; set; }
        public virtual SALSOPDocument Document { get; set; }
        public virtual IList<SALSOPDocument> SOPDocuments { get; set; }
        public virtual SALSOPDocumentType DocumentType { get; set; }
        public virtual SALSOPProductType ProductType { get; set; }
        public virtual SALSOPSeller Seller { get; set; }
        public virtual IList<SALSOPDocumentItem> SOPDocumentItems { get; set; }
        public virtual BigBang.App.ERPSuiteCore.Domain.SuiteCoreParty CustomerID { get; set; }
        public virtual BigBang.App.ERPSuiteCore.Domain.SuiteCoreAddress CustomerAddress { get; set; }

        public virtual bool Equals(SALSOPDocument other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPDocument);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPDocument left, SALSOPDocument right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPDocument left, SALSOPDocument right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPDocumentAdjustment : IEntity, IEquatable<SALSOPDocumentAdjustment>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int ID { get; set; }
        public virtual decimal AdjustmentAmount { get; set; }
        public virtual decimal ParameterAmount { get; set; }
        public virtual SALSOPAdjustment Adjustment { get; set; }
        public virtual SALSOPDocumentItem DocumentItem { get; set; }

        public virtual bool Equals(SALSOPDocumentAdjustment other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPDocumentAdjustment);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPDocumentAdjustment left, SALSOPDocumentAdjustment right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPDocumentAdjustment left, SALSOPDocumentAdjustment right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPDocumentFlow : IEntity, IEquatable<SALSOPDocumentFlow>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int ID { get; set; }
        public virtual int Level { get; set; }
        public virtual SALSOPDocumentType DocumentType { get; set; }

        public virtual bool Equals(SALSOPDocumentFlow other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPDocumentFlow);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPDocumentFlow left, SALSOPDocumentFlow right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPDocumentFlow left, SALSOPDocumentFlow right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPDocumentItem : IEntity, IEquatable<SALSOPDocumentItem>
    {
        public SALSOPDocumentItem()
        {
            SOPDocumentAdjustments = new List<SALSOPDocumentAdjustment>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPDocumentAdjustment> SOPDocumentAdjustments { get; set; }
        public virtual int ID { get; set; }
        public virtual decimal? Amount { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal AdditionalCharge { get; set; }
        public virtual decimal Tax { get; set; }
        public virtual decimal TotalPrice { get; set; }
        public virtual string Description { get; set; }
        public virtual string PrintDescription { get; set; }
        public virtual SALSOPDocument Document { get; set; }
        public virtual SALSOPProductAndServiceRate ProductAndServiceRate { get; set; }
        public virtual string ProductServiceCode { get; set; }
        public virtual BigBang.App.ERPSuiteCore.Domain.SuiteCoreProductDetail ItemID { get; set; }

        public virtual bool Equals(SALSOPDocumentItem other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPDocumentItem);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPDocumentItem left, SALSOPDocumentItem right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPDocumentItem left, SALSOPDocumentItem right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPDocumentType : IEntity, IEquatable<SALSOPDocumentType>
    {
        public SALSOPDocumentType()
        {
            SOPDocuments = new List<SALSOPDocument>();
            SOPDocumentFlows = new List<SALSOPDocumentFlow>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPDocument> SOPDocuments { get; set; }
        public virtual IList<SALSOPDocumentFlow> SOPDocumentFlows { get; set; }
        public virtual int ID { get; set; }
        public virtual int Code { get; set; }
        public virtual string Title { get; set; }
        public virtual string Name { get; set; }

        public virtual bool Equals(SALSOPDocumentType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPDocumentType);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPDocumentType left, SALSOPDocumentType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPDocumentType left, SALSOPDocumentType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPDueType : IEntity, IEquatable<SALSOPDueType>
    {
        public SALSOPDueType()
        {
            SOPPaymentTerms = new List<SALSOPPaymentTerm>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual int Code { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<SALSOPPaymentTerm> SOPPaymentTerms { get; set; }

        public virtual bool Equals(SALSOPDueType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPDueType);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPDueType left, SALSOPDueType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPDueType left, SALSOPDueType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPGuaranteeType : IEntity, IEquatable<SALSOPGuaranteeType>
    {
        public SALSOPGuaranteeType()
        {
            SOPPaymentTerms = new List<SALSOPPaymentTerm>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual int Code { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<SALSOPPaymentTerm> SOPPaymentTerms { get; set; }

        public virtual bool Equals(SALSOPGuaranteeType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPGuaranteeType);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPGuaranteeType left, SALSOPGuaranteeType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPGuaranteeType left, SALSOPGuaranteeType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPMainContractStage : IEntity, IEquatable<SALSOPMainContractStage>
    {
        public SALSOPMainContractStage()
        {
            SOPSubContractStages = new List<SALSOPSubContractStage>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual int Code { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<SALSOPSubContractStage> SOPSubContractStages { get; set; }

        public virtual bool Equals(SALSOPMainContractStage other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPMainContractStage);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPMainContractStage left, SALSOPMainContractStage right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPMainContractStage left, SALSOPMainContractStage right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPPaymentBreakdown : IEntity, IEquatable<SALSOPPaymentBreakdown>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual string PhaseName { get; set; }
        public virtual string Title { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual DateTime DueDate { get; set; }
        public virtual SALSOPDocument Document { get; set; }

        public virtual bool Equals(SALSOPPaymentBreakdown other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPPaymentBreakdown);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPPaymentBreakdown left, SALSOPPaymentBreakdown right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPPaymentBreakdown left, SALSOPPaymentBreakdown right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPPaymentTerm : IEntity, IEquatable<SALSOPPaymentTerm>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual decimal PerAmount { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual DateTime? DueDate { get; set; }
        public virtual bool? IsDeductible { get; set; }
        public virtual bool? IsGuarantee { get; set; }
        public virtual string ReleaseCondition { get; set; }
        public virtual string Description { get; set; }
        public virtual BigBang.App.Sales.Domain.Enums.SOPSettlementMethod? SettlementMethod { get; set; }
        public virtual SALSOPDocument Document { get; set; }
        public virtual SALSOPDueType DueType { get; set; }
        public virtual SALSOPGuaranteeType GuaranteeType { get; set; }
        public virtual SALSOPPaymentTermsType PaymentTermsType { get; set; }

        public virtual bool Equals(SALSOPPaymentTerm other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPPaymentTerm);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPPaymentTerm left, SALSOPPaymentTerm right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPPaymentTerm left, SALSOPPaymentTerm right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPPaymentTermsType : IEntity, IEquatable<SALSOPPaymentTermsType>
    {
        public SALSOPPaymentTermsType()
        {
            SOPPaymentTerms = new List<SALSOPPaymentTerm>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPPaymentTerm> SOPPaymentTerms { get; set; }
        public virtual int Id { get; set; }
        public virtual int Code { get; set; }
        public virtual string Description { get; set; }

        public virtual bool Equals(SALSOPPaymentTermsType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPPaymentTermsType);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPPaymentTermsType left, SALSOPPaymentTermsType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPPaymentTermsType left, SALSOPPaymentTermsType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPPaymentVatShare : IEntity, IEquatable<SALSOPPaymentVatShare>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual long VatShare { get; set; }

        public virtual bool Equals(SALSOPPaymentVatShare other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPPaymentVatShare);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPPaymentVatShare left, SALSOPPaymentVatShare right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPPaymentVatShare left, SALSOPPaymentVatShare right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPProductAndServiceRate : IEntity, IEquatable<SALSOPProductAndServiceRate>
    {
        public SALSOPProductAndServiceRate()
        {
            SOPDocumentItems = new List<SALSOPDocumentItem>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPDocumentItem> SOPDocumentItems { get; set; }
        public virtual int ID { get; set; }
        public virtual decimal BasePrice { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string ProductServiceCode { get; set; }
        public virtual BigBang.App.ERPSuiteCore.Domain.SuiteCoreProductDetail ItemID { get; set; }

        public virtual bool Equals(SALSOPProductAndServiceRate other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPProductAndServiceRate);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPProductAndServiceRate left, SALSOPProductAndServiceRate right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPProductAndServiceRate left, SALSOPProductAndServiceRate right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPProductDetailTaxExemption : IEntity, IEquatable<SALSOPProductDetailTaxExemption>
    {
        public virtual EntityState EntityState { get; set; }
        public virtual int ID { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual SALSOPTaxExemptionType TaxExemptionType { get; set; }
        public virtual BigBang.App.ERPSuiteCore.Domain.SuiteCoreProductDetail ProductDetailId { get; set; }

        public virtual bool Equals(SALSOPProductDetailTaxExemption other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPProductDetailTaxExemption);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPProductDetailTaxExemption left, SALSOPProductDetailTaxExemption right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPProductDetailTaxExemption left, SALSOPProductDetailTaxExemption right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPProductType : IEntity, IEquatable<SALSOPProductType>
    {
        public SALSOPProductType()
        {
            SOPDocuments = new List<SALSOPDocument>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<SALSOPDocument> SOPDocuments { get; set; }

        public virtual bool Equals(SALSOPProductType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPProductType);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPProductType left, SALSOPProductType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPProductType left, SALSOPProductType right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPSeller : IEntity, IEquatable<SALSOPSeller>
    {
        public SALSOPSeller()
        {
            SOPDocuments = new List<SALSOPDocument>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int? EconomicCode { get; set; }
        public virtual int? BranchCode { get; set; }
        public virtual string Address { get; set; }
        public virtual int? PostalCode { get; set; }
        public virtual int? PhoneNumber { get; set; }
        public virtual int? FaxNumber { get; set; }
        public virtual int? RegistrationNumber { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual IList<SALSOPDocument> SOPDocuments { get; set; }

        public virtual bool Equals(SALSOPSeller other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPSeller);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPSeller left, SALSOPSeller right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPSeller left, SALSOPSeller right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPSubContractStage : IEntity, IEquatable<SALSOPSubContractStage>
    {
        public SALSOPSubContractStage()
        {
            SOPContractStages = new List<SALSOPContractStage>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPContractStage> SOPContractStages { get; set; }
        public virtual int Id { get; set; }
        public virtual int Code { get; set; }
        public virtual string Description { get; set; }
        public virtual SALSOPMainContractStage MainContractStage { get; set; }

        public virtual bool Equals(SALSOPSubContractStage other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPSubContractStage);
        }

        public override int GetHashCode()
        {
            if (Id == default)
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(SALSOPSubContractStage left, SALSOPSubContractStage right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPSubContractStage left, SALSOPSubContractStage right)
        {
            return !(left == right);
        }
    }

    [Domain]
    [JsonObject]
    [Serializable]
    public partial class SALSOPTaxExemptionType : IEntity, IEquatable<SALSOPTaxExemptionType>
    {
        public SALSOPTaxExemptionType()
        {
            SOPProductDetailTaxExemptions = new List<SALSOPProductDetailTaxExemption>();
        }

        public virtual EntityState EntityState { get; set; }
        public virtual IList<SALSOPProductDetailTaxExemption> SOPProductDetailTaxExemptions { get; set; }
        public virtual int ID { get; set; }
        public virtual int Code { get; set; }
        public virtual string Description { get; set; }

        public virtual bool Equals(SALSOPTaxExemptionType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (ID == other.ID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SALSOPTaxExemptionType);
        }

        public override int GetHashCode()
        {
            if (ID == default)
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(SALSOPTaxExemptionType left, SALSOPTaxExemptionType right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SALSOPTaxExemptionType left, SALSOPTaxExemptionType right)
        {
            return !(left == right);
        }
    }
}

namespace BigBang.App.Sales.Domain.Reports
{
    public class GetChangeStateHistoryReport
    {
        public virtual int DocumentNo { get; set; }
        public virtual string EntityTypeTitle { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string EntityStateTypeTitle { get; set; }
        public virtual string UserName { get; set; }
    }

    public class GetDocuments
    {
        public virtual int ProformaInvoiceNo { get; set; }
        public virtual DateTime ProformaInvoiceDate { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CurrentStage { get; set; }
        public virtual decimal TotalAmount { get; set; }
        public virtual string Description { get; set; }
        public virtual int ProformaInvoiceID { get; set; }
    }

    public class GetIssuedInvoiceDocuments
    {
        public virtual int InvoiceNo { get; set; }
        public virtual DateTime IssueDate { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string UniqueTaxIdentifier { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal DiscountAmount { get; set; }
        public virtual decimal TaxAmount { get; set; }
        public virtual decimal TotalAmount { get; set; }
        public virtual string Description { get; set; }
        public virtual int DocumentID { get; set; }
    }

    public class GetIssuedInvoiceItems
    {
        public virtual string ProductName { get; set; }
        public virtual string ProductServiceCode { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal Tax { get; set; }
        public virtual decimal TotalPrice { get; set; }
        public virtual string PrintDescription { get; set; }
        public virtual string Description { get; set; }
        public virtual int DocumentID { get; set; }
    }

    public class GetItems
    {
        public virtual string ProductName { get; set; }
        public virtual string ProductServiceCode { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal Tax { get; set; }
        public virtual decimal TotalPrice { get; set; }
        public virtual string PrintDescription { get; set; }
        public virtual string Description { get; set; }
        public virtual int ProformaInvoiceID { get; set; }
    }
}

namespace BigBang.App.Sales.Domain.Enums
{
    public enum SalesSystemModuleType : int
    {
        SuperAdmin = 1,
        Sales = 2,
        Accounting = 3,
        Treasury = 4
    }

    internal class DbSalesSystemModuleTypeEnumType : NHibernate.Type.EnumType<SalesSystemModuleType>
    {
    }

    public enum SOPChangeStateHistoryEntityStateType : int
    {
        Draft = 1,
        Sent = 2,
        Confirmed = 3,
        Discarded = 4,
        Rejected = 5,
        Approved = 6,
        ApprovedBySalesManagment = 7,
        RejectedBySalesManagment = 8
    }

    internal class DbSOPChangeStateHistoryEntityStateTypeEnumType : NHibernate.Type.EnumType<SOPChangeStateHistoryEntityStateType>
    {
    }

    public enum SOPChangeStateHistoryEntityType : int
    {
        ProformaInvoice = 1,
        Order = 2,
        Contract = 3,
        Invoice = 4,
        InvoiceIssuanceRequest = 5,
        PriceProposal = 6
    }

    internal class DbSOPChangeStateHistoryEntityTypeEnumType : NHibernate.Type.EnumType<SOPChangeStateHistoryEntityType>
    {
    }

    public enum SOPCustomerType : int
    {
        Individual = 0,
        Corporate = 1
    }

    internal class DbSOPCustomerTypeEnumType : NHibernate.Type.EnumType<SOPCustomerType>
    {
    }

    public enum SOPItemType : int
    {
        Item = 0,
        Service = 1
    }

    internal class DbSOPItemTypeEnumType : NHibernate.Type.EnumType<SOPItemType>
    {
    }

    public enum SOPSettlementMethod : int
    {
        Cash = 1,
        Credit = 2
    }

    internal class DbSOPSettlementMethodEnumType : NHibernate.Type.EnumType<SOPSettlementMethod>
    {
    }
}

namespace BigBang.App.Sales.Domain.Commands
{
    [DbCommand]
    public interface ISalesCommands
    {
        ISalesCommands WithCommandTimeout(string timeoutPolicy);
        [ApplicationCommand("sp_GetUninvoicedProformaInvoices")]
        IDbCommandResult sp_GetUninvoicedProformaInvoices([ApplicationCommandParameter("@FromDate", "datetime")] DateTime? FromDate, [ApplicationCommandParameter("@ToDate", "datetime")] DateTime? ToDate);
        [ApplicationCommand("sp_GetUninvoicedProformaInvoices")]
        ISingleDbCommandResult<TEntity> sp_GetUninvoicedProformaInvoices<TEntity>([ApplicationCommandParameter("@FromDate", "datetime")] DateTime? FromDate, [ApplicationCommandParameter("@ToDate", "datetime")] DateTime? ToDate);
        [ApplicationCommand("sp_GetUninvoicedProformaInvoices")]
        IMultipleDbCommandResult<TResult> sp_GetUninvoicedProformaInvoicesMultiple<TResult>([ApplicationCommandParameter("@FromDate", "datetime")] DateTime? FromDate, [ApplicationCommandParameter("@ToDate", "datetime")] DateTime? ToDate);
    }
}