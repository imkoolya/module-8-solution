using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите путь к папке:");
        string folderPath = Console.ReadLine();
        var thresholdMinutes = TimeSpan.FromMinutes(30);

        try
        {
            CleanUnusedFilesAndFolders(folderPath, thresholdMinutes);
            Console.WriteLine("Операция завершена.");
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

    private static void CleanUnusedFilesAndFolders(string folderPath, TimeSpan thresholdMinutes)
    {
        throw new NotImplementedException();
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
}