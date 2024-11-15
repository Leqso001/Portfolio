using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileOrganizer
{
    class Program
    {
        private static readonly Dictionary<string, string[]> FileCategories = new()
        {
            { "Images", new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" } },
            { "Documents", new[] { ".pdf", ".docx", ".doc", ".txt", ".xlsx", ".pptx" } },
            { "Videos", new[] { ".mp4", ".mkv", ".avi", ".mov", ".flv" } },
            { "Music", new[] { ".mp3", ".wav", ".aac", ".flac" } },
            { "Archives", new[] { ".zip", ".rar", ".7z", ".tar", ".gz" } }
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to File Organizer!");

            Console.Write("Enter the directory path to organize: ");
            string directoryPath = Console.ReadLine();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Invalid directory path. Exiting...");
                return;
            }

            Console.Write("Do you want to organize files in subdirectories as well? (y/n): ");
            bool includeSubdirectories = Console.ReadLine()?.Trim().ToLower() == "y";

            OrganizeFiles(directoryPath, includeSubdirectories);

            Console.WriteLine("\nFile organization complete!");
            Console.WriteLine("Check the folder for organized files and the log file for details.");
        }

        static void OrganizeFiles(string directoryPath, bool includeSubdirectories)
        {
            SearchOption searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var files = Directory.GetFiles(directoryPath, "*.*", searchOption);

            if (!files.Any())
            {
                Console.WriteLine("No files found in the specified directory.");
                return;
            }

            string logFilePath = Path.Combine(directoryPath, "FileOrganizerLog.txt");
            using StreamWriter logFile = new StreamWriter(logFilePath, false);

            foreach (var file in files)
            {
                string fileExtension = Path.GetExtension(file).ToLower();
                string category = GetCategory(fileExtension);

                if (category == null)
                {
                    logFile.WriteLine($"Skipped: {file} (Unknown file type)");
                    continue;
                }

                string targetFolder = Path.Combine(directoryPath, category);
                Directory.CreateDirectory(targetFolder);

                string targetPath = Path.Combine(targetFolder, Path.GetFileName(file));

                try
                {
                    File.Move(file, targetPath);
                    logFile.WriteLine($"Moved: {file} -> {targetPath}");
                }
                catch (Exception ex)
                {
                    logFile.WriteLine($"Failed: {file} -> {targetPath} ({ex.Message})");
                }
            }

            Console.WriteLine($"Log file created at: {logFilePath}");
        }

        static string GetCategory(string extension)
        {
            foreach (var category in FileCategories)
            {
                if (category.Value.Contains(extension))
                {
                    return category.Key;
                }
            }
            return null;
        }
    }
}
