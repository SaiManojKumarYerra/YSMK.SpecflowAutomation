using Aspose.Pdf;
using Aspose.Pdf.Text;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.IO;
using System.Text;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfReader = iText.Kernel.Pdf.PdfReader;


namespace YSMK.SpecflowAutomation.Utilities
{
    public class PDFHelpers
    {
        /// <summary>
        /// Converts PDF format file to Text File
        /// </summary>
        /// <param name="filePath">PDF Format file path</param>
        /// <returns>Text File Path</returns>
        public static string ConvertPDFtoTxtFile(string filePath)
        {
            try
            {
                PdfReader reader = new PdfReader(filePath);
                PdfDocument pdfDoc = new PdfDocument(reader);

                LocationTextExtractionStrategy locationTextExtractionStrategy = new LocationTextExtractionStrategy();
                string zz = PdfTextExtractor.GetTextFromPage(pdfDoc.GetFirstPage(), locationTextExtractionStrategy);

                string TxtFile = filePath[..^3] + "txt";
                File.WriteAllText(TxtFile, zz);

                pdfDoc.Close();
                reader.Close();

                return TxtFile;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in ConvertPDFtoTxtFile Method() : " + e);
                return null;
            }
        }
        /// <summary>
        /// Extracts the tables into Text File with delimeter '|'
        /// </summary>
        /// <param name="pdfFilePath"></param>
        /// <returns>Text File containing Table Data</returns>
        public static string Extract_TableFromPDF_IntoTextFile(string pdfFilePath)
        {
            string line = string.Empty;
            string TxtFile = pdfFilePath[..^4] + "_TableData.txt";
            StreamWriter writer = new StreamWriter(TxtFile);
            Document pdfDocument = new Document(pdfFilePath);
            foreach (var page in pdfDocument.Pages)
            {
                TableAbsorber absorber = new TableAbsorber();
                absorber.Visit(page);

                foreach (AbsorbedTable table in absorber.TableList)
                {
                    foreach (AbsorbedRow row in table.RowList)
                    {
                        foreach (AbsorbedCell cell in row.CellList)
                        {
                            foreach (TextFragment fragment in cell.TextFragments)
                            {
                                var sb = new StringBuilder();
                                foreach (TextSegment seg in fragment.Segments)
                                    sb.Append(seg.Text);
                                line += $"{sb.ToString()}|";
                            }
                        }
                        writer.WriteLine(line.Replace("| |", " "));
                        line = string.Empty;
                    }
                }
            }
            writer.Flush();
            writer.Close();
            return TxtFile;
        }
    }
}
