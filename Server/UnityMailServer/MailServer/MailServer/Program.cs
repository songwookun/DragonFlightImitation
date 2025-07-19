using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;


class MailServer
{
    private static TcpListener listener;
    private static List<TcpClient> clients = new List<TcpClient>();
    private static List<(string title, string reward)> mailData = new List<(string, string)>();

    static void Main()
    {
        LoadMailDataFromCSV("mail_data.csv");  
        int port = 7778;
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"[우편 서버 시작] 포트: {port}");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("[클라이언트 우편 접속]");
            new Thread(() => HandleClient(client)).Start();
        }
    }

    static void LoadMailDataFromCSV(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"[경고] {path} 파일이 없습니다. 기본값 사용.");
            mailData.Add(("출석 보상", "코인 100개"));
            mailData.Add(("점검 보상", "젬 10개"));
            mailData.Add(("사전 예약", "포션 5개"));
            return;
        }

        var lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            var split = lines[i].Split(',');
            if (split.Length >= 2)
                mailData.Add((split[0], split[1]));
        }

        Console.WriteLine($"[CSV 우편 로드 완료] {mailData.Count}개");
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
                Console.WriteLine("[우편 수신] " + msg);

                if (msg.StartsWith("MAIL_REQ"))
                {
                    string response = "MAIL:";
                    for (int i = 0; i < mailData.Count; i++)
                    {
                        var m = mailData[i];
                        response += $"{m.title}|{m.reward}";
                        if (i < mailData.Count - 1)
                            response += ";";
                    }
                    Send(client, response);
                }
                else if (msg.StartsWith("MAIL_RECV:"))
                {
                    string reward = msg.Substring("MAIL_RECV:".Length);
                    Console.WriteLine($"[수령 완료] 보상: {reward}");
                }
            }
        }
        catch { }
        finally
        {
            clients.Remove(client);
            client.Close();
            Console.WriteLine("[우편 클라이언트 연결 종료]");
        }
    }

    static void Send(TcpClient client, string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        client.GetStream().Write(data, 0, data.Length);
    }
}
