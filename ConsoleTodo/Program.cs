using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Action { Add, Do, Print, Quit }

namespace ConsoleTodo
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the console TODO application!");

            Todo todo = loadOrCreateTodo();

            Console.WriteLine("Available options: Add, Do, Print, Quit");

            while (true)
            {
                string input = Console.ReadLine();
                string[] parameters = input.Split(' ');

                if (!Enum.TryParse(parameters[0], out Action action))
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
                            todo.addTodo(task[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Missing argument on action: add");
                            Console.WriteLine("Expecting example: Add 'Thing to do'");
                        }
                        break;

                    case Action.Do:
                        try
                        {
                            todo.removeTodo(parameters[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Missing argument on action: do");
                            Console.WriteLine("Expecting example: Do #1");
                        }
                        break;

                    case Action.Print:
                        todo.printTodo();
                        break;

                    case Action.Quit:
                        Console.WriteLine("Quitting");
                        saveTodoFile(todo);
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

        private static Todo loadOrCreateTodo()
        {
            Todo todo;
            if (checkForTodoFile())
            {
                todo = loadTodoFile();
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
                    todo = new Todo(name);
                    break;
                }
            }

            return todo;
        }

        private static void saveTodoFile(object todo)
        {
            throw new NotImplementedException();
        }

        private static Todo loadTodoFile()
        {
            throw new NotImplementedException();
        }

        private static bool checkForTodoFile()
        {
            return false;
        }
    }

    class Todo
    {
        string author;
        int counter;
        Dictionary<string, string> todoList;

        public Todo(string author)
        {
            this.author = author;
            this.counter = 1;
            this.todoList = new Dictionary<string, string>();
        }

        public void addTodo(string task)
        {
            string id = "#" + this.counter.ToString();
            this.counter++;
            this.todoList.Add(id, task);
        }

        public void removeTodo(string id)
        {
            this.todoList.Remove(id);
        }

        public void printTodo()
        {
            foreach (KeyValuePair<string, string> task in this.todoList)
            {
                Console.WriteLine(task.Key + " " + task.Value);
            }
        }
    }
}
