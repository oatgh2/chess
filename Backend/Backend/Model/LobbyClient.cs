using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model
{
  public class LobbyClient
  {
    public Guid Id { get; set; }
    public string? Alias { get; set; }

    public TcpClient? Client { get; set; }

    public Thread? ReadThread;

    public bool Disconnect()
    {
      ConnectionCommunication connectionCommunication = new ConnectionCommunication(ConnectionMessageType.Disconnecting, "Disconnecting, bye bye");
      Write(connectionCommunication.ToString());
      Client!.Dispose();
      return true;
    }

    private void read(NetworkStream ns)
    {
      try
      {
        while (true)
        {
          byte[] buffer = new byte[4096];
          int readedBuffer = ns.Read(buffer, 0, buffer.Length);
          string readedString = Encoding.UTF8.GetString(buffer, 0, readedBuffer);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public void Write(string toWrite)
    {
      NetworkStream ns = Client!.GetStream();
      byte[] buffer = Encoding.UTF8.GetBytes(toWrite);
      ns.Write(buffer, 0, buffer.Length);
    }

    public void InitializeClient() { 
      NetworkStream ns = Client!.GetStream();
      ReadThread = new Thread(() => read(ns));
      ReadThread.Start();
    }
  }
}
