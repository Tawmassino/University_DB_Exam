using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Student_Platform_DB_Exam.Models;
using Student_Platform_DB_Exam.Services;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Buffers.Text;
using System.Diagnostics.Metrics;
using static Student_Platform_DB_Exam.Models.Student;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace University_DB_Exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Summary:

            // Program Purpose:

            //    The program is university management system, allowing users to interact with data related to lectures, students, workers, and faculties.

            //Design and Structure:

            //    The program is structured around a database context(UniContext) using Entity Framework Core, which  represents the data model of a university.
            //    It uses a console-based user interface for interaction.
            //    It consists of functions to create, update, and remove entities like lectures, students, workers, and faculties.

            //Abilities:

            //    create new lectures, students, and workers, and associate them with faculties.
            //    update details of lectures, students, and workers.
            //    remove lectures, students, and faculties.
            //    display information about lectures, students, and faculties.

            //Limitations:

            //    Due to limited development time and lack of skill on the part of the programmer there are quite a few elements of the program that could be deemed lacking.
            //    The error handling is basic, with generic exception handling. More specific exception handling could be implemented for better error reporting.
            //    The program lacks validation for user inputs, which could lead to unexpected behavior if users provide invalid data.
            //    The program might benefit from more structured input validation and user guidance.
            //    The design assumes a console interface, limiting the user experience compared to a graphical user interface (GUI).


            //Possible Improvements:

            //    Implement more specific exception handling to provide meaningful error messages.
            //    Add input validation to ensure that users provide valid data.
            //    Consider implementing a more user-friendly interface, possibly moving to a GUI for improved user experience.
            //    Implement logging to capture important events and errors.
            //    Adding Scores and Attendance. Implementing calendar.
            //    Adding more comments to the code.
            //    There's always room for refactoring.
            //    Tutorial for User.
            //    Needs more debugging.

            //Overall:

            //    The program is a console-based application for managing university-related data, demonstrating CRUD (Create, Read, Update, Delete) operations.
            //    There is room for improvement in terms of user experience, error handling, and input validation.



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
