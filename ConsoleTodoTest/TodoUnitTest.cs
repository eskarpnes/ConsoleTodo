using System;
using System.IO;
using ConsoleTodo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTodoTest
{

    public class ConsoleOutput : IDisposable
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        public ConsoleOutput()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string GetOutput()
        {
            return stringWriter.ToString().Trim();
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }

    [TestClass]
    public class TodoUnitTest
    {
        [TestMethod]
        public void TestAddTodo()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");
            string expected = "Added new task: #1 Make unit test";

            var currentConsoleOut = Console.Out;
            using (var output = new ConsoleOutput())
            {
                todo.AddTodo("Make unit test");
                Assert.AreEqual(expected.Trim(), output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void TestDoTodo()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");
            todo.AddTodo("Something");
            string expected = "Task #1 removed!";

            var currentConsoleOut = Console.Out;
            using (var output = new ConsoleOutput())
            {
                todo.RemoveTodo("#1");
                Assert.AreEqual(expected, output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void TestDoTodoInvalidKey()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");
            string expected = "Found no task with id #3";

            var currentConsoleOut = Console.Out;
            using (var output = new ConsoleOutput())
            {
                todo.RemoveTodo("#3");
                Assert.AreEqual(expected, output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void TestPrintOneTodo()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");
            todo.AddTodo("Something");
            string expected = "#1 Something";

            var currentConsoleOut = Console.Out;
            using (var output = new ConsoleOutput())
            {
                todo.PrintTodo();
                Assert.AreEqual(expected, output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void TestPrintMultipleTodo()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");
            todo.AddTodo("Something");
            todo.AddTodo("Something else");

            string expected = "#1 Something\r\n#2 Something else";

            var currentConsoleOut = Console.Out;
            using (var output = new ConsoleOutput())
            {
                todo.PrintTodo();
                Assert.AreEqual(expected, output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

    }
}
