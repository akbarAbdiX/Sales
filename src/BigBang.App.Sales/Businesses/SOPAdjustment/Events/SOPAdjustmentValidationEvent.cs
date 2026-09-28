using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;
using NHibernate.Criterion;

namespace BigBang.App.Sales.Businesses.SOPAdjustment
{
    [EventSubscription(typeof(IValidationEvent<SALSOPAdjustment>))]
    public class SOPAdjustmentValidationEvent : IValidationEvent<SALSOPAdjustment>
    {
        private readonly ISession _session;

        public SOPAdjustmentValidationEvent(ISession session) => _session = session;

        public void OnValidate(IValidationContext<SALSOPAdjustment> context)
        {
            var entity = context.Entity;

            if (context.ValidationAction == ValidationAction.Add)
            {
                ValidateDateRange(entity);
                ValidateSingleDefaultPerType(entity);
                return;
            }

            if (context.ValidationAction != ValidationAction.Update)
                return;

            var sessionImpl = _session.GetSessionImplementation();
            var persister = sessionImpl.Factory.GetEntityPersister(typeof(SALSOPAdjustment).FullName);

            var entry = sessionImpl.PersistenceContext.GetEntry(entity);
            var previousState = entry?.LoadedState
                ?? sessionImpl.PersistenceContext.GetDatabaseSnapshot(entity.ID, persister);

            if (previousState == null)
                return;

            var currentState = persister.GetPropertyValues(entity);
            var dirtyIndexes = persister.FindDirty(currentState, previousState, entity, sessionImpl);

            if (dirtyIndexes == null || dirtyIndexes.Length == 0)
                return;

            var changedProperties = dirtyIndexes
                .Select(i => persister.PropertyNames[i])
                .ToList();

            if (changedProperties.Contains(nameof(SALSOPAdjustment.FromDate))
                || changedProperties.Contains(nameof(SALSOPAdjustment.ToDate)))
                ValidateDateRange(entity);

            if (changedProperties.Contains(nameof(SALSOPAdjustment.IsDefault))
                || changedProperties.Contains(nameof(SALSOPAdjustment.AdjustmentType)))
                ValidateSingleDefaultPerType(entity);

            if (changedProperties.All(p => p == nameof(SALSOPAdjustment.IsDefault)))
                return;

            var isUsed = _session.QueryOver<SALSOPDocumentAdjustment>()
                .Where(x => x.Adjustment.ID == entity.ID)
                .UnderlyingCriteria.SetFlushMode(FlushMode.Manual)
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>() > 0;

            if (isUsed)
                throw new BigBangException(
                    "این عامل تعدیل در یک یا چند سند استفاده شده است و قابل ویرایش نمی‌باشد.");
        }

        private void ValidateDateRange(SALSOPAdjustment entity)
        {
            if (!entity.ToDate.HasValue)
                return;

            if (entity.ToDate.Value < entity.FromDate)
                throw new BigBangException(
                    "تاریخ «تا تاریخ» نمی‌تواند قبل از «از تاریخ» باشد.");
        }

        private void ValidateSingleDefaultPerType(SALSOPAdjustment entity)
        {
            if (!entity.IsDefault || entity.AdjustmentType == null)
                return;

            var query = _session.QueryOver<SALSOPAdjustment>()
                .Where(x => x.AdjustmentType.ID == entity.AdjustmentType.ID)
                .And(x => x.IsDefault);

            if (entity.ID != 0)
                query = query.And(x => x.ID != entity.ID);

            var duplicateDefaultCount = query.UnderlyingCriteria
                .SetFlushMode(FlushMode.Manual)
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>();

            if (duplicateDefaultCount > 0)
                throw new BigBangException(
                    $"برای نوع تعدیل «{entity.AdjustmentType.Title}» قبلاً یک عامل تعدیل پیش‌فرض ثبت شده است؛ از هر نوع تنها یک عامل تعدیل می‌تواند پیش‌فرض باشد.");
        }
    }
}