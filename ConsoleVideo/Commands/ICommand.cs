using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVideo.Commands
{
    public interface ICommand
    {
        public string Name { get; set; }

        bool Run();
    }
}
