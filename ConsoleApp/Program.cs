using System;
using System.IO;
using System.IO.Pipes;

namespace ConsoleApp
{
    class Program
    {
        private const string PipeName = "DateTimePipe";
        private const string ServerName = ".";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine($"创建管道客户端 ...");
            using var clientStream = new NamedPipeClientStream(ServerName, PipeName, PipeDirection.InOut);
            clientStream.Connect();

            Console.WriteLine($"管道客户端连接成功");
            using var reader = new StreamReader(clientStream);
            using var writer = new StreamWriter(clientStream) { AutoFlush = true };

            Console.WriteLine("请输入请求：");
            string request = string.Empty;
            while (!string.IsNullOrEmpty(request = Console.ReadLine()))
            {
                try
                {
                    writer.WriteLine(request);
                    Console.WriteLine("正在等待响应 ...");
                    string response = reader.ReadLine();
                    Console.WriteLine($"响应：{response}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"请求失败：{ex.Message}");
                    clientStream.Dispose();
                    break;
                }
            }

            Console.Read();
        }
    }
}
