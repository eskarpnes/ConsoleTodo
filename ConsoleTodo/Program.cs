using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleTodo
{
    class Program
    {
        public enum Action { Add, Do, Print, Quit, Delete }

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the console TODO application!");

            Todo todo = LoadOrCreateTodo();

            while (true)
            {
                string input = Console.ReadLine();
                string[] parameters = input.Split(' ');

                if (!Enum.TryParse(parameters[0], true, out Action action))
                {
                    Console.WriteLine("Not a valid input: " + parameters[0]);
                    Console.WriteLine("Valid inputs are Add, Do, Print, Quit, Delete");
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

                    case Action.Delete:
                        Console.WriteLine("Deleting saved todo and creating a new one.");
                        todo = ResetTodo();
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

        private static Todo ResetTodo()
        {
            if (CheckForTodoFile())
            {
                File.Delete("savedata");
            }
            Todo todo = CreateNewTodo();
            return todo;
        }

        private static Todo LoadOrCreateTodo()
        {
            Todo todo;
            if (CheckForTodoFile())
            {
                todo = LoadTodoFile();
                Console.WriteLine("Welcome back " + todo.Author);
                Console.WriteLine("Available options: Add, Do, Print, Quit, Delete");
            }
            else
            {
                todo = CreateNewTodo();
            }

            return todo;
        }

        private static Todo CreateNewTodo()
        {
            Todo todo;
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
            Console.WriteLine("Available options: Add, Do, Print, Quit, Delete");
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
                Console.WriteLine("Task " + id + " done!");
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
