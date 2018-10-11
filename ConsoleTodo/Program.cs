using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleTodo
{
    class Program
    {
        public enum Action { Add, Do, Print, Quit }

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the console TODO application!");

            Todo todo = LoadOrCreateTodo();

            Console.WriteLine("Available options: Add, Do, Print, Quit");

            while (true)
            {
                string input = Console.ReadLine();
                string[] parameters = input.Split(' ');

                if (!Enum.TryParse(parameters[0], true, out Action action))
                {
                    Console.WriteLine("Not a valid input: " + parameters[0]);
                    Console.WriteLine("Valid inputs are Add, Do, Print, Quit");
                    continue;
                }

                switch (action)
                {
                    case Action.Add:
                        try
                        {
                            string[] task = input.Split('"');
                            todo.AddTodo(task[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Missing argument on action: add");
                            Console.WriteLine("Expecting example: Add \"Thing to do\"");
                        }
                        break;

                    case Action.Do:
                        try
                        {
                            todo.RemoveTodo(parameters[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Missing argument on action: do");
                            Console.WriteLine("Expecting example: Do #1");
                        }
                        break;

                    case Action.Print:
                        todo.PrintTodo();
                        break;

                    case Action.Quit:
                        Console.WriteLine("Quitting");
                        SaveTodoFile(todo);
                        break;

                    default:
                        Console.WriteLine("Action not implemented");
                        break;
                }

                if (action == Action.Quit)
                {
                    break;
                }
            }
        }

        private static Todo LoadOrCreateTodo()
        {
            Todo todo;
            if (CheckForTodoFile())
            {
                todo = LoadTodoFile();
                Console.WriteLine("Welcome back " + todo.Author);
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("Please input your name");
                    string name = Console.ReadLine();
                    if (name == "")
                    {
                        Console.WriteLine("Nothing detected. Please insert something");
                        continue;
                    }
                    todo = new Todo();
                    todo.Initialize(name);
                    break;
                }
            }

            return todo;
        }

        private static void SaveTodoFile(Todo todo)
        {
            WriteToBinaryFile<Todo>("savedata", todo);
        }

        private static Todo LoadTodoFile()
        {
            return ReadFromBinaryFile<Todo>("savedata");
        }

        private static bool CheckForTodoFile()
        {
            return File.Exists("savedata");
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite)
        {
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }

    [Serializable]
    class Todo
    {
        public string Author { get; private set; }
        int counter;
        SortedDictionary<int, string> todoList;

        public void Initialize(string author)
        {
            this.Author = author;
            this.counter = 1;
            this.todoList = new SortedDictionary<int, string>();
        }

        public void AddTodo(string task)
        {
            this.todoList.Add(this.counter, task);
            Console.WriteLine("Added new task: #" + this.counter.ToString() + " " + task);
            this.counter++;
        }

        public void RemoveTodo(string id)
        {
            int intId = int.Parse(id.Substring(1));
            if (todoList.ContainsKey(intId))
            {
                this.todoList.Remove(intId);
                Console.WriteLine("Task " + id + " removed!");
            } else
            {
                Console.WriteLine("Found no task with id " + id);
            }
        }

        public void PrintTodo()
        {
            foreach (KeyValuePair<int, string> task in this.todoList)
            {
                Console.WriteLine("#" + task.Key + " " + task.Value);
            }
        }
    }
}
