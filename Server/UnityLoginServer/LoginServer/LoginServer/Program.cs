using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class LoginServer
{
    private static TcpListener listener;
    private static string csvPath = "account.csv";
    private static List<string[]> accounts = new List<string[]>(); 

    static void Main()
    {
        LoadAccounts();

        int port = 6000;
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"[서버 시작] 포트: {port}");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Thread thread = new Thread(() => HandleClient(client));
            thread.Start();
        }
    }

    static void LoadAccounts()
    {
        if (File.Exists(csvPath))
        {
            string[] lines = File.ReadAllLines(csvPath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 3)
                    accounts.Add(parts);
            }
            Console.WriteLine($"[로드 완료] 계정 수: {accounts.Count}");
        }
        else
        {
            Console.WriteLine("[알림] account.csv 없음. 새로 생성 예정.");
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"[받음] {msg}");

            if (msg.StartsWith("register:"))
            {
                string[] parts = msg.Substring(9).Split(',');
                string id = parts[0];
                string pw = parts[1];
                string nickname = parts[2];

                if (accounts.Exists(a => a[0] == id))
                {
                    Send(stream, "REGISTER_FAIL");
                }
                else
                {
                    string hashed = Hash(pw);
                    accounts.Add(new string[] { id, hashed, nickname });
                    File.AppendAllText(csvPath, $"{id},{hashed},{nickname}\n");
                    Send(stream, "REGISTER_SUCCESS");
                }
            }
            else if (msg.StartsWith("login:"))
            {
                string[] parts = msg.Substring(6).Split(',');
                string id = parts[0];
                string pw = parts[1];
                string hashed = Hash(pw);

                var account = accounts.Find(a => a[0] == id && a[1] == hashed);
                if (account != null)
                    Send(stream, $"LOGIN_SUCCESS:{account[2]}");
                else
                    Send(stream, "LOGIN_FAIL");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[오류] " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }

    static void Send(NetworkStream stream, string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        stream.Write(data, 0, data.Length);
    }

    static string Hash(string input)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
