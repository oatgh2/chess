using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model
{
  public enum ConnectionMessageType
  {
    Unknown = 0,
    HandShake = 1,
    Message = 2,
    Disconnecting = 3,
    CreateNewSession = 5,
    DeleteSession = 6,
  }
}
