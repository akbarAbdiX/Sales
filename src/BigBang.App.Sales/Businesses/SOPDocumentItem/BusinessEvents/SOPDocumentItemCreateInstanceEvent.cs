using System;
using System.Linq;
using BigBang.App.Sales.Common.Constants;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPDocumentItem.BusinessEvents;

[EventSubscription(typeof(IBusinessCreateInstanceEvent<SALSOPDocumentItem>))]
public class SOPDocumentItemCreateInstanceEvent : IBusinessCreateInstanceEvent<SALSOPDocumentItem>
{
    private readonly ICheckAccess _checkAccess;
    private readonly IBusinessContext _businessContext;
    private readonly ISession _session;
    private readonly IDocumentService _documentService;
    public SOPDocumentItemCreateInstanceEvent(ICheckAccess checkAccess,
                                              IBusinessContext businessContext,
                                              ISession session,
                                              IDocumentService documentService)
    {
        _checkAccess = checkAccess ?? throw new ArgumentNullException(nameof(checkAccess));
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _businessContext = businessContext ?? throw new ArgumentNullException(nameof(businessContext));
    }
    public void OnCreateInstance(IBusinessCreateInstanceEventContext<SALSOPDocumentItem> context)
    {
        _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToInvoiceItems", "SalesRepresentativ", true);

        var res = _documentService.ValidateConfirmDiscardedStatus(context.Entity.Document);
        if (!res.Item1)
        {
            throw new BigBangException(res.Item2);
        }
    }
}