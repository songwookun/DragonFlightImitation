using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatServer
{
    private static TcpListener listener;
    private static List<TcpClient> clients = new List<TcpClient>();

    static void Main()
    {
        int port = 7777;
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"[채팅 서버 시작] 포트: {port}");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("[클라이언트 채팅 접속]");

            Thread thread = new Thread(() => HandleClient(client));
            thread.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0) break;

                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("[채팅 수신] " + msg);

                if (msg.StartsWith("클라:"))
                {
                    Broadcast(msg, client);
                }
            }
        }
        catch { }
        finally
        {
            clients.Remove(client);
            client.Close();
            Console.WriteLine("[채팅 클라이언트 연결 종료]");
        }
    }

    static void Broadcast(string msg, TcpClient sender)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        foreach (var client in clients)
        {
            if (client != sender)
            {
                try { client.GetStream().Write(data, 0, data.Length); }
                catch { }
            }
        }
    }
}
