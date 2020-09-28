namespace Gillingoz.Chronos.Service
{
    public class EnvironmentSettings
    {
        public string Id { get; set; }
        public string ApiUrl { get; set; }
        public string ApiSubscriptionKeyValue { get; set; }
        public string ApiSubscriptionKeyName { get; set; }
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string LegalEntityId { get; set; }
        public string Tenant { get; set; }
        public bool IsEnabled { get; set; }
    }
}
