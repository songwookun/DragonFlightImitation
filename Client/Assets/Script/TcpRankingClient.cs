using UnityEngine;
using System.Net.Sockets;
using System.Text;

public static class TcpRankingClient
{
    public static void SendScore(string name, int score)
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000); 
            string message = $"submit:{name},{score}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.GetStream().Write(data, 0, data.Length);
            client.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("서버 전송 실패: " + e.Message);
        }
    }
}
