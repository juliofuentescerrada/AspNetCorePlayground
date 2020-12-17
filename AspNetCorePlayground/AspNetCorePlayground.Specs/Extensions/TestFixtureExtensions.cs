﻿namespace AspNetCorePlayground.Specs.Extensions
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
        private static RequestBuilder CreateDefaultRequest<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return testFixture.Server.CreateHttpApiRequest(action).WithIdentity(identity);
        }

        public static async Task<TResult> Get<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.Get(action, identity).GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Get<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).GetAsync();
        }

        public static async Task<TResult> Post<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.Post(action, identity).GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Post<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).PostAsync();
        }

        public static async Task<TResult> Put<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.Put(action, identity).GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Put<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).SendAsync(HttpMethod.Put.ToString());
        }

        public static async Task<TResult> Delete<TController, TResult>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.Delete(action, identity).GetResponse<TResult>();
        }

        public static async Task<HttpResponseMessage> Delete<TController>(this TestFixture testFixture, Expression<Func<TController, object>> action, TestIdentity identity = default) where TController : ControllerBase
        {
            return await testFixture.CreateDefaultRequest(action, identity).SendAsync(HttpMethod.Delete.ToString());
        }
    }
}