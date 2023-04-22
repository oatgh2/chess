using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model
{
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
}
