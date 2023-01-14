using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YSMK.SpecflowAutomation.Utilities
{
    public static class ExcelHelpers
    {
        public static string claimNumberfromExcel = "";
        public static DataTable ReadData(string filename, string tabName = "Sheet1", bool hasHeader = true)
        {
            filename = filename.ToApplicationPath();

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(filename))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets[tabName];
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}");
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }

        public static string ToApplicationPath(this string fileName)
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                                .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return Path.Combine(appRoot, fileName);
        }

        public static string getDownloadedFileName()
        {
            var directory = new DirectoryInfo(GenericHelpers.GetExecutingAssemblyProjectPathOfFolder("Downloads"));
            return directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().ToString();
        }


        public static int RowCount(string fileName, string sheetName)
        {
            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = File.OpenRead(GenericHelpers.GetExecutingAssemblyProjectPathOfFolder("TestData") + "\\FileUpload\\" + fileName))
                    {
                        pck.Load(stream);
                    }
                    var ws = pck.Workbook.Worksheets[sheetName];
                    var row = ws.Dimension.End.Row - 1;
                    while (row >= 1)
                    {
                        var range = ws.Cells[row, 1, row, ws.Dimension.End.Column];
                        if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                        {
                            break;
                        }
                        row--;
                    }
                    return row;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The Exception is " + e);
                return 0;
            }
        }

        public static void updateTestDataExcel(string fileName, string memberNumber, string firstName, string lastName, string DOB, string icdCode, bool randomClaimandPrescription = true, string claimNumberValue = "", string prescriptionNumberValue = "")
        {
            FileInfo file = new FileInfo(GenericHelpers.GetExecutingAssemblyProjectPathOfFolder("TestData") + "FileUpload" + "\\" + fileName);
            int rowCount = RowCount(fileName, "Sheet1");
            string prescriptionNumber, claimNumber;
            if (randomClaimandPrescription)
            {
                prescriptionNumber = generateRandomNumber();
                claimNumber = generateRandomNumber();
            }
            else
            {
                prescriptionNumber = prescriptionNumberValue;
                claimNumber = claimNumberValue;
            }
            claimNumberfromExcel = claimNumber;
            for (int j = 2; j <= rowCount + 1; j++)
            {
                if (file.Extension.Contains("xlsx"))
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(file))
                    {
                        ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                        ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                        if (fileName.Contains("MultipleClinicalSection"))
                        {
                            for (int i = 2; i < 4; i++)
                            {
                                excelWorksheet.Cells[i, 1].Value = memberNumber;
                                excelWorksheet.Cells[i, 2].Value = firstName;
                                excelWorksheet.Cells[i, 4].Value = lastName;
                                excelWorksheet.Cells[i, 5].Value = DOB;
                                excelWorksheet.Cells[i, 31].Value = prescriptionNumber;
                                excelWorksheet.Cells[i, 46].Value = claimNumber;
                                excelWorksheet.Cells[i, 51].Value = icdCode;
                                Console.WriteLine("Claim Number :" + claimNumber);

                            }
                        }
                        else if (!randomClaimandPrescription)
                        {
                            excelWorksheet.Cells[j, 1].Value = memberNumber;
                            excelWorksheet.Cells[j, 2].Value = firstName;
                            excelWorksheet.Cells[j, 4].Value = lastName;
                            excelWorksheet.Cells[j, 5].Value = DOB;
                            excelWorksheet.Cells[j, 31].Value = prescriptionNumber;
                            excelWorksheet.Cells[j, 46].Value = claimNumberfromExcel;
                            excelWorksheet.Cells[j, 51].Value = icdCode;
                            Console.WriteLine("Claim Number :" + claimNumberfromExcel);
                        }
                        else
                        {
                            excelWorksheet.Cells[j, 1].Value = memberNumber;
                            excelWorksheet.Cells[j, 2].Value = firstName;
                            excelWorksheet.Cells[j, 4].Value = lastName;
                            excelWorksheet.Cells[j, 5].Value = DOB;
                            excelWorksheet.Cells[j, 31].Value = generateRandomNumber();
                            claimNumberfromExcel = generateRandomNumber();
                            excelWorksheet.Cells[j, 46].Value = claimNumberfromExcel;
                            excelWorksheet.Cells[j, 51].Value = icdCode;
                            Console.WriteLine("Claim Number :" + claimNumberfromExcel);
                        }
                        excelPackage.Save();
                        Console.WriteLine("Total Rows updated is " + rowCount);
                    }
                }
                else
                {
                    Console.WriteLine("No Need of any updates for " + file.Name + " beacsue file format is " + file.Extension);
                }
            }
        }

        public static string generateRandomNumber()
        {
            Random random = new Random();
            return random.Next(0, 1000000000).ToString("D10");
        }

        public static void updateTestDataExcel(string fileName, Dictionary<string, string> keyValuePairs)
        {
            FileInfo file = new FileInfo(GenericHelpers.GetExecutingAssemblyProjectPathOfFolder("TestData") + "FileUpload" + "\\" + fileName);
            int rowCount = RowCount(fileName, "Sheet1");

            for (int j = 2; j <= rowCount + 1; j++)
            {
                if (file.Extension.Contains("xlsx"))
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(file))
                    {
                        ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                        ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                        int column = excelWorksheet.Columns.Count();
                        foreach (var item in keyValuePairs)
                        {
                            for (int i = 1; i < column; i++)
                            {
                                if (excelWorksheet.Cells[1, i].Value.Equals(item.Key.ToString()))
                                {
                                    excelWorksheet.Cells[j, i].Value = item.Value.ToString();
                                    break;
                                }

                            }
                            excelPackage.Save();
                            Console.WriteLine("Total Rows updated is " + rowCount);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No Need of any updates for " + file.Name + " beacsue file format is " + file.Extension);

                }
            }
        }
    }
}
