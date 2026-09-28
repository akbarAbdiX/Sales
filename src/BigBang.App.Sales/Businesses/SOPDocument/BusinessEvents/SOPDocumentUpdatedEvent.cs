using System;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BigBang.App.Sales.Businesses.SOPDocumentAdjustment.BusinessEvents;

[EventSubscription(typeof(IBusinessUpdatedEvent<SALSOPDocument>))]
public class SOPDocumentUpdatedEvent : IBusinessUpdatedEvent<SALSOPDocument>
{
    private readonly ICheckAccess _checkAccess;
    private readonly IBusinessContext _businessContext;
    private readonly IChangeStateHistoryService _changeStateHistoryService;

    public SOPDocumentUpdatedEvent(ICheckAccess checkAccess,
                                   IBusinessContext businessContext,
                                   IChangeStateHistoryService changeStateHistoryService)
    {
        _checkAccess = checkAccess;
        _businessContext = businessContext ?? throw new ArgumentNullException(nameof(businessContext));
        _changeStateHistoryService = changeStateHistoryService ?? throw new ArgumentNullException(nameof(changeStateHistoryService));
    }

    public void OnUpdated(IBusinessUpdatedEventContext<SALSOPDocument> context)
    {
        _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)context.Entity.DocumentType.Code,
                                       context.Entity.ID,
                                       (SOPChangeStateHistoryEntityStateType)context.Entity.StateID,
                                       context.Entity.StateID == SOPChangeStateHistoryEntityStateType.Rejected ? context.Entity.RejectionReason : "");

    }
}