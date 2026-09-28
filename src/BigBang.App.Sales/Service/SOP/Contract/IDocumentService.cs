using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;

namespace BigBang.App.Sales.Service.SOP.Contract
{
    public interface IDocumentService
    {
        void DocumentValidation(SALSOPDocument document);
        int GetNextDocumentNumber(string documentTypeName);
        SALSOPDocument GetByID(int id);
        void DocumentstateMachineValidationForAdding(SALSOPDocument document);
        void ValidateContractNumber(SALSOPDocument document);
        void ValidateUniqueTaxIdentifier(SALSOPDocument document);
        testTuple ValidateConfirmDiscardedStatus(SALSOPDocument document, SOPChangeStateHistoryEntityStateType? oldDocumentState = null, string statusTitle = null);
        bool ValidateEntityStatusToMovingToForward(SALSOPDocument document, SOPChangeStateHistoryEntityStateType? oldDocumentState = null);
        bool IsDefinedNewDocument(SALSOPDocument document);
    }

    public class testTuple
    {
        public bool Item1 { get; set; }
        public string Item2 { get; set; }
    }
}
