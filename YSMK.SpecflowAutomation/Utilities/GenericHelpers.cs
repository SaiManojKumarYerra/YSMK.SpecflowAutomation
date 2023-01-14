using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace YSMK.SpecflowAutomation.Utilities
{
    public class GenericHelpers
    {

        static Random random = new Random();
        public static string GetExecutingAssemblyProjectPathOfFolder(string folderName)
        {
            string path = string.Empty;
            try
            {
                string strExeFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrEmpty(folderName)) { path = strExeFilePath; }
                else {  path = strExeFilePath + "\\" + folderName + @"\"; }
            }
            catch (Exception e)
            {
                throw new Exception("Exception in GetExecutingAssemblyProjectPathOfFolder Method : " + e);
            }
            return path;
        }

        public static string GetProjectPathOfFolder(string folderName)
        {
            string path = string.Empty;
            try
            {
                string projectDir = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName;
                path = projectDir + @"\" + folderName + @"\";
            }
            catch (Exception e)
            {
                throw new Exception("Exception in GetExecutingAssemblyProjectPathOfFolder Method : " + e);
            }
            return path;
        }

        public static int GenerateRandomNumber(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
        public static int GenerateRandomNumber(int maxValue)
        {
            return random.Next(maxValue);
        }
        public static string GenerateRandomNumberasString(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue).ToString();
        }
        public static string GenerateRandomNumberasString(int maxValue)
        {
            return random.Next(maxValue).ToString(); ;
        }
        public static string RandomDigits(int length)
        {
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        public static string GenerateRandomString(int length, bool isLowerCase = false)
        {
            var rString = "";
            for (var i = 0; i < length; i++)
            {
                rString += ((char)(random.Next(1, 26) + 64)).ToString();
            }
            if (isLowerCase)
                rString.ToLower();
            return rString;
        }
        public static double GetRandomNumber_Double(double minimum, double maximum, int decimals)
        {
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, decimals);
        }

        public static string GetRandomNumbers(int range)
        {
            string num = string.Empty;
            for (int index = 0; index < range; index++)
                num = String.Concat(num, random.Next(10).ToString());
            return num;

        }

        public static string GetCurrentDate(string timeZone)
        {
            try
            {
                DateTime dateTime, utcDate;
                string date;
                switch (timeZone.ToUpper())
                {
                    case "UTC":
                        dateTime = DateTime.UtcNow;
                        date = dateTime.ToString("MM/dd/yyyy");
                        break;

                    case "PACIFIC STANDARD TIME":
                        utcDate = DateTime.UtcNow;
                        date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcDate, timeZone).ToString("MM/dd/yyyy");
                        break;

                    case "IST":
                        dateTime = DateTime.Now;
                        date = dateTime.ToString("MM/dd/yyyy");
                        break;

                    default:
                        throw new Exception("None of the timezone matches the available cases hence failed");
                }

                return date;

            }
            catch (Exception e)
            {
                return null;
                throw new Exception("Exception in GetCurrentDate Method : " + e);
            }
        }

        public static string GetCurrentTime(string timeZone)
        {
            try
            {
                DateTime utcDate, pstDate;
                string[] dateString;
                string date;
                switch (timeZone.ToUpper())
                {
                    case "UTC":
                        utcDate = DateTime.UtcNow;
                        date = utcDate.ToString();
                        dateString = date.Split(' ');
                        return dateString[1] + dateString[2];

                    case "PACIFIC STANDARD TIME":
                        utcDate = DateTime.UtcNow;
                        pstDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcDate, timeZone);
                        dateString = pstDate.GetDateTimeFormats('G');
                        dateString = dateString[13].Split(' ');
                        return dateString[1] + " " + dateString[2];

                    case "IST":
                        utcDate = DateTime.Now.Date;
                        date = utcDate.ToString();
                        dateString = date.Split(' ');
                        return dateString[1] + dateString[2];

                    default:
                        throw new Exception("None of the timezone matches the available cases hence failed");
                        
                }

            }
            catch (Exception e)
            {
                return null;
                throw new Exception("Exception in GetCurrentTime Method : " + e);
            }
        }
        public static string GetMonthNameByInt(int number)
        {
            DateTime dateTime = new DateTime(2023, number, 1);
            return dateTime.ToString("MMMM");
        }

        public static string GetDateTimeWithoutSpace()
        {
            var TimeAndDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            return TimeAndDate;
        }
    }
}
