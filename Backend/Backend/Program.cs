
namespace Backend
{
  public class InitClass
  {
    static Lobby lobby;


    public static void Main(string[] args)
    {
      lobby = new Lobby();
      lobby.Init();

      while (true)
      {
        string? commmand = Console.ReadLine();
        if (!string.IsNullOrEmpty(commmand))
        {
          string[] splittedCommand = commmand.Split(" ");

          string function = "";
          string param = "";
          string param2 = "";


          if (splittedCommand.Length > 0)
            function = splittedCommand[0];

          if (splittedCommand.Length > 1)
            param = splittedCommand[1];

          if (splittedCommand.Length > 2)
          {
            param2 = commmand.Substring(commmand.LastIndexOf(' '));
          }

          switch (function.ToLower())
          {
            case "lobby":
              if (param.ToLower().Equals("stop"))
              {
                lobby.ToggleAcceptingClients(false);
              }
              else if (param.ToLower().Equals("start"))
              {
                lobby.ToggleAcceptingClients(true);
              }
              break;
            case "client":
              if (param.ToLower().Equals("disconnect"))
              {
                lobby.DisconnectClient(param2);
              }else if (param.ToLower().Equals("get"))
              {
                lobby.GetClientList(param2.Trim());
              }
              break; 
            case "clear":
              Console.Clear();
              break;
            default:
              Console.WriteLine("Command not found.");
              break;
          }
        }
      }
    }
  }
}