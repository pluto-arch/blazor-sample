using BlazorSample.Domain.Infra;

namespace BlazorSample.Domain.Aggregates.Product;

public class DeviceTag : BaseEntity<int>
{
    public string Name { get; set; }
}
