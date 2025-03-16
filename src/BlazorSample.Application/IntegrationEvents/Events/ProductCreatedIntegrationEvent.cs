using BlazorSample.Domain.Aggregates.EventLogs;

namespace BlazorSample.Application.IntegrationEvents.Events
{
    public class ProductCreatedIntegrationEvent : IntegrationEvent
    {
        public ProductCreatedIntegrationEvent(string productId) : base()
        {
            ProductId = productId;
        }

        public string ProductId { get; set; }
    }
}