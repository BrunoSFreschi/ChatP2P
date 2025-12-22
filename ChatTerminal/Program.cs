using System.Net;
using System.Net.Sockets;

Console.WriteLine("1. Criar sala ");
Console.WriteLine("2. Conectar ");


string choice = Console.ReadLine()!.Trim();

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


void StartServer()
{
    string localIP = GetLocalIP();
    Console.WriteLine($"Seu IP é: {localIP}");


    TcpListener server = new(System.Net.IPAddress.Any, 5000);

    server.Start();
    Console.WriteLine("Sala criada. Aguardando conexões...");

    TcpClient client = server.AcceptTcpClient();
    Console.WriteLine("Cliente conectado!");

    NetworkStream stream = client.GetStream();

    Thread threadReceive = new(() => ReceiverMessage(stream));

    threadReceive.Start();

    SendMessage(stream);

    client.Close();
    server.Stop();
}

void StartClient()
{
    Console.WriteLine("Digite o endereço IP do servidor: ");

    string ipAddress = Console.ReadLine()!.Trim();

    if (string.IsNullOrWhiteSpace(ipAddress) || ipAddress.Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
        ipAddress = "127.0.0.1";
    try
    {
        TcpClient client = new(ipAddress, 5000);

        Console.WriteLine("Conectado!");

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

            // Move o cursor para o início da linha e limpa
            Console.SetCursorPosition(0, Console.CursorTop - 1);

            string timestamp = DateTime.Now.ToString("t");
            string fullMessage = $"Você: {message} - {timestamp}";
            Console.WriteLine(fullMessage);

            message = message + " - " + timestamp;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
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