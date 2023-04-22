using Backend.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Utils
{
  public class ValidMoviment
  {
    public bool IsValid { get; set; }
    public bool IsCapture { get; set; }
    public (int, int) Coordinates { get; set; }
  }


  public class Part
  {
    public Part(string name, string initialPos)
    {
      Name = name;
      InitialPos = initialPos;
    }

    public string Name { get; set; }
    public string InitialPos { get; set; }
  }
  public static class Chess
  {
    private static List<Part> Parts = new List<Part>() {
      new Part("tw", "a1"),
      new Part("tw", "h1"),
      new Part("cw", "b1"),
      new Part("cw", "g1"),
      new Part("bw", "c1"),
      new Part("bw", "f1"),
      new Part("dw", "e1"),
      new Part("rw", "d1"),
      new Part("tb", "a8"),
      new Part("tb", "h8"),
      new Part("cb", "b8"),
      new Part("cb", "g8"),
      new Part("bb", "c8"),
      new Part("bb", "f8"),
      new Part("db", "e8"),
      new Part("rb", "d8"),
    };

    public static ValidMoviment IsValidMoviment(List<House> houses, string part, int vPos, string hPos)
    {
      string hPositions = "abcdefgh";
      bool isValid = false;
      List<House> housesWherePart = houses.Where(x => x.Part == part).ToList();
      ValidMoviment validMoviment = null;
      foreach (House house in housesWherePart)
      {
        int originVPos = house.VPos;
        string originHPos = house.HPos;

        int initialPosH = hPositions.IndexOf(originHPos) + 1;
        int finalPosH = hPositions.IndexOf(hPos) + 1;

        if (part.Contains("bw") || part.Contains("bb"))
        {
          isValid = Math.Abs(finalPosH - initialPosH) == Math.Abs(vPos - originVPos);
          if (isValid)
          {
            int deltaX = Math.Sign(finalPosH - initialPosH);
            int deltaY = Math.Sign(vPos - originVPos);

            int x = initialPosH + deltaX;
            int y = originVPos + deltaY;

            while (x != finalPosH && y != vPos)
            {
              House actuallyHouse = houses.Where(a => a.OrdenedPair.Equals((x, y))).FirstOrDefault()!;
              if (!string.IsNullOrEmpty(actuallyHouse.Part))
              {
                validMoviment = new ValidMoviment();
                if (!actuallyHouse.Part.EndsWith(part.Last()))
                {
                  validMoviment.IsValid = true;
                  validMoviment.IsCapture = true;
                  validMoviment.Coordinates = (x, y);
                }
                else
                {
                  validMoviment.IsValid = false;
                  validMoviment.IsCapture = false;
                  validMoviment.Coordinates = (x, y);
                }
              }

              x += deltaX;
              y += deltaY;
            }
            validMoviment = new ValidMoviment()
            {
              IsValid = true,
              IsCapture = false,
              Coordinates = (x, y)
            };
          }

        }

        if (part.Contains("cw") || part.Contains("cb"))
        {
          int deltaX = Math.Abs(finalPosH - initialPosH);
          int deltaY = Math.Abs(vPos - originVPos);
          isValid = (deltaX == 2 && deltaY == 1) || (deltaX == 1 && deltaY == 2);

          if (isValid)
          {
            House actuallyHouse = houses.Where(a => a.OrdenedPair.Equals((finalPosH, vPos))).FirstOrDefault()!;
            if (!string.IsNullOrEmpty(actuallyHouse.Part))
            {
              
            }
          }
        }
      }
      if (validMoviment == null)
      {
        validMoviment = new ValidMoviment()
        {
          IsValid = false,
          IsCapture = false,
        };
      }

      return validMoviment;
    }

    public static string GetInitPosPart(string house)
    {
      string result = Parts.Where(x => x.InitialPos == house).Select(x => x.Name ?? "").FirstOrDefault()!;
      result = result ?? "";
      return result;
    }

    public static bool IsValidPart(List<House> houses, string part)
    {
      int countOfParts = houses.Where(x => x.Part.Equals(part)).Count();
      bool result = countOfParts > 0;
      return result;
    }
  }
}
