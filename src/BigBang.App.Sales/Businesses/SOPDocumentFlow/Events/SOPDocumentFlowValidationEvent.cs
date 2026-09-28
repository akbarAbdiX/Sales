using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPDocumentFlow.Events
{
    [EventSubscription(typeof(IValidationEvent<SALSOPDocumentFlow>))]
    public class SOPDocumentFlowValidationEvent : IValidationEvent<SALSOPDocumentFlow>
    {
        private readonly ISession _session;

        public SOPDocumentFlowValidationEvent(ISession session)
        {
            _session = session;
        }

        #region Implementation of IValidationEvent<in SALSOPDocumentFlow>

        public void OnValidate(IValidationContext<SALSOPDocumentFlow> context)
        {
            var entity = context.Entity;
            if (entity == null) return;

            if (context.ValidationAction == ValidationAction.Delete)
                return;

            if (entity.DocumentType == null || entity.DocumentType.ID <= 0)
            {
                throw new BigBangException("انتخاب نوع سند الزامی است.");

            }

            if (entity.Level <= 0)
            {
                throw new BigBangException("مقدار سطح (Level) باید عددی مثبت و بزرگتر از صفر باشد.");

            }

            var isDocTypeDuplicate = _session.Query<SALSOPDocumentFlow>()
                .Any(x => x.ID != entity.ID && x.DocumentType.ID == entity.DocumentType.ID);

            if (isDocTypeDuplicate)
            {
                throw new BigBangException($"نوع سند «{entity.DocumentType.Title ?? entity.DocumentType.ID.ToString()}» قبلاً در فرایند دیگری تعریف شده و نمی‌تواند تکراری باشد.");

            }

            var isLevelDuplicate = _session.Query<SALSOPDocumentFlow>()
                .Any(x => x.ID != entity.ID && x.Level == entity.Level);

            if (isLevelDuplicate)
            {
                throw new BigBangException($"سطح شماره {entity.Level} قبلاً تعریف شده است و نمی‌تواند تکراری باشد.");

            }

            ValidateConsecutiveLevels(context, entity);
        }

        #endregion


        private void ValidateConsecutiveLevels(IValidationContext<SALSOPDocumentFlow> context, SALSOPDocumentFlow entity)
        {
            var allLevels = _session.Query<SALSOPDocumentFlow>()
                .Where(x => x.ID != entity.ID)
                .Select(x => x.Level)
                .ToList();

            allLevels.Add(entity.Level);

            var sortedLevels = allLevels.OrderBy(x => x).ToList();

            if (sortedLevels.First() != 1)
            {
                throw new BigBangException("سطوح فرایند باید حتماً از عدد ۱ شروع شوند.");

            }

            for (int i = 0; i < sortedLevels.Count; i++)
            {
                int expectedLevel = i + 1;
                if (sortedLevels[i] != expectedLevel)
                {
                    throw new BigBangException($"ترتیب سطوح رعایت نشده است. سطح مورد انتظار: {expectedLevel}، اما سطح ثبت شده: {sortedLevels[i]} می‌باشد. سطوح باید بدون پرش و به ترتیب ثبت شوند.");

                }
            }
        }
    }
}
