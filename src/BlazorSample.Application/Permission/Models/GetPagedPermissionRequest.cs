using BlazorSample.Application.Models.Generics;

namespace BlazorSample.Application.Permission.Models;

public class GetPagedPermissionRequest : PageRequest
{
    public string PermissionName { get; set; }
}