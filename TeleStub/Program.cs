using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message = TeleStub.Networking.Telepathy.Message;
using static TeleStub.Networking.Client;
using EventType = TeleStub.Networking.Telepathy.EventType;
using DataType = TeleStub.Networking.DataType;
using System.Threading;

namespace TeleStub
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        private static void Start()
        {
            ConnectLoop();
        }

        private static void ConnectLoop()
        {
            Console.WriteLine("Inside ConnectLoop");
            while (!MainClient.Connected)
            {
                //await Task.Delay(50);
                Thread.Sleep(50);
                MainClient.Connect("127.0.0.1", 1609);
            }

            while (MainClient.Connected)
            {
                //await Task.Delay(2000);
                Thread.Sleep(2000);
                GetData();
            }

            ConnectLoop();
        }

        private static void GetData()
        {
            Message Data;
            while (MainClient.GetNextMessage(out Data))
            {
                switch (Data.eventType)
                {
                    case EventType.Connected:
                        Console.WriteLine("Connected");
                        List<byte> ToSend = new List<byte>();
                        ToSend.Add((int)DataType.Hello);
                        ToSend.AddRange(Encoding.ASCII.GetBytes("Hello Server, I Connected!!!"));
                        MainClient.Send(ToSend.ToArray());
                        ToSend.Clear();
                        break;
                    case EventType.Disconnected:
                        break;
                    case EventType.Data:
                        HandleData(Data.data);
                        break;

                }
            }

        }

        private  static void HandleData(byte[] RawData)
        {
            string StringForm = string.Empty;

            try
            {
                StringForm = Encoding.ASCII.GetString(RawData);
            }
            catch { }
            List<byte> ToSend = new List<byte>();

            switch (StringForm)
            {
                case "Hello":
                    ToSend.Add((int)DataType.Hello);
                    ToSend.AddRange(Encoding.ASCII.GetBytes("Hi Boss!!"));
                    MainClient.Send(ToSend.ToArray());
                    ToSend.Clear();
                    break;
                case "GoodBye":
                    ToSend.Add((int)DataType.GoodBye);
                    ToSend.AddRange(Encoding.ASCII.GetBytes("GoodBye Boss!"));
                    MainClient.Send(ToSend.ToArray());
                    ToSend.Clear();
                    break;
            }



        }
    }
}
