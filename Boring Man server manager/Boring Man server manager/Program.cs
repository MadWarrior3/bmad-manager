using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Threading;

namespace Boring_Man_server_manager
{
    class Program
    {
        static int Main(string[] args)
        {
            TerminalUserInterface TUI = new TerminalUserInterface();

            do
            {
                TUI.CLI.readInputLine();
            }
            while (true);

            Environment.Exit(69420);
        }
    }

    class TerminalUserInterface
    {
        public CommandLineInterface CLI;

        public TerminalUserInterface()
        {
            CLI = new CommandLineInterface();
        }

        public class CommandLineInterface
        {
            List<string> arguments = new List<string>();
            string currentCommand = "";

            public CommandLineInterface()
            {

            }

            public void readInputLine()
            {
                // temp vars
                ConsoleKeyInfo k;
                char c;

                bool isInAutocomplete = false;
                string autocompleteString = "";

                // read line
                do
                {
                    k = Console.ReadKey(true);
                    c = k.KeyChar;

                    if (isInAutocomplete)
                    {
                        if (c == 'a')
                        {
                            Console.Write(autocompleteString.Length);
                        }

                        if (k.Key == ConsoleKey.Spacebar)
                        {
                            isInAutocomplete = false;
                            foreach (char temp in autocompleteString)
                            {
                                Console.Write("\b \b");
                            }

                            currentCommand += autocompleteString + ' ';
                            Console.Write(autocompleteString + ' ');
                        }
                        else if (k.Key == ConsoleKey.Enter)
                        {
                            isInAutocomplete = false;
                            foreach (char temp in autocompleteString)
                            {
                                Console.Write("\b \b");
                            }

                            currentCommand += autocompleteString;
                            Console.Write(autocompleteString);
                        }
                        else if (!Char.IsControl(c))
                        {
                            isInAutocomplete = false;
                            foreach (char temp in autocompleteString)
                            {
                                Console.Write("\b \b");
                            }

                            currentCommand += c;
                            Console.Write(c);
                        }
                        else if (k.Key == ConsoleKey.Backspace)
                        {
                            isInAutocomplete = false;
                            foreach (char temp in autocompleteString)
                            {
                                Console.Write("\b \b");
                            }
                        }
                    }
                    else
                    {
                        autocompleteString = "";

                        if (!Char.IsControl(c))
                        {
                            currentCommand += c;
                            Console.Write(c);
                        }
                        else if (k.Key == ConsoleKey.Backspace)
                        {
                            if (currentCommand.Length > 0)
                            {
                                currentCommand = currentCommand.Remove(currentCommand.Length - 1);
                                Console.Write("\b \b");
                            }
                        }
                        else if (k.Key == ConsoleKey.Tab)
                        {
                            isInAutocomplete = true;
                            autocompleteString = autocomplete((k.Modifiers | ConsoleModifiers.Shift) != 0);
                            Console.Write(Color.Autocomplete + autocompleteString + Color.Default); // test
                        }
                    }

                    if (k.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("iiiight bai <3");
                        return;
                    }
                }
                while (c != '\n' && c != '\r'/* && c!= ' '*/);
                Console.Write('\n');

                // interpret line
                string[] args = currentCommand.Split(' ');
                switch (args[0])
                {
                    case "echo": // this is a test command
                        for (int i=1; i<args.Length; i++)
                            Console.Write(Color.Message + args[i] + Color.Default + ' ');
                        Console.Write('\n');
                        break;

                    case "smol":
                        Console.Write("\n      " + Color.Underlined + "o" + Color.Default + "\n      ^\n");
                            break;

                    default:
                        Console.WriteLine(Color.Error + "Please enter a valid command" + Color.Default);
                        break;
                }

                // reset command
                currentCommand = "";

                // linefeed before next line/instructions
                Console.Write("\n");
            }

            private string autocomplete(bool backwards = false) // select next possibility by default
            {
                return "fart";
            }
        };

        private class CommandSyntax
        {
            public enum InputType
            {
                Command,
                Text,
                Coordinate,
                Player,
                Color
            }
            
            public static readonly List<string> Commands;
            
            public List<InputType> Fields;
        };

        private class Argument
        {
            Argument(string display_name, ArgumentType argument_type)
            {
                name = display_name;
                type = argument_type;
            }

            public readonly string name;
            public readonly ArgumentType type;
        };

        private class ArgumentType
        {
            ArgumentType(List<string> explicit_names, CommandSyntax.InputType input_type, List<string> auto_complete_options)
            {
                names = explicit_names;
                inputType = input_type;
                autoCompleteOptions = auto_complete_options;
            }

            public readonly List<string> names;
            public readonly CommandSyntax.InputType inputType;
            public List<string> autoCompleteOptions;
        };

        //private abstract class Context// mostly here for autocomplete
        //{             // this is the context corresponding to the current input i.e. if you're entering text for a message command, the context is "Text" not "Message"
        //    public readonly List<Context> nextContexts; // list of possible following contexts

        //    public readonly string contextModifier; // allows user to specify a context when multiple are available
        //    public readonly List<string> autocompleteOptions; // list of possible autocompletions within this context

        //    public Context(List<Context> next_contexts, string context_modifier, List<string> autocomplete_options)
        //    {
        //        nextContexts = next_contexts;
        //        contextModifier = context_modifier;
        //        autocompleteOptions = autocomplete_options;
        //    }
        //}

        //private class CommandContext : Context
        //{
        //    public new readonly List<Context> nextContexts; // list of possible following contexts

        //    public new readonly string contextModifier; // allows user to specify a context when multiple are available
        //    public new readonly List<string> autocompleteOptions; // list of possible autocompletions within this context
        //    public CommandContext()
        //        : base(new List<Context>() { new TextContext(null) },
        //               "",
        //               new List<string>() { "echo", "smol" } // TODO : make separate variable/class to store these ?
        //               ) { }
        //};

        //private class TextContext : Context
        //{
        //    public new readonly List<Context> nextContexts; // list of possible following contexts

        //    public new readonly string contextModifier; // allows user to specify a context when multiple are available
        //    public new readonly List<string> autocompleteOptions; // list of possible autocompletions within this context
        //    public TextContext(List<Context> next_contexts)
        //        : base(next_contexts,
        //               "",
        //               new List<string>() { "echo", "smol" } // TODO : make separate variable/class to store these ?
        //        ) { }
        //}

        public static class Color
        {
            // special
            public static string Default = "\u001b[0m"; // resets all
            public static string Underlined = "\u001b[4m";

            // text colors
            public static string Error = "\u001b[38;2;255;0;0m";
            public static string Message = "\u001b[38;2;200;0;180m";

            // background colors
            public static string Autocomplete = "\u001b[48;2;0;0;255m";
        }
    }
}

