namespace BigBang.App.Sales.Common.Exceptions
{
    public class ExceptionDetail
    {
        public ExceptionCode Code { get; set; }
        public string Message { get; set; }
        public ExceptionCategories Category { get; set; }
    }
}
