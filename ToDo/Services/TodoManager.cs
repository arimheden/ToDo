using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ToDo;

/* 
 * Helper class for todo task list management
 * 
 */
internal class TodoManager
{
    // Request user input
    public String UserInput()
    {
        string? userInput = Console.ReadLine();
        if (userInput is not null)
        {
            userInput = userInput.Trim();
            return userInput;
        }
        else return "";        
    }

    // Validate user input for product name
    public bool ValidateInput(string inputToValidate, string validateType)
    {
        if (String.IsNullOrEmpty(inputToValidate))
        {
            throw new ArgumentException("Input can not be empty!");
        }

        // Validate price
        if (validateType == "date")
        {
            if (DateTime.TryParse(inputToValidate, out DateTime date))
            {
                if (date > DateTime.Now) return true;
                else
                {
                    throw new ArgumentException("Due date can not be in the past!");
                }
            }
            else
            {
                throw new ArgumentException("Due date must be a date!");
            }

        }

        if (validateType == "id")
        {
            if (Int32.TryParse(inputToValidate, out int id))
            {
                if (id > 0) return true;
                else
                {
                    throw new ArgumentException("Todo id must be a number greater than 0!");
                }
            }
            else
            {
                throw new ArgumentException("Todo id must be a number!");
            }
        }
        return true;
    }

    // Adding todo tasks
    public void AddTask(List<TodoItem> tasks)
    {
        Console.WriteLine();
        string todoProject = "";
        string todoTitle = "";
        DateTime todoDueDate = DateTime.Now;

        // Request user input
        bool isValid = false;
        while (!isValid)
        {
            Console.Write("Enter project name: ");
            string userInput = this.UserInput();
            try
            {
                this.ValidateInput(userInput, "project");
                todoProject = userInput;
                isValid = true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        isValid = false;
        while (!isValid)
        {
            Console.Write("Enter task title: ");
            string userInput = this.UserInput();
            try
            {
                this.ValidateInput(userInput, "title");
                todoTitle = userInput;
                isValid = true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }            

        isValid = false;
        while (!isValid)
        {
            Console.Write("Enter task due date: ");
            string userInput = this.UserInput();
            try
            {
                this.ValidateInput(userInput, "date");
                todoDueDate = Convert.ToDateTime(userInput);
                isValid = true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        tasks.Add(new TodoItem(todoProject, todoTitle, todoDueDate, Status.Pending));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Task has been added!");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    // Sort and show tasks
    public void ShowTasks(List<TodoItem> tasks)
    {
        Console.WriteLine();
        Console.WriteLine("===== To Do =====");
        Console.WriteLine();
        foreach (TodoItem task in tasks.OrderBy(task => task.Status).ThenBy(task => task.Project).ThenBy(task => task.DueDate))
        {
            Console.WriteLine(task.Id.ToString().PadRight(5) + " | " + task.Project.PadRight(20) + " | " + task.Title.PadRight(20) + " | " + task.DueDate.ToString("yyyy-MM-dd").PadRight(12) + " | " + task.Status.ToString());
        }
    }

    // Search tasks by project or title
    public void SearchTasks(List<TodoItem> tasks)
    {
        Console.Write("Search Task: ");

        // Request user input
        string searchString = this.UserInput();

        if (!String.IsNullOrEmpty(searchString))
        {

            Console.WriteLine();
            Console.WriteLine("FOUND TASKS:");
            Console.WriteLine();

            var taskQuery = from task in tasks
                            where task.Project.ToLower().Contains(searchString.ToLower())
                            || task.Title.ToLower().Contains(searchString.ToLower()) orderby task.Project, task.DueDate
                            select task;

            foreach (TodoItem task in taskQuery)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(task.Id.ToString().PadRight(5) + " | " + task.Project.PadRight(20) + " | " + task.Title.PadRight(20) + " | " + task.DueDate.ToString("yyyy-MM-dd").PadRight(12) + " | " + task.Status.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }

    // Delete task by Id
    public void DeleteTask(List<TodoItem> tasks)
    {

        Console.Write("Delete Task: ");

        // Request user input
        string deleteString = this.UserInput();

        // Validate user input
        try
        {
            this.ValidateInput(deleteString, "id");
            if (tasks.Any(task => task.Id == Convert.ToInt32(deleteString)))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Task has been removed!");
                Console.ForegroundColor = ConsoleColor.Gray;
                tasks.RemoveAll(task => task.Id.Equals(Convert.ToInt32(deleteString)));
            }
            else throw new ArgumentException("Task does not exist!");

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    // Complete task
    public void CompleteTask(List<TodoItem> tasks)
    {

        Console.Write("Complete Task: ");

        // Request user input
        string completeString = this.UserInput();

        // Validate user input
        try
        {
            this.ValidateInput(completeString, "id");
            if (tasks.Any(task => task.Id == Convert.ToInt32(completeString)))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Task has been conpleted!");
                Console.ForegroundColor = ConsoleColor.Gray;
                try
                {
                    foreach (TodoItem task in tasks.Where(w => w.Id == Convert.ToInt32(completeString)))
                    {
                        task.Status = Status.Completed;
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            else throw new ArgumentException("Task does not exist!");

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    // Edit task by Id
    public void EditTask(List<TodoItem> tasks)
    {

        Console.Write("Edit Task: ");

        // Request user input
        string editString = this.UserInput();

        // Validate user input
        try
        {
            this.ValidateInput(editString, "id");
            if (tasks.Any(task => task.Id == Convert.ToInt32(editString)))
            {
                Console.Write("Enter new project (blank to keep existing): ");
                string newProject = this.UserInput();
                if (!String.IsNullOrEmpty(newProject))
                {
                    this.ValidateInput(newProject, "project");
                    try
                    {
                        foreach (TodoItem task in tasks.Where(w => w.Id == Convert.ToInt32(editString)))
                        {
                            task.Project = newProject;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }

                Console.Write("Enter new title (blank to keep existing): ");
                string newTitle = this.UserInput();
                if (!String.IsNullOrEmpty(newTitle))
                {
                    this.ValidateInput(newTitle, "title");
                    try
                    {
                        foreach (TodoItem task in tasks.Where(w => w.Id == Convert.ToInt32(editString)))
                        {
                            task.Title = newTitle;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                    
                Console.Write("Enter new due date (blank to keep existing): ");
                string userInput = this.UserInput();
                if (!String.IsNullOrEmpty(userInput))
                {
                    this.ValidateInput(userInput, "date");
                    try
                    {
                        DateTime newDueDate = Convert.ToDateTime(userInput);
                        foreach (TodoItem task in tasks.Where(w => w.Id == Convert.ToInt32(editString)))
                        {
                            task.DueDate = newDueDate;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }
            else throw new ArgumentException("Task does not exist!");

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    // Save tasks to json file
    public void SaveTasks(List<TodoItem> tasks)
    {
        // Determine whether the directory exists.
        if (!Directory.Exists("Data"))
        {
            DirectoryInfo di;
            try
            {
                // Try to create the directory.
                di = Directory.CreateDirectory("Data");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
        }
        StreamWriter sw = new StreamWriter("Data\\tasks.json");
        var jsonTaskList = JsonSerializer.Serialize(tasks);
        sw.Write(jsonTaskList.ToString());
        sw.Close();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Task list has been saved!");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    // Load tasks from json file
    public void LoadTasks(ref List<TodoItem> tasks)
    {
        if (File.Exists("Data\\tasks.json"))
        {
            try
            {
                string json = File.ReadAllText("Data\\tasks.json");
                if (!String.IsNullOrEmpty(json))
                {
                    var jsonTaskList = JsonSerializer.Deserialize<List<TodoItem>>(json);
                    if (jsonTaskList is not null)
                    {
                        tasks = jsonTaskList;
                        TodoItem.currentId = tasks.Max(a => a.Id);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Task list has been loaded!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }

    // Display menu and wait for key press
    public string TodoListMenu()
    {
        Console.WriteLine();
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. View Tasks");
        Console.WriteLine("3. Search Tasks");
        Console.WriteLine("4. Delete Task");
        Console.WriteLine("5. Edit Task");
        Console.WriteLine("6. Complete Task");
        Console.WriteLine("7. Exit");
        Console.WriteLine();

        ConsoleKeyInfo keypressed;
        string numKey;
        do
        {
            keypressed = Console.ReadKey(true);
            numKey = keypressed.Key.ToString().Trim('D');

        } while (!Regex.IsMatch(numKey, @"^[1-7]+\z"));
        return numKey;
    }
}




