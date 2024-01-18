using System;
using System.IO;

class Program
{
    static void Main()
    {
        string folderPath = GetFolderPathFromUser();
        if (Directory.Exists(folderPath))
        {
            try
            {
                long folderSize = GetFolderSize(folderPath);
                Console.WriteLine($"Размер папки {folderPath} составляет {folderSize} байт");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при подсчете размера папки: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Указанного пути к папке не существует");
        }
    }

    static string GetFolderPathFromUser()
    {
        Console.Write("Введите путь к папке: ");
        return Console.ReadLine();
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
