using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YSMK.SpecflowAutomation.Utilities
{
    public class TextFileHelpers
    {
        private static string ReadLineByLineText(String textFilePath, int lineNumber)
        {
            try
            {
                int counter = 0;
                string line;

                // Read the file and display it line by line.  
                StreamReader file =
                    new StreamReader(textFilePath);
                while ((line = file.ReadLine()) != null)
                {
                    counter++;
                    if (counter == lineNumber)
                    {
                        break;
                    }
                }

                file.Close();
                return line;
            }
            catch (Exception e)
            {
                Assert.Fail("" + e);
                return "";
            }
        }

        public static int GetCountOfRecordsInFile(String textFilePath)
        {
            int counter = 0;

            try
            {

                // Read the file and display it line by line.  
                StreamReader file = new StreamReader(textFilePath);
                while ((file.ReadLine()) != null)
                {
                    counter++;

                }

                file.Close();
                return counter;
            }
            catch (Exception e)
            {
                Assert.Fail("" + e);
                return counter;
            }
        }


        public static String ReadDataFromTextFile(String textFilePath, int lineNumber, int columnNumber)
        {
            try
            {
                String fullLine = ReadLineByLineText(textFilePath, lineNumber);

                String[] stringArray = fullLine.Split(new[] { " &^ " }, StringSplitOptions.RemoveEmptyEntries);
                string a = stringArray[columnNumber - 1];
                return stringArray[columnNumber - 1];
            }
            catch (Exception e)
            {
                Assert.Fail("" + e);
                return "";
            }
        }

        public static IList<String> ReadDataFromTextFileAddtoList(String textFilePath, int lineNumber, int fromcolumnnumber, int toColumnnumber)
        {
            try
            {
                String fullLine = ReadLineByLineText(textFilePath, lineNumber);

                String[] stringArray = fullLine.Split(new[] { " &^ " }, StringSplitOptions.RemoveEmptyEntries);
                //string a = stringArray[columnNumber - 1];

                List<string> list = new List<String>();

                for (int index = fromcolumnnumber - 1; index < toColumnnumber; index++)
                {
                    String val = stringArray[index];
                    list.Add(stringArray[index]);
                }
                return list;
            }
            catch (Exception e)
            {
                Assert.Fail("" + e);
                return null;
            }
        }

        public static void WriteDataFromTextFile(String textFilePath, int lineNumber, int columnNumber, string replacingText, string textToReplace)
        {
            try
            {
                string newText = null;
                StreamReader sr = new StreamReader(textFilePath);
                string text = sr.ReadToEnd();
                if (text.Contains(textToReplace))
                {
                    newText = text.Replace("&^ " + textToReplace + " &^", "&^ " + replacingText + " &^");
                }
                sr.Close();

                StreamWriter sw = new StreamWriter(textFilePath);
                sw.WriteLine(newText);
                sw.Close();
            }
            catch (Exception e)
            {
                Assert.Fail("" + e);

            }
        }

        public static List<string> ReadAllDataFromTextFile(String textFilePath)
        {
            try
            {
                //Log.LogInfo("Entered Method:ReadAllDataFromTextFile ");

                //string[] list; 
                List<string> list = new List<String>();
                int totalLines = File.ReadAllLines(textFilePath).Length;
                if (totalLines == 0)
                {
                    Assert.Fail("No data present in the data file.");
                }

                else if (totalLines == 1)
                {
                    Assert.Fail("Only header is present in the file, no data");
                }
                else
                {

                    for (int lineNumber = 1; lineNumber <= totalLines; lineNumber++)
                    {
                        if (lineNumber == 1)
                        {
                            continue;
                        }
                        else
                        {
                            String fullLine = ReadLineByLineText(textFilePath, lineNumber);
                            list.Add(fullLine);
                        }
                    }

                }
                //Log.LogInfo("Exit Method:ReadAllDataFromTextFile ");

                return list;
            }
            catch (Exception e)
            {
                Assert.Fail("Unable to read data from text file :" + e);
                //Log.LogInfo("Unable to read data from text file :" + e);
                return null;
            }
        }
    }
}
