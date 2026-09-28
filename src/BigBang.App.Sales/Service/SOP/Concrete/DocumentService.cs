using System;
using System.Security.Claims;
using BigBang.App.Sales.Common.Constants;
using BigBang.App.Sales.Common.Exceptions;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Exceptions;
using BigBang.WebServer.Common.Services;
using static BigBang.App.Sales.Common.Constants.SaleConstant;

using System.Linq;



namespace BigBang.App.Sales.Service.SOP.Concrete;

[InternalService(ServiceType = typeof(IDocumentService), InstanceMode = InstanceMode.PerRequest)]
internal class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private readonly IChangeStateHistoryService _changeStateHistoryService;
    private readonly IBigBangIdentityService _identityService;

    public DocumentService(IDocumentRepository documentRepository,
                           IDocumentTypeRepository documentTypeRepository,
                           IChangeStateHistoryService changeStateHistoryService,
                           IBigBangIdentityService identityService)
    {
        _documentRepository = documentRepository;
        _documentTypeRepository = documentTypeRepository;
        _changeStateHistoryService = changeStateHistoryService;
        _identityService = identityService;
    }

    public void DocumentValidation(SALSOPDocument document)
    {
        if (document.DocumentType.Name == Invoice)
        {
            bool duplicateExists = _documentRepository.IsExists(i =>
                i.No == document.No &&
                i.IssueDate == document.IssueDate &&
                i.DocumentType.Name == Invoice);

            if (duplicateExists)
                throw Exceptions.Raise(ExceptionCode.DocumentNumberIsDuplicate);
            var lastInvoice = _documentRepository.GetLastOrDefaultOrdered(c => true, i => i.IssueDate);
            if (lastInvoice != null)
            {
                bool invalidOrder = (document.IssueDate < lastInvoice.IssueDate) ||
                                    (document.IssueDate == lastInvoice.IssueDate && document.No < lastInvoice.No);
                if (invalidOrder)
                    throw Exceptions.Raise(ExceptionCode.DocumentNumberOrDateIsInvalid);
            }
        }

    }
    public int GetNextDocumentNumber(string documentTypeName)
    {
        var document = _documentRepository.GetLastOrDefaultOrdered(c => c.DocumentType.Name == documentTypeName, c => c.No);
        return document == null ? 1 : document.No + 1;
    }

    public SALSOPDocument GetByID(int id)
    {
        return _documentRepository.GetFirstOrDefault(c => c.ID == id);
    }

    public bool IsDefinedNewDocument(SALSOPDocument document)
    {
        /*در صورتی آیتمی با این شروط وجود داشته باشد*/
        var exDocument = _documentRepository.GetFirstOrDefault(c => c.Document.ID == document.ID && c.StateID == SOPChangeStateHistoryEntityStateType.Draft);
        return exDocument != null;
    }

    public void DocumentstateMachineValidationForAdding(SALSOPDocument document)
    {
        if (document.DocumentType.Name != Invoice) return;
        SALSOPDocument lastInvoice = _documentRepository
            .GetLastOrDefaultOrdered(x => x.DocumentType.Name == Invoice, x => x.No);
        if (lastInvoice == null)
            return;

        if (lastInvoice.StateID == null)
            throw Exceptions.Raise(ExceptionCode.LastDocumentIsNotConfiermd);
    }

    public void ValidateContractNumber(SALSOPDocument document)  //TODO 
    {
        var contract = _documentTypeRepository.GetSingleOrDefault(documentType => documentType.Name == SaleConstant.Contract);

        if (document.DocumentType.ID == contract.ID && document.StateID == SOPChangeStateHistoryEntityStateType.Confirmed)
        {
            if (string.IsNullOrWhiteSpace(document.ContractNumber))
            {
                throw new BigBangException("شماره قرارداد باید مقدار داشته باشد.");
            }

            var duplicatedDocument = _documentRepository.GetFirstOrDefault(d => d.ContractNumber == document.ContractNumber && d.ID != document.ID);

            if (duplicatedDocument != null)
            {
                throw new BigBangException("شماره قرار داد تکراری است.");
            }

        }
    }

    public void ValidateUniqueTaxIdentifier(SALSOPDocument document)
    {
        var invoiceIssuanceRequest = _documentTypeRepository
            .GetSingleOrDefault(x => x.Name == SaleConstant.InvoiceIssuanceRequest);

        var invoice = _documentTypeRepository
            .GetSingleOrDefault(x => x.Name == SaleConstant.Invoice);

        var isInvoiceDocument =
            document.DocumentType.ID == invoiceIssuanceRequest.ID ||
            document.DocumentType.ID == invoice.ID;

        if (!isInvoiceDocument)
            return;

        var uniqueTaxIdentifierLength = document.UniqueTaxIdentifier?.Length;

        if (uniqueTaxIdentifierLength > 20)
        {
            throw new BigBangException(
                "شماره منحصر به فرد مالیاتی نباید بیشتر از 20 رقم باشد.");
        }

        if (document.StateID == SOPChangeStateHistoryEntityStateType.Confirmed &&
            uniqueTaxIdentifierLength == null &&
            (document.DocumentType.Code == 6)
            )
        {
            throw new BigBangException("شماره منحصر به فرد مالیاتی باید مقدار داشته باشد.");
        }
    }

    public testTuple ValidateConfirmDiscardedStatus(SALSOPDocument document, SOPChangeStateHistoryEntityStateType? oldDocumentState = null, string statusTitle = null)
    {
        var currentState = _changeStateHistoryService.GetCurrentStateByEntity((SOPChangeStateHistoryEntityType)document.DocumentType.Code, document.ID);

        var curState = currentState != null ? currentState.EntityStateTypeId : document.StateID;
        if (oldDocumentState != null && document.StateID != oldDocumentState)
            curState = oldDocumentState;

        switch (curState)
        {
            case SOPChangeStateHistoryEntityStateType.Confirmed:

                if (ValidateEntityStatusToApproveAfterConfirmed(document))
                    return new testTuple { Item1 = true, Item2 = "" };

                statusTitle = !string.IsNullOrEmpty(statusTitle) ? statusTitle : "ثبت مجدد";
                if (document.StateID == SOPChangeStateHistoryEntityStateType.ApprovedBySalesManagment)
                    return new testTuple { Item1 = false, Item2 = $"امکان {statusTitle} در وضعیت ثبت نهایی سند وجود ندارد و یا کاربر جاری فاقد نقش مدیریت فروش می باشد." };
                return new testTuple { Item1 = false, Item2 = $"امکان {statusTitle} در وضعیت ثبت نهایی سند وجود ندارد." };

            case SOPChangeStateHistoryEntityStateType.Discarded:
                return new testTuple { Item1 = false, Item2 = $"اقدام برای سندی که ابطال شده است، قابل انجام نمی باشد" };

            default:
                return new testTuple { Item1 = true, Item2 = "" };
        }
    }

    public bool ValidateEntityStatusToMovingToForward(SALSOPDocument document, SOPChangeStateHistoryEntityStateType? oldDocumentState = null)
    {
        var currentState = _changeStateHistoryService.GetCurrentStateByEntity((SOPChangeStateHistoryEntityType)document.DocumentType.Code, document.ID);

        if (currentState == null)
        {
            if (oldDocumentState != null && document.StateID != oldDocumentState)
                return true;
        }

        if (currentState?.EntityStateTypeId != document.StateID)
            return true;

        return false;
    }

    public bool ValidateEntityStatusToApproveAfterConfirmed(SALSOPDocument document)
    {
        /*
         * وضعیتی که در این حالت مجاز است تایید توسط کاربری هست که نقش مدیر فروش را دارد
         * در این قسمت باید چند مورد چک بشود:
         * 1. نقش کاربری که دارد فعل را انجام میدهد
         * 2. وضعیتی که در این حالت مجاز را توجیح می کند که آنهم تایید می باشد
         */

        if (!_identityService.Identity.Claims.TryGetValue("Roles", out Claim? claim))
            return false;

        var hasClaim = claim.Value
                            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                            .Contains("SalesManagment", StringComparer.OrdinalIgnoreCase);

        if ((document.StateID == SOPChangeStateHistoryEntityStateType.ApprovedBySalesManagment ||
             document.StateID == SOPChangeStateHistoryEntityStateType.RejectedBySalesManagment) && hasClaim)
            return true;

        return false;
    }
}
