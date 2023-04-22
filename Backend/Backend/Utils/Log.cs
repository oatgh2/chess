using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Utils
{
  public static class Log
  {
    public static void Register(string value)
    {
      (int Left, int Top) pos = Console.GetCursorPosition();
      Console.SetCursorPosition(0, pos.Top - 2);
      Console.WriteLine(value);
      Console.SetCursorPosition(0, pos.Top);
    }
  }
}
