using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Utils.Common {  

public class ConsoleShell {

        private static IDictionary<string, Tuple<Action<string>, string>> _commands =
            new ConcurrentDictionary<string, Tuple<Action<string>, string>>();

        public void Register(string command, string help, Action<string> action) {            
            _commands.Add(command, new Tuple<Action<string>, string>(action, help));
        }

        public void Execute(string command) {
            if (string.IsNullOrWhiteSpace(command)) {
                Console.WriteLine("Bad request");
                return;
            }

            if (command == "help" || command == "?") {
                Help();
                return;
            }
            if (command == "cls" || command == "clear") {
                Console.Clear();
                return;
            }

            var parts = command.Split(' ');
            var cmd = parts[0];

            var found = _commands.Keys.FirstOrDefault(l => {
                var segments = l.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                return segments.Length == 0 ? l == cmd : segments.Any(segment => segment == cmd);
            });

            if (found == null) {                 
                Console.WriteLine("Bad request");
                return;
            }
            
            var action = _commands[found];
            action.Item1.Invoke(string.Join(" ", parts.Skip(1)));
        }

        public void Help() {
            Console.WriteLine();
            WriteCommandHelp("cls|clear", "to clear the console");
            WriteCommandHelp("help|?", "to show this help");
            foreach (var command in _commands) {
                WriteCommandHelp(command.Key, command.Value.Item2);
            }
        }

        private void WriteCommandHelp(string command, string help) {
            Console.WriteLine("    {0,-10}\t{1}", command, help);
        }

        public void Prompt() {
            Help();
            while (true) {
                var cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd)) {
                    return;
                }
                Execute(cmd);
            }
        }

    }
}
