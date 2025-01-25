using API.Filters;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace API.Attributes;

public class ValidatePermissionAttribute: TypeFilterAttribute
{
    public ValidatePermissionAttribute(RequiredAccessLevel requiredAccessLevel) 
        : base(typeof(ValidatePermissionFilter))
    {
        Arguments = new object[] { requiredAccessLevel };
    }
}