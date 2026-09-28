using System;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Common.Extensions;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Interfaces.ChangeStateHistoryReportService;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Services;


namespace BigBang.App.Sales.Service.SOP.Concrete;

[InternalService(ServiceType = typeof(IChangeStateHistoryService), InstanceMode = InstanceMode.Scoped)]
internal class ChangeStateHistoryService : IChangeStateHistoryService
{
    private readonly IChangeStateHistoryRepository _changeStateHistoryRepository;
    private readonly IBigBangIdentityService _bigBangIdentityService;
    private readonly ISessionLoader _sessionLoader;


    public ChangeStateHistoryService(IChangeStateHistoryRepository changeStateHistoryRepository,
                                     IBigBangIdentityService bigBangIdentityService,
                                     ISessionLoader sessionLoader)
    {
        _changeStateHistoryRepository = changeStateHistoryRepository ?? throw new ArgumentNullException(nameof(changeStateHistoryRepository));
        _bigBangIdentityService = bigBangIdentityService ?? throw new ArgumentNullException(nameof(bigBangIdentityService));
        _sessionLoader = sessionLoader ?? throw new ArgumentNullException(nameof(sessionLoader));
    }

    public void Add(SOPChangeStateHistoryEntityType EntityType,
                    long entityId,
                    SOPChangeStateHistoryEntityStateType EntityStateType,
                    string comment)
    {

        /*
         * استخراج آخرین آیتم بالای استک برای آیتم جاری
         * یا به عبارت دیگر پیدا کردن آخرین آیتم قبلی
         * و نال کردن آن
         */
        var latestState = GetCurrentStateByEntity(EntityType, entityId);

        /*در صورتی که وضعیت مورد درخواست با وضعیت جاری در بالای استک یکی باشند مجاز به ثبت مجدد نیست.*/
        if (latestState != null && latestState.EntityStateTypeId == EntityStateType) return;

        if (latestState != null)
        {
            latestState.IsCurrent = false;
            _changeStateHistoryRepository.Update(latestState);
        }

        string id = _bigBangIdentityService.Identity.Actor.Id;

        var changeStateHistory = new SALSOPChangeStateHistory/*اضافه کردن آیتم جدید به بالای استک*/
        {
            EntityId = entityId,
            EntityTypeId = EntityType,
            EntityTypeTitle = EntityType.GetDescription(),
            EntityStateTypeId = EntityStateType,
            EntityStateTypeTitle = EntityStateType.GetDescription(),
            UserId = Convert.ToInt64(_bigBangIdentityService.Identity.Actor.Id),
            IsCurrent = true,
            CreatedDate = DateTime.UtcNow,
            Comment = comment
        };

        _changeStateHistoryRepository.Insert(changeStateHistory);
    }

    public List<ChangeStateHistoryReportDTO> GetChangeStateHistoryReport()
    {
        return _changeStateHistoryRepository.GetChangeStateHistoryReport();
    }

    public SALSOPChangeStateHistory GetCurrentStateByEntity(SOPChangeStateHistoryEntityType entityTypeId, long entityId)
    {
        var csh = _changeStateHistoryRepository.GetMany(x => x.IsCurrent &&
                                                             x.EntityTypeId == entityTypeId &&
                                                             x.EntityId == entityId);

        return csh.SingleOrDefault();

    }

    public void Validate(SALSOPChangeStateHistory changeStateHistory)
    {

    }

    public void ValidateContractNumber(SALSOPChangeStateHistory changeStateHistory)
    {

    }
}
