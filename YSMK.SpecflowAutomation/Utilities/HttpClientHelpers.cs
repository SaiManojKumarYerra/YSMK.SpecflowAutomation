using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace YSMK.SpecflowAutomation.Utilities
{
    public class HttpClientHelpers
    {

        static HttpClient _httpClient;
        /// <summary>
        /// Create HTTP POST Request with content type as application/json
        /// </summary>
        /// <param name="baseURL">API base url</param>
        /// <param name="endPoint">API end point</param>
        /// <param name="requestString">Request body</param>
        /// <param name="serializeJSON">If Request string needs to be Serialized pass true</param>
        /// <returns>Content as string</returns>
        public static async Task<string> CreateJSONPostHttpRequest(string baseURL, string endPoint, string requestString, bool serializeJSON)
        {
            //HttpClient client = null;
            string stringData;
            try
            {
                string json;
                if (serializeJSON)
                    json = JSONHelpers.SerializetoJSONString(requestString);
                else
                    json = requestString;
                var data = new StringContent(json, Encoding.Default, "application/json");

                using (_httpClient = new HttpClient())
                {
                    SetHttpClient(baseURL);
                    Task<HttpResponseMessage> response = _httpClient.PostAsync(endPoint, data);
                    response.Wait();

                    if (!response.Result.IsSuccessStatusCode)
                    {
                        throw new Exception(response.Result.ReasonPhrase);
                    }

                    stringData = await response.Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                }
            }

            return stringData;
        }

        private static void SetHttpClient(string connection)
        {
            _httpClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _httpClient.BaseAddress = new Uri(connection);
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        //public static HttpClientResult PerformGetAsync()
        //{
        //    var result = new HttpClientResult();
        //    try
        //    {
        //        Task<HttpResponseMessage> response = _httpClient.GetAsync(_httpClientRequest.EndPoint);
        //        response.Wait();

        //        result.StatusCode = (int)response.Result.StatusCode;
        //        result.ErrorMessage = response.Result.ReasonPhrase;
        //        result.ResponseHeaders = response.Result.Headers;
        //        result.ResponseMessage = response.Result;
        //        HttpContent httpContent = response.Result.Content;
        //        Task<string> responseString = httpContent.ReadAsStringAsync();
        //        responseString.Wait();

        //        result.ResponseContent = responseString.Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelpers.Write(ex.Message, ex.InnerException);
        //    }
        //    finally
        //    {
        //        if (_httpClient != null)
        //        {
        //            _httpClient.Dispose();
        //        }
        //    }
        //    return result;
        //}

        public static HttpClientResult Perform(HttpRequestMessage request)
        {
            var result = new HttpClientResult();
            try
            {
                using (_httpClient = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    Task<HttpResponseMessage> response = _httpClient.SendAsync(request);
                    response.Wait();

                    using (HttpResponseMessage responseResult = response.Result)
                    {
                        result.StatusCode = (int)responseResult.StatusCode;
                        result.ErrorMessage = responseResult.ReasonPhrase;
                        result.ResponseHeaders = responseResult.Headers;
                        result.ResponseMessage = responseResult;
                        HttpContent httpContent = responseResult.Content;
                        Task<string> responseString = httpContent.ReadAsStringAsync();
                        responseString.Wait();

                        result.ResponseContent = responseString.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
            }
            finally
            {
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                }
            }
            return result;
        }

    }

    public class HttpClientResult
    {
        public int StatusCode { get; set; }
        public dynamic Body { get; set; } = null;
        public string ErrorMessage { get; set; } = string.Empty;
        public HttpResponseMessage ResponseMessage { get; set; }
        public string ResponseContent { get; set; }
        public HttpResponseHeaders ResponseHeaders { get; set; }

    }
}
