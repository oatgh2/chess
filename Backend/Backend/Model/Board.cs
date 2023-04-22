using Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model
{
  public class Board
  {
    private const int width = 8;
    private const int heigh = 8;
    private int maxHouses = width * heigh;

    private List<House> houses = new List<House>();

    public void InitBoard()
    {
      string hPos = "abcdefgh";

      for (int vPosPointer = 1; vPosPointer < heigh; vPosPointer++)
      {
        House house = new House();
        house.VPos = vPosPointer;
        for (int hPosPointer = 0; hPosPointer < width; hPosPointer++)
        {
          string hPointedPos = hPos[hPosPointer].ToString();
          house.HPos = hPointedPos;
        }
        house.Part = Chess.GetInitPosPart(house.RealPos);
        houses.Add(house);
      }
    }

    public string MovePart(string part, int vPos, string hPos)
    {

      bool isValidMoviment = Chess.IsValidMoviment(houses, part, vPos, hPos);
      bool isValidPart = Chess.IsValidPart(houses, part);

      if (isValidPart)
      {
        if (isValidMoviment)
        {
          House house = houses.Where(x => x.VPos == vPos && x.HPos.Equals(hPos)).FirstOrDefault()!;
          houses.Remove(house);
          house.Part = part;
        }
        else
        {
          return "Invalid Moviment";
        }
      }
      else
      {
        return "Invalid Part";
      }
      return "Moved";
    }
  }

  public class House
  {
    public string HPos { get; set; }
    public int VPos { get; set; }
    public (int, int) OrdenedPair
    {
      get
      {
        string hPositions = "abcdefgh";
        int realHPosition = hPositions.IndexOf(HPos) + 1;
        return (realHPosition, VPos);
      }
    }
    public int Number
    {
      get
      {
        string hPositions = "abcdefgh";
        int hPosRealNumber = hPositions.IndexOf(HPos) + 1;
        int vPosRealNumber = VPos + 1;
        int realPosNumber = vPosRealNumber * hPosRealNumber;
        return realPosNumber;
      }
    }
    public string RealPos
    {
      get
      {
        return string.Format("{0}{1}", HPos, VPos);
      }
    }
    public string Part { get; set; }
  }
}
