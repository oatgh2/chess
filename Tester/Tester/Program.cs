using System.Net.Sockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;

TcpClient client = new TcpClient();


client.Connect(IPAddress.Parse("127.0.0.1"), 4194);

Thread thRead = new Thread(() =>
{
  NetworkStream stream = client.GetStream();
  while (client.Connected)
  {
    byte[] decodedMessage = new byte[4096];
    int readedBytes = stream.Read(decodedMessage, 0, decodedMessage.Length);
    string response = Encoding.UTF8.GetString(decodedMessage, 0, readedBytes);
    ConnectionCommunication connectionCommunication = JsonConvert.DeserializeObject<ConnectionCommunication>(response);
    Console.WriteLine(response);
    if (connectionCommunication.TypeEnum == ConnectionMessageType.Disconnecting)
      break;
  }
});

thRead.Start();

Thread thWrite = new Thread(() =>
{
  NetworkStream stream = client.GetStream();
  Console.Clear();
  Console.WriteLine("Digite algo:");
  ConsoleKeyInfo keyInfo;
  do
  {
    keyInfo = Console.ReadKey(true);
    char keyPressed = keyInfo.KeyChar;
    byte[] encodedMessage = Encoding.UTF8.GetBytes(keyPressed.ToString());
    stream.Write(encodedMessage, 0, encodedMessage.Length);
  }
  while (keyInfo.Key != ConsoleKey.Escape);
});

thWrite.Start();


class ConnectionCommunication
{
  public ConnectionCommunication(ConnectionMessageType type, string message, string value = "")
  {
    Type = (int)type;
    Message = message;
    Value = value;
  }
  public ConnectionMessageType TypeEnum
  {
    get
    {
      return (ConnectionMessageType)Type;
    }
  }
  public int Type { get; set; }
  public string Message { get; set; }
  public string Value { get; set; }
  public override string ToString()
  {
    string result = JsonConvert.SerializeObject(this);
    return result;
  }
}


public enum ConnectionMessageType
{
  Unknown = 0,
  HandShake = 1,
  Message = 2,
  Disconnecting = 3,
  CreateNewSession = 5,
  DeleteSession = 6,
}