namespace AspNetCorePlayground.Specs.Extensions
{
    using Configuration;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.TestHost;
    using System;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class TestFixtureExtensions
    {
        private static RequestBuilder CreateDefaultRequest<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return testFixture.Server.CreateHttpApiRequest(action).WithIdentity(Identities.Profile(identity));
        }

        public static async Task<TResult> Get<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).GetAsync().GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Get<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).GetAsync();
        }

        public static async Task<TResult> Post<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).PostAsync().GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Post<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).PostAsync();
        }

        public static async Task<TResult> Put<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).SendAsync(HttpMethod.Put.ToString()).GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Put<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).SendAsync(HttpMethod.Put.ToString());
        }

        public static async Task<TResult> Delete<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).SendAsync(HttpMethod.Delete.ToString()).GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Delete<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, string identity) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).SendAsync(HttpMethod.Delete.ToString());
        }
    }
}