using ConsoleAppDemo;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace To_Do_List_CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TodoEntry> todoList = new List<TodoEntry>()
            {
                new TodoEntry("Sample Todo"),
                new TodoEntry("Due Todo", dueDate: DateTime.Now.AddDays(3))
            };

            Guid selectedId = new Guid();
            string message = string.Empty;

            while (true)
            {   
                Console.Clear();

                ShowTodoList(todoList, selectedId);

                Console.WriteLine("Type \"exit\" to quit");
                Console.Write(message);
                Console.Write("> ");
                var command = Console.ReadLine();

                if (command == "exit")
                {
                    break;
                }

                if (string.IsNullOrEmpty(command))
                {
                    continue;
                }

                Dictionary<string, string> commandSummary = commandSum(command);       

                switch (commandSummary["command"])
                {
                    case "unknown":
                        message = "Unknown command. Please try again.\n";
                        break;
                    case "create":
                        var newEntry = new TodoEntry(commandSummary["name"]);
                        todoList.Add(newEntry);
                        message = ($"Added \"{newEntry.Title}\" to Todo List.\n");
                        break;
                    case "select":
                        if (Guid.TryParse(commandSummary["name"], out Guid Id))
                        {
                            selectedId = Id;
                        } else
                        {
                            message = "Invalid Id. Please try again.\n";
                        }
                        break;
                }
            }

            static void ShowTodoList(List<TodoEntry> todoList, Guid Id)
            {
                Console.WriteLine("Todo List:");
                foreach (var todo in todoList)
                {
                    if (todo.Id == Id)
                    {
                        Console.WriteLine("* {0} | {1}", todo.Title, todo.Id);
                    } else
                    {
                        Console.WriteLine("- {0} | {1}", todo.Title, todo.Id);
                    }
                }
            }

            static Dictionary<string, string> commandSum(string command)
            {
                Dictionary<string, string> commandSummary = new Dictionary<string, string>();
                string[] commandArgs = command.Split(" ");

                switch (commandArgs[0])
                {
                    case "create":
                        commandSummary.Add("command", "create");
                        break;
                    case "list":
                        commandSummary.Add("command", "list");
                        break;
                    case "remove":
                        commandSummary.Add("command", "remove");
                        break;
                    case "filter":
                        commandSummary.Add("command", "filter");
                        break;
                    case "select":
                        commandSummary.Add("command", "select");
                        break;
                    default:
                        commandSummary.Add("command", "unknown");
                        return commandSummary;
                }

                string lastType = string.Empty;
                for (int i = 1; i <= commandArgs.Length - 1; i++)
                {
                    string arg = commandArgs[i];
                    // string nextArg = commandArgs[i + 1];
                    switch (arg)
                    {
                        case "-n":
                        case "--name":                       
                            lastType = "name";
                            break;
                        case "-desc":
                        case "--description":
                            lastType = "description";
                            break;
                        case "-due":
                        case "--duedate":
                            lastType = "dueDate";
                            break;
                        default:
                            if (!string.IsNullOrEmpty(lastType))
                            {
                                if (commandSummary.TryGetValue(lastType, out string? value))
                                {
                                    commandSummary[lastType] = value + " " + arg;
                                }
                                else
                                {
                                    
                                    commandSummary.Add(lastType, arg);
                                }
                            } else
                            {
                                commandSummary["command"] = "unknown";
                                return commandSummary;
                            }
                            break;
                    }
                }
                return commandSummary;
            }
        }
    }
}
