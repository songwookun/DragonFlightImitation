using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class RankingServer
{
    private static TcpListener listener;
    private const int port = 5000;
    private static readonly string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ranking.csv");

    static void Main()
    {
        Console.WriteLine("[랭킹 서버 시작] 포트: " + port);
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Thread thread = new Thread(() => HandleClient(client));
            thread.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("[받음] " + message);

                if (message.StartsWith("submit:"))
                {
                    string data = message.Substring("submit:".Length);
                    SaveToCSV(data);
                }
            }
        }

        client.Close();
    }

    static void SaveToCSV(string data)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(csvPath, append: true, Encoding.UTF8))
            {
                writer.WriteLine(data);
                writer.Flush();
            }

            Console.WriteLine("[저장 완료] " + data);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[에러] CSV 저장 실패: " + ex.Message);
        }
    }
}