using System.Linq;
using Microsoft.AspNetCore.Identity;
using DAL.Interface;
using Model.DB;

namespace DAL.Seed
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MainDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;

        public DbInitializer(
            MainDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
        }

        public void Initialize()
        {
            context.Database.EnsureCreated();

            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
                string userAdmin = "admin@gmail.com";
                string passwordAdmin = "Admin_123";
                userManager.CreateAsync(new User { UserName = userAdmin, Email = userAdmin, EmailConfirmed = true }, passwordAdmin).Wait();
                var t = userManager.FindByNameAsync(userAdmin);
                userManager.AddToRoleAsync(t.Result, "Administrator").Wait();
            }

            if (!context.Roles.Any(r => r.Name == "Teacher"))
            {
                roleManager.CreateAsync(new IdentityRole("Teacher")).Wait();
                string userTeacher = "teacher@gmail.com";
                string passwordTeacher = "Teacher_123";
                userManager.CreateAsync(new User { UserName = userTeacher, Email = userTeacher, EmailConfirmed = true }, passwordTeacher).Wait();
                var t = userManager.FindByNameAsync(userTeacher);
                userManager.AddToRoleAsync(t.Result, "Teacher").Wait();

                string userTeacher2 = "teacher2@gmail.com";
                userManager.CreateAsync(new User { UserName = userTeacher2, Email = userTeacher2, EmailConfirmed = true }, passwordTeacher).Wait();
                var t2 = userManager.FindByNameAsync(userTeacher2);
                userManager.AddToRoleAsync(t2.Result, "Teacher").Wait();
            }
            if (!context.Roles.Any(r => r.Name == "Student"))
            {
                roleManager.CreateAsync(new IdentityRole("Student")).Wait();
                string userStudent = "student@gmail.com";
                string passwordStudent = "Student_123";
                userManager.CreateAsync(new User { UserName = userStudent, Email = userStudent, EmailConfirmed = true }, passwordStudent).Wait();
                var t = userManager.FindByNameAsync(userStudent);
                userManager.AddToRoleAsync(t.Result, "Student").Wait();

                string userStudent2 = "student2@gmail.com";
                userManager.CreateAsync(new User { UserName = userStudent2, Email = userStudent2, EmailConfirmed = true }, passwordStudent).Wait();
                var t2 = userManager.FindByNameAsync(userStudent2);
                userManager.AddToRoleAsync(t2.Result, "Student").Wait();

                string userStudent3 = "student3@gmail.com";
                userManager.CreateAsync(new User { UserName = userStudent3, Email = userStudent3, EmailConfirmed = true }, passwordStudent).Wait();
                var t3 = userManager.FindByNameAsync(userStudent3);
                userManager.AddToRoleAsync(t3.Result, "Student").Wait();
            }

            if (!context.Courses.Any(r => r.Name == "C# Starter"))
            {
                unitOfWork.CourseRepo.Insert(new Course
                {
                    Name = "C# Starter",
                    Description = @"The ""C# Starter"" course is a unique course for beginners, which will allow you to begin learning the C# programming language without having any special preliminary training.",
                    IsActive = true,
                    CreationDate = System.DateTime.Now,
                    UserId = unitOfWork.UserRepo.Get(c => c.Email == "teacher@gmail.com").First().Id
                });
            }
            if (!context.Courses.Any(r => r.Name == "C# Essential"))
            {
                unitOfWork.CourseRepo.Insert(new Course
                {
                    Name = "C# Essential",
                    Description = @"The course ""C# Essential"" will allow you to fully understand the syntax of the C# language and its semantics. It will give you the necessary level of knowledge and skills, you will master the basic capabilities of the C# programming language and this will be a good foundation for learning more complex technologies that .NET Developer should possess.",
                    IsActive = true,
                    CreationDate = System.DateTime.Now,
                    UserId = unitOfWork.UserRepo.Get(c => c.Email == "teacher2@gmail.com").First().Id
                });
            }
            if (!context.Courses.Any(r => r.Name == "C# Advanced"))
            {
                unitOfWork.CourseRepo.Insert(new Course
                {
                    Name = "C# Advanced",
                    Description = @"The ""C# Advanced"" course focuses on in-depth study of the Microsoft .NET Framework and C#. In this course you will learn what is reflection and attributes, serialization and garbage collection.",
                    IsActive = true,
                    CreationDate = System.DateTime.Now,
                    UserId = unitOfWork.UserRepo.Get(c => c.Email == "teacher2@gmail.com").First().Id
                });
            }
            unitOfWork.Save();

            if (!context.Exercises.Any(r => r.TaskName == "Simple addition"))
            {
                string code = 
@"public class Program
{
    public static int Addition(int a, int b)
    {
        return a + b;
    }
}";
                string testCasesCode = 
@"using NUnit.Framework;
[TestFixture]
public class UnitTest
{
    [Test, TestCaseSource(""Cases"")]
    public void TestMethod(int expected, int a, int b)
    {
        Assert.AreEqual(expected, Program.Addition(a, b));
    }
    static object[] Cases =
    {
        new object[] { 12, 8, 4 },
        new object[] { 12, 6, 6 },
        new object[] { 12, 2, 10 }
    };
}";
                unitOfWork.ExerciseRepo.Insert(new Exercise
                {
                    TaskName = "Simple addition",
                    TaskTextField = "Implement a simple function which will add two integer numbers and return their sum.",
                    Course = "C# Starter",
                    CourseId = unitOfWork.CourseRepo.Get(c => c.Name == "C# Starter").First().Id,
                    IsDeleted = false,
                    CreateDateTime = System.DateTime.Now,
                    UpdateDateTime = System.DateTime.Now,
                    TeacherId = unitOfWork.UserRepo.Get(c => c.Email == "teacher@gmail.com").First().Id,
                    TaskBaseCodeField = code,
                    TestCasesCode = testCasesCode
                });
            }

            if (!context.Exercises.Any(r => r.TaskName == "Indexers"))
            {
                string code =
@"public class Indexer
{
        private int[] TargetArray { get; set; }

        public Indexer(int[] arr)
        {
            TargetArray = arr;
        }

        public int this[int index]
        {
            get
            {
                if (index >= 0 && index < TargetArray.Length)
                {
                    return TargetArray[index];
                }
                return -1;
            }
            set
            {
                if (index >= 0 && index < TargetArray.Length)
                {
                    TargetArray[index] = value;
                }
            }
        }

        public int TestMethod(int value)
        {
            return Array.IndexOf(TargetArray, value);
        }
}";
                string testCasesCode =
@"using NUnit.Framework;
using System;
[TestFixture]
public class UnitTest
{
        [Test, TestCaseSource(""Cases"")]
        public void TestMethod(int expected, int[] array, int value)
                {
                    Assert.AreEqual(expected, new Indexer(array).TestMethod(value));
                }
                static object[] Cases =
                {
            new object[] { 3, new int[] {1, 2, 3, 4 ,5}, 4 },
            new object[] { -1, new int[] {1, 2, 3, 4 ,5}, 10 }
        };
            }
            ";
                unitOfWork.ExerciseRepo.Insert(new Exercise
                {
                    TaskName = "Indexers",
                    TaskTextField = "Create a class which must contain an array of integer numbers. Add constructor with one parameter(type of in-class array) and initialize the array. Create an indexer which will allow to access members of array. Finally, create a method: int parameter - integer value; int output - index of value in array. If value does not exist in array return -1.",
                    Course = "C# Essential",
                    CourseId = unitOfWork.CourseRepo.Get(c => c.Name == "C# Essential").First().Id,
                    IsDeleted = false,
                    CreateDateTime = System.DateTime.Now,
                    UpdateDateTime = System.DateTime.Now,
                    TeacherId = unitOfWork.UserRepo.Get(c => c.Email == "teacher@gmail.com").First().Id,
                    TaskBaseCodeField = code,
                    TestCasesCode = testCasesCode
                });
            }

            if (!context.Exercises.Any(r => r.TaskName == "Elevator modeling"))
            {
                string code =
@"public class Elevator
    {
        public Status Direction { get; set; }
        public int CurrentFloor { get; set; }
        public int ResultFloor { get; set; }

        public Elevator(int currentFloor, int resultFloor)
        {
            this.CurrentFloor = currentFloor;
            this.ResultFloor = resultFloor;
            if (CurrentFloor - ResultFloor > 0)
            {
                Direction = Status.Down;
            }
            else if (CurrentFloor - ResultFloor < 0)
            {
                Direction = Status.Up;
            }
            else
            {
                Direction = Status.Stop;
            }
        }

        public override bool Equals(object obj)
        {
            return CurrentFloor == ((Elevator)obj).CurrentFloor && ResultFloor == ((Elevator)obj).ResultFloor;
        }

        public override int GetHashCode()
        {
            return CurrentFloor.GetHashCode() + ResultFloor.GetHashCode();
        }
    }

    public class Person
    {
        public Status Direction { get; set; }
        public int Floor { get; set; }

        public Person(Status state, int floor)
        {
            this.Direction = state;
            this.Floor = floor;
        }
    }

    public enum Status
    {
        Stop, Up, Down
    }

    public class ElevatorSelector
    {
        private List<Elevator> Elevators;

        public ElevatorSelector(params Elevator[] items)
        {
            Elevators = new List<Elevator>();
            foreach (var item in items)
            {
                Elevators.Add(item);
            }
        }

        public Elevator Find(Person person)
        {
            if (Elevators.Count != 0)
            {
                bool isAnyPast = false;
                int minOptimalTimeForPast = Int16.MaxValue;
                int minOptimalTime = Int16.MaxValue;
                Elevator optimal = null;
                foreach (var elevator in Elevators)
                {
                    if (person.Direction == elevator.Direction &&
                        (
                          (person.Floor >= elevator.CurrentFloor && person.Floor <= elevator.ResultFloor) ||
                          (person.Floor >= elevator.ResultFloor && person.Floor <= elevator.CurrentFloor)
                        )
                       )
                    {
                        if (Math.Abs(person.Floor - elevator.CurrentFloor) < minOptimalTimeForPast)
                        {
                            minOptimalTimeForPast = Math.Abs(person.Floor - elevator.CurrentFloor);
                            optimal = elevator;
                        }
                        isAnyPast = true;
                    }
                    else if (!isAnyPast)
                    {
                        int temp = Math.Abs(elevator.CurrentFloor - elevator.ResultFloor) + Math.Abs(person.Floor - elevator.ResultFloor);
                        if (temp < minOptimalTime)
                        {
                            minOptimalTime = temp;
                            optimal = elevator;
                        }
                    }
                }
                return optimal;
            }
            return null;
        }
    }";
                string testCasesCode =
@"using NUnit.Framework;
using System;
using System.Collections.Generic;
[TestFixture]
public class ElevatorModelUnitTest
    {
        [Test, TestCaseSource(""TestCases"")]
        public void ElevatorTest(Person person, ElevatorSelector elevatorSelector, Elevator expected)
                {
                    Assert.AreEqual(expected, elevatorSelector.Find(person));
                }

                static object[] TestCases =
                {
        new object[] {new Person(Status.Up, 7), new ElevatorSelector(new Elevator(1, 6), new Elevator(8, 8), new Elevator(9, 6), new Elevator(9, 10)), new Elevator(8, 8)},
        new object[] {new Person(Status.Up, 7), new ElevatorSelector(new Elevator(10, 20), new Elevator(1, 8), new Elevator(6, 6)), new Elevator(1, 8) },
        new object[] {new Person(Status.Up, 7), new ElevatorSelector(new Elevator(1, 10), new Elevator(6, 8)), new Elevator(6, 8) },
        new object[] {new Person(Status.Down, 8), new ElevatorSelector(new Elevator(7, 8), new Elevator(10, 9), new Elevator(8, 8), new Elevator(10, 8)), new Elevator(10, 8)}
        };
            }
            ";
                unitOfWork.ExerciseRepo.Insert(new Exercise
                {
                    TaskName = "Elevator modeling",
                    TaskTextField = "Write a simple program that will accept a set of elevators with parameters and information about pressing a button by the user. Output the best lift for the current user.",
                    Course = "C# Advanced",
                    CourseId = unitOfWork.CourseRepo.Get(c => c.Name == "C# Advanced").First().Id,
                    IsDeleted = false,
                    CreateDateTime = System.DateTime.Now,
                    UpdateDateTime = System.DateTime.Now,
                    TeacherId = unitOfWork.UserRepo.Get(c => c.Email == "teacher@gmail.com").First().Id,
                    TaskBaseCodeField = code,
                    TestCasesCode = testCasesCode
                });
            }

            unitOfWork.Save();
        }
    }
}
