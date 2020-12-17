namespace AspNetCorePlayground.Specs.Configuration
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public sealed class TestIdentity : List<Claim>
    {
        public static readonly TestIdentity Default = new(nameof(Default));
        
        public TestIdentity(string id)
        {
            Add(new Claim(ClaimTypes.NameIdentifier, id));
            Add(new Claim(ClaimTypes.Name, id));
        }
    }
}