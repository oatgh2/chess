using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model
{
  public class SessionClient
  {
    public LobbyClient Client { get; set; }
    public int Points { get; set; }
    public string LastPlayed { get; set; }
  }
}
