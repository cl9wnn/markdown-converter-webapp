using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IsDocumentExistsAttribute: ServiceFilterAttribute
{ 
    public IsDocumentExistsAttribute() : base(typeof(DocumentExistsFilter)) { }
}