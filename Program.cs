using Azure;
using Microsoft.EntityFrameworkCore;
using Student_Platform_DB_Exam.Models;
using Student_Platform_DB_Exam.Services;
using System;
using static Student_Platform_DB_Exam.Models.Student;

namespace University_DB_Exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //----------- program start ----------
            using var dbContext = new UniContext();

            Menu menu = new Menu();
            while (true)
            {
                Console.Clear();
                menu.DisplayMainMenuOptions();

                Console.Write("Enter your choice: ");
                string userChoice = Console.ReadLine();

                menu.ProcessMainMenuChoice(userChoice);
            }

        }


        // ====================================== METHODS  ======================================        

        #region ShowMethods        
        public static void ShowAllFaculties()
        {
            Show.AllFaculties();
        }

        public static void ShowAllLectures()
        {
            Show.AllLectures();
        }

        //...show all students
        public static void ShowAllStudents()
        {
            Show.AllStudents();
        }

        //...show all students from a faculty
        public static void ShowAllStudentsFromFaculty()
        {
            Show.AllStudentsFromFaculty();
        }

        //...show all lectures of a faculty
        public static void ShowAllLecturesOfFaculty()
        {
            Show.AllLecturesOfFaculty();
        }

        //...show all lectures of individual student
        public static void ShowAllLecturesOfOneStudent()
        {
            Show.AllLecturesOfOneStudent();
        }

        #endregion

        //----------

        #region CreateNew


        // ...creates new faculty
        public static void CreateNewFaculty()
        {
            Create.CreateNewFaculty();

        }

        // ...creates new lecture
        public static void CreateNewLecture()
        {
            Create.CreateNewLecture();
        }

        // ...creates new student
        public static void CreateNewStudent()
        {
            Create.CreateNewStudent();
        }
        public static void CreateNewWorker()
        {
            Create.CreateNewWorker();
        }

        #endregion

        //----------       


        public static void InputKeyToContinue()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

    }
}
