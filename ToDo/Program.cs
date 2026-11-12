using ToDo;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Text.Json.Serialization;


// Display instructions
Console.WriteLine("--------------------------------");
Console.WriteLine("          ToDo List");
Console.WriteLine("--------------------------------");
Console.WriteLine();

// Initialize list
List<TodoItem> tasks = new List<TodoItem>();

// Initialize helper class'
TodoManager todoManager = new();

// Load previously saved tasks
todoManager.LoadTasks(ref tasks);

while (true)
{

    string menuOption = todoManager.TodoListMenu();

    switch (menuOption)
    {
        case "1":
            todoManager.AddTask(tasks);
            break;
                
        // Perform task sort and display
        case "2":
            todoManager.ShowTasks(tasks);
            break;

        // Perform task search
        case "3":
            todoManager.SearchTasks(tasks);
            break;

        // Delete task from list
        case "4":
            todoManager.DeleteTask(tasks);
            break;

        // Edit task
        case "5":
            todoManager.EditTask(tasks);
            break;

        // Mark task as completed
        case "6":
            todoManager.CompleteTask(tasks);
            break;

        // Exit program
        case "7":
            todoManager.SaveTasks(tasks);
            Environment.Exit(0);
            break;
    }
}


