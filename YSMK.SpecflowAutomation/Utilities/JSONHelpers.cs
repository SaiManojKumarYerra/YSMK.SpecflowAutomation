using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.IO;
using System.Xml;

namespace YSMK.SpecflowAutomation.Utilities
{
    public class JSONHelpers
    {
        public static XmlDocument ConvertJSONToXML(string jsonString)
        {
            try
            {
                XmlDocument doc = JsonConvert.DeserializeXmlNode(jsonString);

                return doc;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static T ConvertJSONToObject<T>(string jsonText)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonText);
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static string ConvertObjectToJson<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static T GetContent<T>(RestResponse restResponse)
        {
            try
            {
                var _content = restResponse.Content;
                return JsonConvert.DeserializeObject<T>(_content);
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static T ReadJSONfromFileandConvertJSONToObject<T>(string file)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(file));
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static string SerializetoJSONString(dynamic payload)
        {
            try
            {
                return JsonConvert.SerializeObject(payload);
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static JObject ConvertDynamicObjecttoJSONObject(dynamic dynamicTable)
        {
            try
            {
                JObject jobject = new JObject(dynamicTable);

                return jobject;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static JValue ConvertDynamicObjecttoJSONValue(dynamic dynamicTable)
        {
            try
            {
                JValue jobject = new JValue(dynamicTable);

                return jobject;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static object DeSerializetoJSONObject(string payload)
        {
            try
            {
                return JsonConvert.DeserializeObject(payload);
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static JObject ConvertStringtoJSONObject(string jsonString)
        {
            try
            {
                dynamic data = JObject.Parse(jsonString);

                return data;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                //throw new Exception(ex.Message);
            }
            return null;
        }

        public static JArray ConvertStringtoJSONArray(string jsonString)
        {
            try
            {
                dynamic data = JArray.Parse(jsonString);

                return data;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                //throw new Exception(ex.Message);
            }
            return null;
        }

        public static void ReplaceTokenValue_EmptyStringORNull(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.String:
                    token.Replace("");
                    break;
                case JTokenType.Array:
                case JTokenType.Object:
                    Assert.Inconclusive("Field type is " + token.Type + ". Field value cannot be empty as some of its children are mandatory");
                    break;
                default:
                    token.Replace(null);
                    break;
            }
        }
    }
}
