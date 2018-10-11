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
                Assert.AreEqual(expected, output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void TestDoTodo()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");
            todo.AddTodo("Something");
            string expected = "Task #1 done!";

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

        [TestMethod]
        public void TestDoubleDigitIdPrintSort()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");

            for (int i = 0; i <= 10; i++)
            {
                todo.AddTodo("x");
            }

            string expected = "#1 x\r\n#2 x\r\n#3 x\r\n#4 x\r\n#5 x\r\n#6 x\r\n#7 x\r\n#8 x\r\n#9 x\r\n#10 x\r\n#11 x";

            var currentConsoleOut = Console.Out;
            using (var output = new ConsoleOutput())
            {
                todo.PrintTodo();
                Assert.AreEqual(expected, output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void TestIdCounter()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");
            todo.AddTodo("1");
            todo.AddTodo("2");
            todo.RemoveTodo("#1");
            todo.AddTodo("3");

            string expected = "#2 2\r\n#3 3";

            var currentConsoleOut = Console.Out;
            using (var output = new ConsoleOutput())
            {
                todo.PrintTodo();
                Assert.AreEqual(expected, output.GetOutput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);

        }

        [TestMethod]
        public void TestSaveTodo()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");

            Program.WriteToBinaryFile<Todo>("testpath", todo);

            Assert.IsTrue(File.Exists("testpath"));

            File.Delete("testpath");

            Assert.IsFalse(File.Exists("testpath"));
        }

        [TestMethod]
        public void TestLoadTodo()
        {
            Todo todo = new Todo();
            todo.Initialize("Erlend");

            Program.WriteToBinaryFile<Todo>("testpath", todo);

            Todo loadedTodo = Program.ReadFromBinaryFile<Todo>("testpath");

            Assert.IsNotNull(loadedTodo);
            Assert.IsInstanceOfType(loadedTodo, typeof(Todo));
            Assert.AreEqual("Erlend", loadedTodo.Author);

            File.Delete("testpath");

            Assert.IsFalse(File.Exists("testpath"));
        }
    }
}
