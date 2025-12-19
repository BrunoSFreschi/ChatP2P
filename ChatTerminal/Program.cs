
using System.Net;
using System.Net.Sockets;

Console.WriteLine("=== Chat via terminal ===");
Console.WriteLine("=== 1- Criar sala ===");
Console.WriteLine("=== 2 - Conectar ===");
Console.WriteLine("=== Escolha ===");

string choice = Console.ReadLine()!;

if (choice == "1")
{
    StartServer();
}
else if (choice == "2")
{
    StartClient();
}
else
{
    Console.WriteLine("=== Escolha inválida ===");
}


// Placeholder for starting the client
void StartServer()
{
    string localIP = GetLocalIP();
    Console.WriteLine($"=== Seu IP é: {localIP} ===");


    TcpListener server = new(System.Net.IPAddress.Any, 5000);

    server.Start();
    Console.WriteLine("=== Sala criada. Aguardando conexões na porta 5000... ===");

    TcpClient client = server.AcceptTcpClient();
    Console.WriteLine("=== Cliente conectado! ===");

    NetworkStream stream = client.GetStream();

    Thread threadReceive = new(() => ReceiverMessage(stream));

    threadReceive.Start();

    SendMessage(stream);

    client.Close();
    server.Stop();
}

static string GetLocalIP()
{
    try
    {
        string hostName = Dns.GetHostName();
        IPAddress[] adress = Dns.GetHostAddresses(hostName);

        foreach (IPAddress ip in adress)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork
                && !IPAddress.IsLoopback(ip))
            {
                return ip.ToString();
            }
        }

        return "127.0.0.1";
    }
    catch
    {
        return "Not found IP";
    }
}

void StartClient()
{
    Console.WriteLine("=== Digite o endereço IP do servidor: ");

    string ipAddress = Console.ReadLine()!.Trim();

    if (string.IsNullOrWhiteSpace(ipAddress) || ipAddress.ToLower() == "localhost")
        ipAddress = "127.0.0.1";
    try
    {
        TcpClient client = new(ipAddress, 5000);

        Console.WriteLine("=== Conectado ao servidor! ===\n");

        NetworkStream stream = client.GetStream();

        Thread threadReceive = new(() => ReceiverMessage(stream));
        threadReceive.Start();

        SendMessage(stream);

        client.Close();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

void ReceiverMessage(NetworkStream stream)
{
    byte[] buffer = new byte[1024];

    try
    {
        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0)
                break;


            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"\n Amigo: {message}");
            Console.WriteLine("Você: ");
        }
    }
    catch
    {
        Console.WriteLine("\n Close connect.");
    }
}

void SendMessage(NetworkStream stream)
{
    try
    {
        while (true)
        {
            Console.Write("Você: ");
            string message = Console.ReadLine()!;

            if (message.ToLower() == "sair")
                break;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}