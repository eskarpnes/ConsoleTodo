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

                switch(action)
                {
                    case Action.Add:
                        try
                        {
                            addTodo(input);
                        } catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Missing argument on action: add");
                            Console.WriteLine("Expecting example: Add 'Thing to do'");
                        }
                        break;

                    case Action.Do:
                        try
                        {
                            doTodo(parameters[1]);
                        } catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Missing argument on action: do");
                            Console.WriteLine("Expecting example: Do #1");
                        }
                        break;

                    case Action.Print:
                        printTodo();
                        break;

                    case Action.Quit:
                        Console.WriteLine("Quitting");
                        quitTodo();
                        break;

                    default:
                        Console.WriteLine("Action not implemented");
                        break;
                }

                if(action == Action.Quit)
                {
                    break;
                }
            }
        }

        private static void quitTodo()
        {
            throw new NotImplementedException();
        }

        private static void printTodo()
        {
            throw new NotImplementedException();
        }

        private static void doTodo(string v)
        {
            throw new NotImplementedException();
        }

        private static void addTodo(string v)
        {
            throw new NotImplementedException();
        }
    }

    class Todo
    {
        string author;
        int counter;
        Dictionary<string, string> todoList;

        Todo(string author)
        {
            this.author = author;
            this.counter = 1;
            this.todoList = new Dictionary<string, string>();
        }

    }
}
