using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IsUserExistsAttribute: ServiceFilterAttribute
{
    public IsUserExistsAttribute() : base(typeof(UserExistsFilter)) { }
}