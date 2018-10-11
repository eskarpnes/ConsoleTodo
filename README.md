# ConsoleTodo

A CLI application for handling a TODO-list!

Possible actions:

+ Add "Task description"

Adds a new todo-object with an increasing id-counter.

+ Do #id

Marks a todo-object as done (removes it). Id is an increasing counter, so the counting persist even if you remove every object.

+ Print

Pretty-prints every current todo-object.

+ Delete

Deletes the saved todo-list and creates a new one.

+ Quit

Saves the todo-list as a binary file called "savedata" in the same folder as the program and quits the program.

# Testing

Unit-testing is done on every expected use-case of the program. There are some inner functions that is not tested as testing them would require changing the logic/encapsulation.
