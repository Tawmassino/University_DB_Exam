using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_DB_Exam;
using Microsoft.EntityFrameworkCore;

namespace Student_Platform_DB_Exam.Services
{
    public class Menu
    {
        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                DisplayMainMenuOptions();

                Console.Write("Enter your choice: ");
                string userChoice = Console.ReadLine();

                ProcessMainMenuChoice(userChoice);
            }
        }

        public void DisplayMainMenuOptions()
        {
            Console.WriteLine("========== Main Menu ==========");
            Console.WriteLine("1. Show");
            Console.WriteLine("2. Create");
            Console.WriteLine("3. Change");
            Console.WriteLine("0. Exit");
        }

        public void ProcessMainMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    ShowMenu();
                    break;

                case "2":
                    CreateMenu();
                    break;

                case "3":
                    ChangeMenu();
                    break;

                case "0":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }

        private void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Show Menu ==========");
            Console.WriteLine("1. Show All Faculties");
            Console.WriteLine("2. Show All Lectures");
            Console.WriteLine("3. Show All Students");
            Console.WriteLine("4. Show Students From Faculty");
            Console.WriteLine("5. Show Lectures Of Faculty");
            Console.WriteLine("6. Show Lectures Of One Student");

            Console.Write("Enter your choice: ");
            string userChoice = Console.ReadLine();

            ProcessShowMenuChoice(userChoice);
        }

        public static void ProcessShowMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    Show.AllFaculties();
                    break;

                case "2":
                    Show.AllLectures();
                    break;

                case "3":
                    Show.AllStudents();
                    break;

                case "4":
                    Show.AllStudentsFromFaculty();
                    break;

                case "5":
                    Show.AllLecturesOfFaculty();
                    break;

                case "6":
                    Show.AllLecturesOfOneStudent();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    InputKeyToContinue();
                    break;
            }
        }

        public void CreateMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Create Menu ==========");
            Console.WriteLine("1. Create New Faculty");
            Console.WriteLine("2. Create New Lecture");
            Console.WriteLine("3. Create New Student");
            Console.WriteLine("4. Create New Worker");

            Console.Write("Enter your choice: ");
            string userChoice = Console.ReadLine();

            ProcessCreateMenuChoice(userChoice);
        }

        public void ProcessCreateMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    Create.CreateNewFaculty();
                    break;

                case "2":
                    Create.CreateNewLecture();
                    break;

                case "3":
                    Create.CreateNewStudent();
                    break;

                case "4":
                    Create.CreateNewWorker();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    InputKeyToContinue();
                    break;
            }
        }

        public void ChangeMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Change Menu ==========");
            Console.WriteLine("1. Update Faculty Data");
            Console.WriteLine("2. Update Lecture");
            Console.WriteLine("3. Update Student Data");

            Console.Write("Enter your choice: ");
            string userChoice = Console.ReadLine();

            ProcessChangeMenuChoice(userChoice);
        }

        public static void ProcessChangeMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    Update.UpdateFacultyData();
                    break;

                case "2":
                    Update.UpdateLecture();
                    break;

                case "3":
                    Update.UpdateStudentData();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    InputKeyToContinue();
                    break;
            }
        }

        public static void InputKeyToContinue()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

    }
}
