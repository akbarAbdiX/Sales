using System;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPDocumentAdjustment.BusinessEvents;

[EventSubscription(typeof(IBusinessCreateInstanceEvent<SALSOPDocumentAdjustment>))]
public class SOPDocumentAdjustmentCreateInstanceEvent : IBusinessCreateInstanceEvent<SALSOPDocumentAdjustment>
{

    private readonly ICheckAccess _checkAccess;
    private readonly IBusinessContext _businessContext;
    private readonly ISession _session;
    private readonly IDocumentService _documentService;
    public SOPDocumentAdjustmentCreateInstanceEvent(ICheckAccess checkAccess,
                                                    IBusinessContext businessContext,
                                                    ISession session,
                                                    IDocumentService documentService)
    {
        _checkAccess = checkAccess ?? throw new ArgumentNullException(nameof(checkAccess));
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _businessContext = businessContext ?? throw new ArgumentNullException(nameof(businessContext));
    }

    public void OnCreateInstance(IBusinessCreateInstanceEventContext<SALSOPDocumentAdjustment> context)
    {
        _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToInvoiceAdjustment", "SalesRepresentativ", true);

        var res = _documentService.ValidateConfirmDiscardedStatus(context.Entity.DocumentItem.Document);
        if (!res.Item1)
        {
            throw new BigBangException(res.Item2);
        }
        context.Entity.ParameterAmount = 0;
    }
}