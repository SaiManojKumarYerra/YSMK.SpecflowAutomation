using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using YSMK.SpecflowAutomation.Utilities;

namespace YSMK.SpecflowAutomation.Utilities
{
    public class XMLHelpers
    {
        public static ObjectType ReadXml<ObjectType>(string fileName)
        {
            try
            {
                using (var sw = new StreamReader(fileName))
                {
                    return (ObjectType)new XmlSerializer(typeof(ObjectType)).Deserialize(sw);
                }
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static void SaveXml<ObjectType>(ObjectType o, string fileName)
        {
            try
            {
                using (var sw = new StreamWriter(fileName))
                {
                    new XmlSerializer(typeof(ObjectType)).Serialize(sw, o);
                }
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }
        public static ObjectType ReadXmlString<ObjectType>(string xmlString)
        {
            try
            {
                using (var sw = new StringReader(xmlString))
                {
                    return (ObjectType)new XmlSerializer(typeof(ObjectType)).Deserialize(sw);
                }
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static string SaveXmlString<ObjectType>(ObjectType o)
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    new XmlSerializer(typeof(ObjectType)).Serialize(sw, o);
                    return sw.ToString();
                }

            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static XmlNodeList ReadXMLandFetchAllXMLNodesbyXpath(string fileName, string xPath)
        {
            try
            {
                //Load the XML file in XmlDocument.
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);//Example @"D:\Xml\EmployeeData.xml"

                //Fetch all the Nodes.
                XmlNodeList nodeList = doc.SelectNodes(xPath); // Example: //text()

                return nodeList;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static XmlNodeList FetchAllXMLNodesbyXpath(string xmlString, string xPath)
        {
            try
            {
                //Load the XML file in XmlDocument.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                //Fetch all the Nodes.
                XmlNodeList nodeList = doc.SelectNodes(xPath); // Example: //text()

                return nodeList;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static string ReadXMLandFetchSingleNodeXMLValuebyXpath(string fileName, string xPath)
        {
            try
            {
                //Load the XML file in XmlDocument.
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);//Example @"D:\Xml\EmployeeData.xml"

                //Fetch single XML Node.
                XmlNode titleNode = doc.SelectSingleNode(xPath); // Example: //Employee/FirstName

                if (titleNode != null)
                    return titleNode.InnerText;
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static string FetchSingleNodeXMLValuebyXpath(string xmlString, string xPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                //Fetch single XML Node.
                XmlNode titleNode = doc.SelectSingleNode(xPath); // Example: //Employee/FirstName

                if (titleNode != null)
                    return titleNode.InnerText;
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static string ConvertXMLToJSON(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                string jsonBody = JsonConvert.SerializeXmlNode(doc);
                return jsonBody;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

        public static string ReadXMLandReplaceXMLNodeValue(string fileName, Dictionary<string, string> XpathandValuetobeReplaced)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XPathNavigator xPathNavigator = doc.CreateNavigator();
                foreach (var item in XpathandValuetobeReplaced)
                {
                    XPathNavigator node = xPathNavigator.SelectSingleNode(item.Key);
                    if (node != null)
                        node.InnerXml = item.Value;
                }

                return doc.OuterXml;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                throw new Exception(ex.Message);
            }
        }

    }
}
