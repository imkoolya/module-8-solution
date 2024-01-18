using System;
using System.Collections.Generic;
using System.IO;

class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal AverageGrade { get; set; }
}

class Program
{
    static void Main()
    {
        string binaryFilePath = GetBinaryFilePathFromUser();

        try
        {
            List<Student> students = ReadStudentsFromBinaryFile(binaryFilePath);
            CreateDirectoryForStudents();
            DistributeStudentsByGroup(students);
            Console.WriteLine("Программа успешно завершила свою работу.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static string GetBinaryFilePathFromUser()
    {
        Console.Write("Введите путь к бинарному файлу: ");
        return Console.ReadLine();
    }

    static List<Student> ReadStudentsFromBinaryFile(string filePath)
    {
        throw new NotImplementedException();
    }

    static void CreateDirectoryForStudents()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string studentsDirectoryPath = Path.Combine(desktopPath, "Students");
        Directory.CreateDirectory(studentsDirectoryPath);
    }

    static void DistributeStudentsByGroup(List<Student> students)
    {
        foreach (var group in students.GroupBy(s => s.Group))
        {
            string groupFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Students", $"{group.Key}.txt");
            using (StreamWriter writer = new StreamWriter(groupFilePath))
            {
                foreach (var student in group)
                {
                    writer.WriteLine($"{student.Name}, {student.DateOfBirth}, {student.AverageGrade}");
                }
            }
        }
    }
}