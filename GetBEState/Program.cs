using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Timer = System.Timers.Timer;

Console.Title = Assembly.GetEntryAssembly().GetName().Name;
string[] guid = Guid.NewGuid().ToString().ToUpper().Split('-');
string str = $"01234567890123456700FFFF00FEFEFEFEFDFDFDFD12345678{guid[2]}{guid[4]}";
byte[] str1 = new byte[str.Length / 2];
for (int i = 0; i < str.Length; i += 2)
{
    str1[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
}
Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
Console.Write("请输入服务器地址：");
string ip = Console.ReadLine();
Console.Write("请输入服务器端口：");
int port = Convert.ToInt32(Console.ReadLine());
Timer timer = new()
{
    Interval = 1000
};
timer.Elapsed += (sender, e) =>
{
    Console.Clear();
    byte[] back = new byte[255];
    Console.WriteLine($"服务器{ip}:{port}的信息：");
    try
    {
        if (!socket.Connected)
        {
            socket.Connect(ip, port);
        }
        socket.Send(str1);
        socket.Receive(back);
        string[] data = Encoding.UTF8.GetString(back).Split(';');
        // Console.WriteLine($"头：{data[0]}");
        Console.WriteLine($"服务器信息：{data[1]}");
        Console.WriteLine($"协议版本：{data[2]}");
        Console.WriteLine($"游戏版本：{data[3]}");
        Console.WriteLine($"玩家数：{data[4]}/{data[5]}");
        // Console.WriteLine($"ID：{data[6]}");
        Console.WriteLine($"存档名：{data[7]}");
        Console.WriteLine($"默认游戏模式：{data[8]}");
        Console.WriteLine($"是否可见：{data[9]}");
        Console.WriteLine($"IPv4端口：{data[10]}");
        Console.WriteLine($"IPv6端口：{data[11]}");
    }
    catch (IndexOutOfRangeException) { }
    catch (Exception ex)
    {
        Console.WriteLine($"服务器不在线，正在等待上线：{ex.Message}");
        socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }
};
timer.Start();
Console.ReadKey();
