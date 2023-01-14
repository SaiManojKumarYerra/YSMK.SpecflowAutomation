using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace YSMK.SpecflowAutomation.Utilities
{
    public class RestClientHelpers
    {
        static RestClient _client;
        static RestRequest _request;
        /// <summary>
        /// Create Rest Request. 
        /// </summary>
        /// <param name="restClientRequest"></param>
        public static void CreateRestRequest(RestClientRequest restClientRequest)
        {
            //***************Create Rest Client******************//
            CreateRestClient(restClientRequest.BaseUrl, restClientRequest.EndPoint);

            _request = new RestRequest();

            //***************Add Rest Request Method******************//
            switch (restClientRequest.Method)
            {
                case Method.Get:
                    _request = new RestRequest("", Method.Get);
                    break;
                case Method.Post:
                    _request = new RestRequest("", Method.Post);
                    break;
                case Method.Put:
                    _request = new RestRequest("", Method.Put);
                    break;
                case Method.Delete:
                    _request = new RestRequest("", Method.Delete);
                    break;
            }

            //***************Add Rest Request Content type******************//
            switch (restClientRequest.ContentType)
            {
                case ContentType.JSON:
                    _request.AddHeader("Content-Type", "application/json");
                    break;
                case ContentType.XML:
                    _request.AddHeader("Content-Type", "application/xml");
                    break;
            }

            //***************Add headers for Rest Request******************//
            foreach (var item in restClientRequest.HeaderNameandValue)
            {
                _request.AddHeader(item.Key, item.Value);
            }

            //***************Add params for Rest Request******************//
            foreach (var item in restClientRequest.ParameterNameandValue)
            {
                _request.AddParameter(item.Key, item.Value);
            }



            //***************Add Rest Request Data format and body based on the format******************//
            switch (restClientRequest.DataFormat)
            {
                case DataFormat.Json:
                    _request.RequestFormat = DataFormat.Json;
                    _request.AddJsonBody(restClientRequest.RequestBody);
                    break;
                case DataFormat.Xml:
                    _request.RequestFormat = DataFormat.Xml;
                    _request.AddXmlBody(restClientRequest.RequestBody);
                    break;
                case DataFormat.None:
                    _request.RequestFormat = DataFormat.None;
                    break;
                default:
                    break;
            }
            //return _request;
        }
        /// <summary>
        /// Creates Rest Request using the values passed from RestClientRequest. Then executes the request asynchronously.
        /// </summary>
        /// <returns>Rest Response</returns>
        public static RestResponse ExecuteRestRequest(RestClientRequest restClientRequest)
        {
            try
            {
                CreateRestRequest(restClientRequest);
                Task<RestResponse> actual = _client.ExecuteAsync(_request);
                actual.Wait();
                return actual.Result;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Rest Request should be created before executing this function. Executes the rest request asynchronously.
        /// </summary>
        /// <returns>RestClientResult with Json Deserialized content as body</returns>
        public static RestClientResult Perform()
        {
            var result = new RestClientResult();
            try
            {
                Task<RestResponse> response = _client.ExecuteAsync(_request);
                response.Wait();
                result.StatusCode = (int)response.Result.StatusCode;
                result.ErrorMessage = response.Result.ErrorMessage;
                result.RestResponse = response.Result;
                result.Body = response.Result.Content;//JsonConvert.DeserializeObject(response.Result.Content);
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
            }
            finally
            {
                _request = null;
            }
            return result;
        }

        /// <summary>
        /// Rest Request should be created before executing this function. Executes the rest request asynchronously.
        /// </summary>
        /// <typeparam name="T">Target deserialization type</typeparam>
        /// <returns>RestClientResult with Data as body</returns>
        public static RestClientResult<T> Perform<T>() where T : class, new()
        {
            var result = new RestClientResult<T>();

            try
            {
                var response = _client.ExecuteAsync<T>(_request);
                response.Wait();
                result.StatusCode = (int)response.Result.StatusCode;
                result.ErrorMessage = response.Result.ErrorMessage;
                result.RestResponse = response.Result;
                result.Body = response.Result.Data;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
            }
            finally
            {
                _request = null;
            }
            return result;
        }


        #region Private functions
        private static RestClient CreateRestClient(string baseUrl, string endPoint)
        {
            var url = Path.Combine(baseUrl, endPoint);
            _client = new RestClient(url);

            return _client;
        }
        private static RestRequest CreateGetRequest(string baseUrl, string endPoint,
            Dictionary<string, string> headerNameandValue, Dictionary<string, string> parameterNameandValue)
        {
            CreateRestClient(baseUrl, endPoint);
            _request = new RestRequest("", Method.Get);
            foreach (var item in headerNameandValue)
            {
                _request.AddHeader(item.Key, item.Value);
            }

            foreach (var item in parameterNameandValue)
            {
                _request.AddParameter(item.Key, item.Value);
            }
            return _request;
        }
        private static RestRequest CreateJSONPostRequest(string baseUrl, string endPoint, string jsonRequest, Dictionary<string, string> headerNameandValue)
        {
            CreateRestClient(baseUrl, endPoint);
            _request = new RestRequest("", Method.Post);
            foreach (var item in headerNameandValue)
            {
                _request.AddHeader(item.Key, item.Value);
            }
            _request.RequestFormat = DataFormat.Json;
            _request.AddJsonBody(jsonRequest);
            return _request;
        }
        private static RestRequest CreateXMLPostRequest(string baseUrl, string endPoint, string payload, Dictionary<string, string> headerNameandValue)
        {
            CreateRestClient(baseUrl, endPoint);
            _request = new RestRequest("", Method.Post);
            foreach (var item in headerNameandValue)
            {
                _request.AddHeader(item.Key, item.Value);
            }
            _request.RequestFormat = DataFormat.Xml;
            _request.AddXmlBody(payload);
            return _request;
        }
        #endregion

    }

    public class RestClientRequest
    {
        public string BaseUrl { get; set; } = "";
        public string EndPoint { get; set; } = "";
        public Dictionary<string, string> HeaderNameandValue { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParameterNameandValue { get; set; } = new Dictionary<string, string>();
        public ContentType ContentType { get; set; }
        public Method Method { get; set; }
        public DataFormat DataFormat { get; set; }
        public object RequestBody { get; set; } = "";
    }
    public class RestClientResult
    {
        public int StatusCode { get; set; }

        public dynamic Body { get; set; } = null;

        public string ErrorMessage { get; set; } = string.Empty;

        public RestResponse RestResponse { get; set; }
    }

    public class RestClientResult<T>
    {
        public int StatusCode { get; set; }

        public T Body { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public RestResponse<T> RestResponse { get; set; }
    }

    public enum ContentType
    {
        JSON,
        XML
    }
}
