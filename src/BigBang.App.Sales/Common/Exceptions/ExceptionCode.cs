namespace BigBang.App.Sales.Common.Exceptions
{
    public enum ExceptionCode
    {
        //Document exception(1xxx)
        DocumentNumberOrDateIsInvalid = 1001,
        DocumentNumberIsDuplicate = 1002,
        DocumentStateIsInvalid = 1003,
        LastDocumentIsNotConfiermd = 1004,

        // ProductAndServiceRate exceptions (3XXX)
        ProductAndServiceRateDateWasExpier = 3001,
        ProductAndServiceRateNotValidOnDocumentDate = 3002
    }
}
