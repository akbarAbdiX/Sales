using System;
using BigBang.App.RoleBaseAuthorization;
using BigBang.Metadata.Models;
using BigBang.Metadata.Models.Datagram;
using BigBang.Metadata.Models.EntityModel;
using BigBang.WebServer.Common.Attributes;

namespace BigBang.App.Sales.RowLevelAccess
{
    [InternalService(InstanceMode = InstanceMode.Scoped, ServiceType = typeof(IRowLevelAccessProvider))]
    internal class SalesAdminRowLevelAccessProvider : IRowLevelAccessProvider
    {
        public SalesAdminRowLevelAccessProvider() { }

        public FilterGroup GetRowLevelAccess(IObjectId objectId, IBigBangActorIdentity actor, object context, IEntityModelInfo model)
        {
            return FilterGroupBuilder.Build();
        }
    }
}
