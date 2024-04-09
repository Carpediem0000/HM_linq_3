using System.Linq;

namespace HM_linq_3
{
    class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<int> CourseIds { get; set; }

        public override string ToString()
        {
            return $"Id: {StudentId}, Name: {Name}, Age: {Age}, CourseIds: [{CourseIds.ConvertAll<string>(id => id.ToString()).Aggregate((res, next) => res + ", " + next)}]";
        }
    }

    class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>
            {
              new Student { StudentId = 1, Name = "Alice", Age = 20, CourseIds = new List<int> { 1, 2, 3 } },
              new Student { StudentId = 2, Name = "Bob", Age = 22, CourseIds = new List<int> { 2, 3, 4 } },
              new Student { StudentId = 3, Name = "Charlie", Age = 21, CourseIds = new List<int> { 1, 3, 5 } },
            };

            List<Course> courses = new List<Course>
            {
              new Course { CourseId = 1, CourseName = "Math" },
              new Course { CourseId = 2, CourseName = "Physics" },
              new Course { CourseId = 3, CourseName = "Computer Science" },
              new Course { CourseId = 4, CourseName = "Biology" },
              new Course { CourseId = 5, CourseName = "Chemistry" },
            };
            ///////////////////////////////////////////////////////////////////////
            //Вывести имена студентов, которые старше 21 года.

            var task_1 = students.Where(s => s.Age > 21);
            foreach (var student in task_1)
            {
                Console.WriteLine(student.Name);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Найти средний возраст студентов по каждому курсу.

            var task_2 = from courseId in students.SelectMany(s => s.CourseIds)
                         group courseId by courseId into g
                         select new
                         {
                             CourseId = g.Key,
                             AverageAge = students.Where(s => s.CourseIds.Contains(g.Key)).Average(s => s.Age)
                         };
            foreach (var student in task_2)
            {
                Console.WriteLine(student);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Вывести названия курсов, на которых учится более двух студентов.

            var task_3 = (from courseId in students.SelectMany(s => s.CourseIds)
                         from course in courses
                         where course.CourseId == courseId
                         select new
                         {
                             CourseName = course.CourseName,
                             Count = students.Where(s => s.CourseIds.Contains(courseId)).Count(),
                         }).Distinct().Where(c => c.Count > 1);
            foreach (var item in task_3)
            {
                Console.WriteLine(item.CourseName + ": " + item.Count);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Найти студента с наибольшим возрастом.

            var task_4 = students.Where(s => s.Age == students.Max(s => s.Age));
            foreach (var item in task_4)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Вывести имена студентов, у которых нет курсов.

            var task_5 = students.Where(s => s.CourseIds == null);
            foreach (var item in task_5)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Найти суммарный возраст студентов на каждом курсе.

            var task_6 = from courseId in students.SelectMany(s => s.CourseIds)
                         group courseId by courseId into g
                         select new
                         {
                             CourseId = g.Key,
                             SumAge = students.Where(s => s.CourseIds.Contains(g.Key)).Sum(s => s.Age)
                         };

            foreach (var item in task_6)
            {
                Console.WriteLine(item.CourseId + ": " + item.SumAge + " y.o");
            }
            Console.WriteLine("-------------------------------------------------------");

            //Вывести имена студентов, у которых есть общие курсы.

            foreach (var item in students)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Найти средний возраст студентов, у которых есть общие курсы.

            int SumAge = 0;
            foreach (var item in students)
            {
                SumAge += item.Age;
            }
            Console.WriteLine(SumAge);
            Console.WriteLine("-------------------------------------------------------");

            //Вывести имена студентов, у которых средний возраст на курсе больше 20 лет.

            var task_9_1 = (from courseId in students.SelectMany(s => s.CourseIds)
                         group courseId by courseId into g
                         select new
                         {
                             CourseId = g.Key,
                             AverageAge = students.Where(s => s.CourseIds.Contains(g.Key)).Average(s => s.Age)
                         }).Where(a => a.AverageAge > 20);
            var task_9_2 = (from student in students
                           from CourseId in task_9_1
                           where student.CourseIds.Contains(CourseId.CourseId)
                           select student).Distinct();

            foreach (var item in task_9_2)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Найти курс с наибольшим числом студентов.

            var task_10 = from courseId in students.SelectMany(s => s.CourseIds)
                         group courseId by courseId into g
                         select new
                         {
                             CourseId = g.Key,
                             CountStudents = students.Where(s => s.CourseIds.Contains(g.Key)).Count()
                         };

            foreach (var item in task_10.Where(s => s.CountStudents == task_10.Max(c => c.CountStudents)))
            {
                Console.WriteLine("CourseId: {0}, CountStudents: {1}", item.CourseId, item.CountStudents);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Вывести имена студентов, у которых средний возраст на курсе максимален.

            var task_11_1 = (from courseId in students.SelectMany(s => s.CourseIds)
                            group courseId by courseId into g
                            select new
                            {
                                CourseId = g.Key,
                                AverageAge = students.Where(s => s.CourseIds.Contains(g.Key)).Average(s => s.Age)
                            }).Where(a => a.AverageAge > 20);
            var task_11_2 = from student in students
                            from CourseId in task_11_1.Where(s => s.AverageAge == task_11_1.Max(c => c.AverageAge)).Select(s => s.CourseId)
                            where student.CourseIds.Contains(CourseId)
                            select student;

            foreach (var item in task_11_2)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Найти курсы, на которых учится хотя бы один студент старше 25 лет.

            var task_12 = from courseId in students.SelectMany(s => s.CourseIds)
                          from student in students
                          where student.CourseIds.Contains(courseId) && student.Age > 25
                          select student;
            foreach (var item in task_12)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Вывести имена студентов, у которых возраст на курсе отличается не более чем на 1 год.

            Console.WriteLine("-------------------------------------------------------");

            //Найти курсы, на которых нет студентов.

            var task_14 = (from courseId in students.SelectMany(s => s.CourseIds)
                          group courseId by courseId into g
                          select new
                          {
                              CourseId = g.Key,
                              CountStudents = students.Where(s => s.CourseIds.Contains(g.Key)).Count()
                          }).Where(c => c.CountStudents == 0);
            foreach (var item in task_14)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Вывести имена студентов, у которых есть курсы и которые не учатся на курсах "Math" и "Physics".

            var task_15 = students.Where(c => !c.CourseIds.Contains(1) && !c.CourseIds.Contains(2) && c.CourseIds != null);
            foreach (var item in task_15)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");

            //Найти студентов, у которых есть курсы и которые учатся хотя бы на одном курсе в каждой категории(Math, Physics, Computer Science).

            var task_16 = students.Where(c => (c.CourseIds.Contains(1) || c.CourseIds.Contains(2) || c.CourseIds.Contains(3)) && c.CourseIds != null);
            foreach (var item in task_16)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-------------------------------------------------------");
        }
    }
}
