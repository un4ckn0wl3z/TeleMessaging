using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static TeleMessaging.Networking.Server;
using Message = TeleMessaging.Networking.Telepathy.Message;
using EventType = TeleMessaging.Networking.Telepathy.EventType;
namespace TeleMessaging
{
    class Program
    {
        private static Timer _timer = null;

        static void Main(string[] args)
        {
            StartServer();
            ServerController();
            StopServer();
            Console.WriteLine("Press any key to exit program.");
            Console.ReadKey();
        }

        private static void StartServer()
        {
            if (MainServer.Active) return;

            try
            {
                int port = 1609;
                MainServer.Start(port);
                Console.WriteLine("Server Starting at port: " + port);
                _timer = new Timer(GetDataLoop, null, 0, 2000);
                Thread.Sleep(3000);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        private static void StopServer()
        {
            if (MainServer.Active)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                MainServer.Stop();
                Console.WriteLine("Server Stopped");
            }
        }

        private static async void GetRecievedData()
        {
            Message Data;
            while (MainServer.GetNextMessage(out Data))
            {
                switch (Data.eventType)
                {
                    case EventType.Connected:
                        string ClientAddress = MainServer.GetClientAddress(Data.connectionId);
                        Console.WriteLine("connectionId: " + Data.connectionId);
                        Console.WriteLine("Client connected: " + ClientAddress);
                        break;
                    case EventType.Disconnected:
                        await Task.Delay(50);
                        MainServer.Disconnect(Data.connectionId);
                        break;
                    case EventType.Data:
                        HandleData(Data.connectionId, Data.data);
                        break;


                }
            }
        }

        private static void GetDataLoop(Object o)
        {
            //Console.WriteLine("GetDataLoop ticked");
            GetRecievedData();
        }

        private static void HandleData(int ConnectionId, byte[] RawData)
        {
            Console.WriteLine("ConnectionId: " + ConnectionId);
            byte[] ToProcess = RawData.Skip(1).ToArray();

            switch (RawData[0])
            {
                // Hello
                case 0:
                    Console.WriteLine("Incoming Hello data: ");
                    Console.WriteLine(Encoding.ASCII.GetString(ToProcess));

                    break;
                // Goodbye
                case 1:
                    Console.WriteLine("Incoming Goodbye data: ");
                    Console.WriteLine(Encoding.ASCII.GetString(ToProcess));
                    break;
            }


        }

        private static void ServerController()
        {
            bool runningController = true;

            while (runningController)
            {
                Console.WriteLine(@"
------------------------------------------------------
                TELE MESSAGING MENU
------------------------------------------------------
Choose your message to sending to client
1.Hello
2.GoodBye
3.Exit from controller
");
                string val;
                Console.Write("Enter choice: ");
                val = Console.ReadLine();

                int a = Convert.ToInt32(val);
                Console.WriteLine("Your input: {0}", a);

                switch (a)
                {
                    case 1:
                        MainServer.Send(1, Encoding.ASCII.GetBytes("Hello"));
                        break;
                    case 2:
                        MainServer.Send(1, Encoding.ASCII.GetBytes("GoodBye"));
                        break;
                    case 3:
                        runningController = false;
                        break;
                }

                //Thread.Sleep(2000);
            }



        }
    }
}
