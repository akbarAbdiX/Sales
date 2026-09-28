using System;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPDocumentAdjustment.BusinessEvents;

[EventSubscription(typeof(IBusinessAddedEvent<SALSOPDocument>))]
public class SOPDocumentAddedEvent : IBusinessAddedEvent<SALSOPDocument>
{
    private readonly ICheckAccess _checkAccess;
    private readonly IBusinessContext _businessContext;
    private readonly IChangeStateHistoryService _changeStateHistoryService;

    public SOPDocumentAddedEvent(ICheckAccess checkAccess,
                                 IBusinessContext businessContext,
                                 IChangeStateHistoryService changeStateHistoryService)
    {
        _checkAccess = checkAccess ?? throw new ArgumentNullException(nameof(checkAccess));
        _businessContext = businessContext ?? throw new ArgumentNullException(nameof(businessContext));
        _changeStateHistoryService = changeStateHistoryService ?? throw new ArgumentNullException(nameof(changeStateHistoryService));
    }

    public void OnAdded(IBusinessAddedEventContext<SALSOPDocument> context)
    {
        #region [ Comment ]

        /*در این قسمت باید بر اساس اینکه فرم و تایپ انتیتی چه هستند تصمیم گرفته بشود*/
        //if (_businessContext.BusinessContext.ContainsKey("DocumentTypeInvoiceIssuanceRequest"))
        //{
        //    _changeStateHistoryService.Add(SOPChangeStateHistoryEntityType.Document,
        //                                   context.Entity.ID,
        //                                   SOPChangeStateHistoryEntityStateType.Draft,
        //                                   "");
        //}

        #endregion

        _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)context.Entity.DocumentType.Code,
                                       context.Entity.ID,
                                       SOPChangeStateHistoryEntityStateType.Draft,
                                       "");

    }

}