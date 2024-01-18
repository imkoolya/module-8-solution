namespace task_4
{
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageScore { get; set; }
    }
    class Program
    {
        static string GetBinaryFilePathFromUser()
        {
            Console.Write("Введите путь к бинарному файлу: ");
            return Console.ReadLine();
        }
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
        static List<Student> ReadStudentsFromBinaryFile(string fileName)
        {
            List<Student> result = new();
            using FileStream fs = new FileStream(fileName, FileMode.Open);
            using StreamReader sr = new StreamReader(fs);

            Console.WriteLine(sr.ReadToEnd());

            fs.Position = 0;

            BinaryReader br = new BinaryReader(fs);

            while (fs.Position < fs.Length)
            {
                Student student = new Student();
                student.Name = br.ReadString();
                student.Group = br.ReadString();
                long dt = br.ReadInt64();
                student.DateOfBirth = DateTime.FromBinary(dt);
                student.AverageScore = br.ReadDecimal();

                result.Add(student);
            }

            fs.Close();
            return result;
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
                        writer.WriteLine($"{student.Name}, {student.DateOfBirth}, {student.AverageScore}");
                    }
                }
            }
        }
    }
}