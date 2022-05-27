namespace Cosmos.Models
{
    public class Authentication
    {
        public string Authority { set; get; }
        public string ClientId { set; get; }
        public string ClientSecret { set; get; }
        public string Scope { set; get; }
        public List<string> Scopes
        {
            get
            {
                List<string> scopes = new List<string>();
                if (Scope != null)
                {
                    scopes = Scope.Split(" ")
                        .ToList();
                }
                return scopes;
            }
        }
        public string? CallbackPath { set; get; }
        public bool SaveTokens { set; get; }
        public bool GetClaimsFromUserInfoEndpoint { set; get; }
        public bool RequireHttpsMetadata { set; get; }
        public int ExpireTimeSpan { set; get; }
        public string RedirectUri { set; get; }
        public bool SlidingExpiration { set; get; }
    }
}
