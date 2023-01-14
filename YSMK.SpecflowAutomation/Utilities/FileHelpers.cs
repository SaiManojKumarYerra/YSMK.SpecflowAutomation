using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YSMK.SpecflowAutomation.Utilities
{
    /// <summary>
    /// Contains methods to create/delete folders under the user's Temp\Automation folder
    /// and directory copy operation
    /// </summary>
    public class FilesHelper
    {
        /// <summary>
        /// Automation temporary directory under the user's temp directory
        /// that's used only for automation related file operations.
        /// </summary>
        public static string AutomationTempRoot => Path.GetTempPath() + "Automation";

        ///// <summary>
        ///// Copies directory and subdirectories using xcopy command.
        ///// Does not support copying root directory (e.g. c:\)
        ///// </summary>
        ///// <param name="sourceDirectory">Source directory to copy</param>
        ///// <param name="targetDirectory">Target directory to copy to</param>
        ///// <param name="timeoutMsec">Max time to wait for copy to complete</param>
        //public static void CopyDirectory(string sourceDirectory, string targetDirectory, int timeoutMsec = 60000)
        //{
        //    if (sourceDirectory.Length <= 3)
        //    {
        //        throw new Exception("Copying root directory not supported");
        //    }
        //    ProcessHelper.RunCommand("xcopy", $"{sourceDirectory} {targetDirectory} /e /c", readOutput: true, timeoutMsec: timeoutMsec);
        //    Console.Error.WriteLine($"Copied directory {sourceDirectory} to {targetDirectory}");
        //}

        /// <summary>
        /// Deletes a directory under the user's automation temp directory only
        /// </summary>
        /// <param name="folderName">Name of folder to delete under the Automation temp folder to delete</param>
        /// <param name="includeSubFolders">Option to delete subfolders under the folder name</param>
        /// <returns></returns>
        public static void DeleteTempDirectory(string folderName, bool includeSubFolders)
        {
            string path = $@"{AutomationTempRoot}\{folderName}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, includeSubFolders);
            }
        }

        /// <summary>
        /// Deletes a directory
        /// </summary>
        /// <param name="directoryPath">Path of directory folder to delete</param>
        /// <param name="includeSubFolders">Option to delete subfolders under the folder name</param>
        /// <returns></returns>
        public static void DeleteDirectory(string directoryPath, bool includeSubFolders)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, includeSubFolders);
            }
        }

        /// <summary>
        /// Gets a random temp file name from the automation temp folder.
        /// </summary>
        /// <returns></returns>
        public static string GetTempFile()
        {
            return $"{Path.GetTempPath()}Automation\\{Path.GetRandomFileName()}";
        }

        /// <summary>
        /// Creates a temporary directory under the user's automation temp folder.
        /// Returns the full path to the directory
        /// </summary>
        /// <returns></returns>
        public static string CreateTempDirectory()
        {
            string tempProfileFolder = $@"{AutomationTempRoot}\{Path.GetRandomFileName().Replace(".", "")}";
            Directory.CreateDirectory(tempProfileFolder);
            return tempProfileFolder;
        }

        /// <summary>
        /// Returns the full file names of the files that matches a search pattern in a specified directory.
        /// </summary>
        /// <param name="path">The full path to the directory to search.</param>
        /// <param name="searchPattern">Search pattern to match against including file extension and wildcard (*.txt, *.*, List*.xlsx)</param>
        /// <param name="includeDirectories">Should include subdirectories or not</param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string searchPattern, bool includeSubDirectories)
        {
            var searchLevel = includeSubDirectories == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.EnumerateFiles(path, searchPattern, searchLevel).ToArray();
        }

        /// <summary>
        /// Gets the number of files in the target directory and optionally any subdirectories.
        /// </summary>
        /// <param name="path">Full path to the directory</param>
        /// <param name="searchPattern">Search pattern to match against including file extension and wildcard (*.txt, *.*, List*.xlsx)</param>
        /// <param name="includeDirectories">Should include subdirectories or not</param>
        /// <returns></returns>
        public static int GetFilesCount(string path, string searchPattern, bool includeSubDirectories)
        {
            return GetFiles(path, searchPattern, includeSubDirectories).Length;
        }

        /// <summary>
        /// Deletes all files with provided extension in a directory only (no file in subdirectories are deleted).
        /// </summary>
        /// <param name="path">Full path to directory to delete</param>
        /// <param name="fileExt">File extension to delete (*.txt, *.*)</param>
        /// <returns></returns>
        public static bool DeleteFiles(string path, string fileExt)
        {
            bool deleted = false;

            if (!Directory.Exists(path))
            {
                Console.Error.WriteLine($"Directory {path} not found");
                return false;
            }

            foreach (var file in Directory.EnumerateFiles(path, fileExt))
            {
                File.Delete(file);
            }

            if (Directory.EnumerateFiles(path, fileExt).Count() == 0)
            {
                Console.WriteLine($"All files with extension '{fileExt}' in directory '{path}' deleted");
                deleted = true;
            }

            return deleted;
        }

        /// <summary>
        /// Waits for a file to exist up to max wait time.
        /// </summary>
        /// <param name="filePath">Full file path to check for exists</param>
        /// <param name="timeoutMsec">Max timeout to wait for in milliseconds</param>
        /// <returns></returns>
        public static bool WaitForFileExists(string filePath, int timeoutMsec = 60000)
        {
            var endTime = DateTime.Now.AddMilliseconds(timeoutMsec);

            while (DateTime.Now.CompareTo(endTime) <= 0)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }
                System.Threading.Thread.Sleep(1000);
            }
            return false;
        }

        /// <summary>
        /// Reads all lines from the file as string
        /// </summary>
        /// <param name="filePath">File path from which text needs to be read</param>
        /// <returns>File content as string</returns>
        public static string ReadAllTextfromFile(string filePath)
        {
            string text = File.ReadAllText(filePath);
            return text;
        }

        /// <summary>
        /// Writes content to file
        /// </summary>
        /// <param name="filePath">File path to which text needs to be written</param>
        /// <param name="content">File content</param>
        public static void WriteAllTexttoFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
        /// <summary>
        /// Returns the full line based on labelText
        /// </summary>
        /// <param name="textFilePath">The text File Path</param>
        /// <param name="labelText">label Text that need to be searched in text file)</param>
        /// <returns></returns>
        public static string GetLineByText(string textFilePath, string labelText)
        {
            string line;

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(textFilePath);

            while ((line = file.ReadLine()) != null)
            {
                if (labelText != "" && line.Contains(labelText)) break;
            }

            file.Close();
            return line;
        }

        /// <summary>
        /// Returns the full line based on Line Number
        /// </summary>
        /// <param name="textFilePath">The text File Path</param>
        /// <param name="LineNo">Line Number</param>
        /// <returns></returns>
        public static string GetLineByLineNo(string textFilePath, int LineNo)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(textFilePath);

            while ((line = file.ReadLine()) != null)
            {
                counter++;
                if (LineNo != 0 && counter == LineNo) break;
            }

            file.Close();
            return line;
        }
        /// <summary>
        /// Returns Line Number based on labelText
        /// </summary>
        /// <param name="textFilePath">The text File Path</param>
        /// <param name="labelText">label Text that need to be searched in text file)</param>
        /// <returns></returns>
        public static int GetLineNoByText(string textFilePath, string labelText)
        {
            int LineNo = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(textFilePath);
            while ((line = file.ReadLine()) != null)
            {
                LineNo++;
                if (line.Contains(labelText)) break;
            }

            file.Close();
            return LineNo;
        }
        /// <summary>
        /// Returns True/False upon the presense of specified string in the text File
        /// </summary>
        /// <param name="textFilePath">The text File Path</param>
        /// <param name="value">value that need to be searched in text file)</param>
        /// <returns></returns>
        public static bool VerifyStringInFile(string textFilePath, string value)
        {
            StreamReader file = new StreamReader(textFilePath);
            string data = file.ReadToEnd();
            file.Close();
            return data.Contains(value);
        }
    }
}
