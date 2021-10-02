using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleStub.Networking.Telepathy
{
    public static class Logger
    {
        public static Action<string> Log = Console.WriteLine;
        public static Action<string> LogWarning = Console.WriteLine;
        public static Action<string> LogError = Console.Error.WriteLine;
    }
}
