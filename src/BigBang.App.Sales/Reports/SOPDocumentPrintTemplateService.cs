using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BigBang.App.sales.Interfaces;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Services.Print;
using BigBang.WebServer.Common.Services.Print.DataSources;
using BigBang.WebServer.Common.Services.ServerInfo;


namespace BigBang.App.Sales.Reports
{
    class SOPDocumentPrintTemplateService : IPrintTemplateSource
    {
        public Task<IPrintDataSource> LoadAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken = default)
        {
            parameters.TryGetValue("ID", out object id);
            string _companyTitle = ServiceLocator.Get<IServerInfoService>().GetLicenseInfo().Title;

            IList<SOPDocumentPrintDTO> _documentLists = ServiceLocator.Get<IDocumentPrintService>().GetDocuments((int)id);

            SOPDocumentPrintParameter ValueResult = new SOPDocumentPrintParameter(_documentLists, _companyTitle);
            SinglePrintDataSource searchResult;
            searchResult = new SinglePrintDataSource { Data = ValueResult };
            return Task.FromResult<IPrintDataSource>(searchResult);
        }
    }

    class SOPDocumentPrintParameter
    {
        public SOPDocumentPrintParameter(IList<SOPDocumentPrintDTO> documentLists, string companyTitle)
        {
            DocumentLists = documentLists;
            CompanyTitle = companyTitle;
        }

        public IList<SOPDocumentPrintDTO> DocumentLists { get; set; }
        public string CompanyTitle { get; set; }



    }
}
