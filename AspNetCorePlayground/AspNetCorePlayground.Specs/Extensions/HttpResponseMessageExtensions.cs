namespace AspNetCorePlayground.Specs.Extensions
{
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> GetResponse<T>(this Task<HttpResponseMessage> task)
        {
            var response = await task;

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }

            return default;
        }
    }
}