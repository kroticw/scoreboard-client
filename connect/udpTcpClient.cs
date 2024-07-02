using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using scoreboard_client;

namespace connect;

public class UdpTcpClient
{
    private UdpClient udpClient;
    private TcpClient tcpClient;
    private TcpListener tcpListener;
    public NetworkStream tcpStream;
    private int tcpPort = 8888; // TCP порт для прослушивания
    private int udpPort = 8889; // UDP порт для broadcast
    Client client = new Client();

    public UdpTcpClient()
    {
        udpClient = new UdpClient();
        tcpListener = new TcpListener(IPAddress.Any, tcpPort);
        tcpListener.Start();
    }

    public async Task StartAsync()
    {
        Task broadcastTask = BroadcastConnectionInfo();
        Task<TcpClient> acceptTask = tcpListener.AcceptTcpClientAsync();

        await Task.WhenAny(broadcastTask, acceptTask);

        if (acceptTask.IsCompleted)
        {
            tcpClient = acceptTask.Result;
            udpClient.Close(); // Закрытие UDP порта после установления TCP соединения
            Console.WriteLine("TCP connection established.");
            tcpStream = tcpClient.GetStream();
            await HandleTcpConnectionAsync();
        }
    }

    private async Task BroadcastConnectionInfo()
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.88.255"), udpPort);
        udpClient.EnableBroadcast = true;
        Console.WriteLine("111");
        
        string host = Dns.GetHostName();
        IPHostEntry ip = Dns.GetHostEntry(host);
        string IP="";
        foreach (IPAddress ip1 in ip.AddressList){
            if (ip1.ToString().Contains("192.168.88.249")){
                IP = ip1.ToString();
            }
        }
        
        client.ClientIp = IP;
        client.ClientPort = tcpPort.ToString();
        string json = System.Text.Json.JsonSerializer.Serialize(client);
        Console.WriteLine(json);
        byte[] data = Encoding.UTF8.GetBytes(json);

        while (!tcpListener.Pending())
        {
            udpClient.Send(data, data.Length, ipEndPoint);
            Console.WriteLine("Broadcasting connection info...");
            await Task.Delay(1000); // Пауза в 1 секунду между отправками
        }
    }

    private void ParseNewEvent(ClientServerMessage receivedMessage) {
        MainWindow.Instance.SetAllLabels(receivedMessage);
    }

    private async Task HandleTcpConnectionAsync()
    {
        //Console.WriteLine("HandleTcpConnectionAsync");
        try
        {
            while(true) {
                byte[] size = new byte[4];
                int n = tcpStream.Read(size); //считываем длину сообщения
                if (n > 0) {
                    if (n == 1) continue;
                    Console.WriteLine($"n {n}"); 
                    int sizeInInt = BitConverter.ToInt32(size, 0);
                    Console.WriteLine($"sizeInInt: {sizeInInt}");
                    byte[] buffer = new byte[sizeInInt];
                    
                    n = await tcpStream.ReadAsync(buffer); //считываем сообщение
                    if (buffer[0] == (byte)0) continue;
                    
                    Console.WriteLine($"next n {n}");
                    int newInt = Array.IndexOf(buffer, (byte)0);
                    Console.WriteLine($"newInt: {newInt}");
                    
                    if (newInt != -1)
                        Array.Resize(ref buffer, newInt);

                    string receivedMessage = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine(receivedMessage);
                    try {
                        ClientServerMessage? message = JsonConvert.DeserializeObject<ClientServerMessage>(receivedMessage);
                        Console.WriteLine(message.CommandOne.Name);
                        if (message != null) ParseNewEvent(message);
                    } catch (Exception ex) {
                        continue;
                    }
                }
                await Task.Delay(100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TCP error: {ex.Message}");
        }
        finally
        {
            RestartConnection();
        }
    }

    private async void RestartConnection()
    {
        tcpClient.Close();
        tcpListener.Stop();
        Console.WriteLine("TCP connection lost. Restarting UDP broadcast...");
        await StartAsync(); // Перезапуск процесса для нового соединения
    }

}