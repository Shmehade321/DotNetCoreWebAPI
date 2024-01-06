namespace DotNetCoreWebAPI.Authority
{
    public static class AppRepository
    {
        private static readonly List<Application> _applications = new List<Application>
        {
            new Application
            {
                ApplicationId = 1,
                ApplicationName = "MVCWebApp",
                ClientId = "A8D21EE0-73B8-4B8A-8DA0-C0F6410B4ECB",
                Secret = "CAE7E8BE-95D6-429B-9C81-3780B086A276",
                Scopes = "read,write"
            }
        };

        public static bool Authenticate(string clientId, string secret)
        {
            return _applications.Any(a => a.ClientId == clientId && a.Secret == secret);
        }

        public static Application? GetApplicationByClientId(string clientId)
        {
            return _applications.FirstOrDefault(a => a.ClientId == clientId);
        }
    }
}
