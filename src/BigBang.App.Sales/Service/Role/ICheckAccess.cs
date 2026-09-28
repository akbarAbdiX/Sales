namespace BigBang.App.Sales.Service.Role
{
    public interface ICheckAccess
    {
        public void CheckDocumentsAccess<T>(string businessContextName,
                                            string claimName,
                                            T expectedResult,
                                            string message = null);

    }
}