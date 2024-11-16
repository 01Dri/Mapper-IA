namespace MapperIA.Tests.MappedClasses;
public class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string StudentId { get; set; }
    public Student(string name, int age, string studentId)
    {
        Name = name;
        Age = age;
        StudentId = studentId;
    }
    public bool IsAdult()
    {
        return Age >= 18;
    }
}