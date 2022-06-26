using ConsoleVideo.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVideo
{
    internal class CommandManager
    {
        private static List<ICommand> _commands = new();

        public static void Setup()
        {
            IEnumerable<Type> commandTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(t => typeof(ICommand).IsAssignableFrom(t));

            foreach (var type in commandTypes)
            {
                if (type.Name != "ICommand")
                {
                    object? commandObjectInstance = Activator.CreateInstance(type);
                    if (commandObjectInstance is ICommand command)
                    {
                        _commands.Add(command);
                    }

                }

            }


        }

        public static bool Send(string commandString)
        {
            commandString = commandString.ToLower();

            ICommand? command = _commands.FirstOrDefault(command => command?.Name == commandString, null);

            if (command != null)
            {
                command.Run();
                return true;
            }

            return false;
        }
    }
}
