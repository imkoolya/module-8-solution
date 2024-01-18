using System;
using System.IO;

class Program
{
    static void Main()
    {
        string folderPath = @"C:\путь\к\папке";
        int thresholdMinutes = 30;
        long folderSize = GetFolderSize(folderPath);
        Console.WriteLine($"Размер папки {folderPath} до составляет {folderSize} байт");

        try
        {
            CleanUnusedFilesAndFolders(folderPath, thresholdMinutes);
            Console.WriteLine("Операция завершена.");
            Console.WriteLine($"Размер папки {folderPath} после составляет {folderSize} байт");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Отсутствуют права доступа к указанной директории.");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Указанная директория не существует.");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Некорректный путь к директории.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static void CleanUnusedFilesAndFolders(string path, int thresholdMinutes)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException();
        }

        string[] files = Directory.GetFiles(path);
        string[] subdirectories = Directory.GetDirectories(path);

        foreach (string file in files)
        {
            FileInfo fileInfo = new FileInfo(file);
            if ((DateTime.Now - fileInfo.LastAccessTime).TotalMinutes > thresholdMinutes)
            {
                File.Delete(file);
                Console.WriteLine($"Файл {file} удален, так как он не использовался более {thresholdMinutes} минут.");
            }
        }

        foreach (string directory in subdirectories)
        {
            CleanUnusedFilesAndFolders(directory, thresholdMinutes);

            if (Directory.GetFileSystemEntries(directory).Length == 0)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directory);
                if ((DateTime.Now - dirInfo.LastAccessTime).TotalMinutes > thresholdMinutes)
                {
                    Directory.Delete(directory);
                    Console.WriteLine($"Папка {directory} удалена, так как она не использовалась более {thresholdMinutes} минут.");
                }
            }
        }
    }
    static long GetFolderSize(string path)
    {
        long size = 0;
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            size += new FileInfo(file).Length;
        }
        string[] subdirectories = Directory.GetDirectories(path);
        foreach (string subdirectory in subdirectories)
        {
            size += GetFolderSize(subdirectory);
        }
        return size;
    }
}