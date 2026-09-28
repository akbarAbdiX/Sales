using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Exceptions;
using BigBang.WebServer.Common.Services;

namespace BigBang.App.Sales.Service.Role;

[InternalService(ServiceType = typeof(ICheckAccess), InstanceMode = InstanceMode.Single)]
public class CheckAccess : ICheckAccess
{
    private readonly IBigBangIdentityService _identityService;
    private readonly IBusinessContext _businessContext;

    public CheckAccess(IBigBangIdentityService identityService, IBusinessContext businessContext)
    {
        _identityService = identityService;
        _businessContext = businessContext;
    }

    void ICheckAccess.CheckDocumentsAccess<T>(string businessContextName,
                                              string claimName,
                                              T expectedResult,
                                              string message = null)
    {
        if (_businessContext is null ||
            !_businessContext.HasKey(businessContextName))
        {
            return;
        }

        if (!_identityService.Identity.Claims.TryGetValue("Roles", out Claim? claim))
            return;

        var actualResult = (T)_businessContext[businessContextName];

        var hasClaim = claim.Value
                            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                            .Contains(claimName, StringComparer.OrdinalIgnoreCase);

        var isExpectedResult = EqualityComparer<T>.Default.Equals(actualResult, expectedResult);

        if (hasClaim && isExpectedResult)
            throw new BigBangException(string.IsNullOrEmpty(message) ? "دسترسی مجاز نیست." : message);
    }

}