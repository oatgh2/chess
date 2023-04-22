using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model
{
  public class Session
  {
    public Session() { 
    
    }


    public Session(Guid id, string name, SessionClient firtsClient)
    {
      Id = id;
      Name = name;
      Player1= firtsClient;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public SessionClient? Player1 { get; set; }
    public SessionClient? Player2 { get; set; }
    public List<LobbyClient> Spectetors { get; set; }

    public Board Board { get; set; }

  }
}
