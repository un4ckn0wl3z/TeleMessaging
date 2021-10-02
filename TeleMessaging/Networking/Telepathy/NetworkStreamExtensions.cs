using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TeleMessaging.Networking.Telepathy
{
    public static class NetworkStreamExtensions
    {
        public static int ReadSafely(this NetworkStream stream, byte[] buffer, int offset, int size)
        {
            try
            {
                return stream.Read(buffer, offset, size);
            }
            catch (IOException)
            {
                return 0;
            }
        }

        public static bool ReadExactly(this NetworkStream stream, byte[] buffer, int amount)
        {
            int bytesRead = 0;
            while (bytesRead < amount)
            {
                int remaining = amount - bytesRead;
                int result = stream.ReadSafely(buffer, bytesRead, remaining);

                if (result == 0)
                    return false;

                bytesRead += result;
            }

            return true;
        }
    }
}
