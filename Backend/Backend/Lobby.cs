using Backend.Model;
using Backend.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
  public class Lobby
  {
    public Lobby()
    {
      tcpClients = new List<LobbyClient>();
    }

    private List<LobbyClient> tcpClients;

    public Thread threadLobby;

    private bool _isStarted = false;

    private bool _acceptingClients = true;

    #region Control Behavior
    public void Init()
    {
      if (!_isStarted)
      {
        Start();
      }
    }

    private void Start()
    {
      Console.WriteLine("Initializing Lobby");
      _isStarted = true;

      TcpListener lobbyServer;
      lobbyServer = new TcpListener(IPAddress.Any, 25560);
      lobbyServer.Start();
      Console.WriteLine("Lobby Initialized");
      Thread thread = new Thread(() => AcceptClient(lobbyServer));
      thread.Start();
    }
    private void Stop()
    {
      Console.WriteLine("Stopping Lobby");
      _isStarted = false;
    }
    public void ToggleAcceptingClients(bool value)
    {
      _acceptingClients = value;

      Console.WriteLine(value ? "Lobby is currently accepting connections" : "Lobby is currently declining connections");
    }


    public void Close()
    {
      if (_isStarted)
        Stop();
    }

    #endregion


    #region Control Clients

    public void GetClientList(string search)
    {
      Table table = new Table();
      table.SetHeaders("Id", "Alias", "Ip");


      if (string.IsNullOrEmpty(search))
      {
        foreach (LobbyClient item in tcpClients)
        {
          table.AddRow(item.Id.ToString(), item.Alias, item.Client.Client.RemoteEndPoint.ToString());
        }
      }
      else
      {
        search= search.ToLower();
        foreach (LobbyClient item in tcpClients.Where(x => x.Id.ToString().ToLower().Contains(search) 
        || x.Alias.ToLower().Contains(search)
        || x.Client.Client.RemoteEndPoint.ToString().ToLower().Contains(search)))
        {
          table.AddRow(item.Id.ToString(), item.Alias, item.Client.Client.RemoteEndPoint.ToString());
        }
      }
      
      Console.WriteLine(table.ToString());
    }

    public void DisconnectClient(string identifier)
    {

      Guid id;

      bool canParse = Guid.TryParse(identifier, out id);
      List<LobbyClient> clients = new List<LobbyClient>();

      if (canParse)
        clients.AddRange(tcpClients.Where(x => x.Id == id).ToList());
      else
        clients.AddRange(tcpClients.Where(x => x.Alias.Contains(identifier)).ToList());

      if (clients.Count > 0)
      {
        foreach (LobbyClient? client in clients)
        {
          if (client!.Disconnect())
          {
            Console.WriteLine(string.Format("{0} Disconnected", client.Alias));
            tcpClients.Remove(client);
          }
        }
      }
    }

    private void AcceptClient(TcpListener tcpListener)
    {
      try
      {
        while (true)
        {
          Console.WriteLine("Waiting for new connections");
          TcpClient client = tcpListener.AcceptTcpClient();
          Console.WriteLine(string.Format("{0} trying to connect", client.Client.RemoteEndPoint!.ToString()));

          Thread connectedClientTh = new Thread(() =>
          {
            try
            {
              if (_acceptingClients)
              {

                NetworkStream ns = client.GetStream();
                Guid guid = Guid.NewGuid();
                Console.WriteLine(string.Format("{0} id: {1}", client.Client.RemoteEndPoint!.ToString(), guid.ToString()));
                byte[] encodedMessage = Encoding.UTF8.GetBytes(new ConnectionCommunication(ConnectionMessageType.Message, string.Format("Hello {0}, send your alias",
                  client.Client.RemoteEndPoint!.ToString())).ToString());
                ns.Write(encodedMessage, 0, encodedMessage.Length);

                byte[] decodedMessage = new byte[4096];
                int readedBytes = ns.Read(decodedMessage, 0, decodedMessage.Length);

                ConnectionCommunication? response = JsonConvert.DeserializeObject<ConnectionCommunication>(Encoding.UTF8.GetString(decodedMessage, 0, readedBytes));


                encodedMessage = Encoding.UTF8.GetBytes(new ConnectionCommunication(ConnectionMessageType.HandShake, string.Format("Ok, This is your id {0}",
                  guid.ToString()), guid.ToString()).ToString());

                ns.Write(encodedMessage, 0, encodedMessage.Length);

                LobbyClient lobbyClient = new LobbyClient();
                lobbyClient.Client = client;
                lobbyClient.Alias = response!.Value;
                lobbyClient.Id = guid;
                lobbyClient.InitializeClient();
                tcpClients.Add(lobbyClient);
                Console.WriteLine(string.Format("{0} Connected", lobbyClient.Alias));
              }
              else
              {
                NetworkStream ns = client.GetStream();
                Console.WriteLine("Refused Connection");
                byte[] encodedMessage = Encoding.UTF8.GetBytes(string.Format("Unabled for new connections"));
                ns.Write(encodedMessage, 0, encodedMessage.Length);
                ns.Dispose();
                client.Close();
              }
            }
            catch (Exception ex)
            {
              Console.WriteLine("Aborted Connection");
            }
          });
          connectedClientTh.Start();
        }
      }
      catch (Exception ex)
      {

      }
    }
    #endregion



  }
}
