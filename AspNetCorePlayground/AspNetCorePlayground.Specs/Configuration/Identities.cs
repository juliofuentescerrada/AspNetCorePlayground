namespace AspNetCorePlayground.Specs.Configuration
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public static class Identities
    {
        public static string Default = nameof(Default);

        public static IEnumerable<Claim> Profile(string id)
        {
            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, id)
            };
        }
    }
}