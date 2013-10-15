using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Providers.Basket.Interface;
using System;

namespace Atlantis.Framework.Providers.Basket
{
  public class BasketAddPriceOverride : IBasketAddPriceOverride
  {
    private readonly static DateTime _daysBase;
    private static readonly char[] _base25chars;

    static BasketAddPriceOverride()
    {
      _daysBase = new DateTime(1979, 9, 13, 0, 0, 0);  
      _base25chars = new char[25] {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O'};
    }

    internal BasketAddPriceOverride(int privateLabelId, int unifiedProductId, int overrideCurrentPrice, int overrideListPrice)
    {
      CurrentPrice = overrideCurrentPrice;
      ListPrice = overrideListPrice;
      Hash = GenerateOverrideHash(privateLabelId, unifiedProductId, overrideCurrentPrice, overrideListPrice);
    }

    public int CurrentPrice {get; private set;}
    public int ListPrice {get; private set;}
    public string Hash { get; private set; }

    private string GenerateOverrideHash(int privateLabelId, int unifiedProductId, int overrideCurrentPrice, int overrideListPrice)
    {
      var sb = new StringBuilder(100);
      sb.Append(privateLabelId.ToString(CultureInfo.InvariantCulture));
      sb.Append(unifiedProductId.ToString(CultureInfo.InvariantCulture));
      sb.Append(overrideListPrice.ToString(CultureInfo.InvariantCulture));
      sb.Append(overrideCurrentPrice.ToString(CultureInfo.InvariantCulture));

      var days = (int)DateTime.Now.Subtract(_daysBase).TotalDays;
      sb.Append(ConvertToBase25(days));
      //sb.Append(days.ToString(CultureInfo.InvariantCulture));

      return CreateSHA256Hash(sb.ToString());
    }

    private string CreateSHA256Hash(string stringToHash)
    {
      string result;

      using (var sha256 = SHA256.Create())
      {
        byte[] stringBytes = Encoding.ASCII.GetBytes(stringToHash);
        var sha256Bytes = sha256.ComputeHash(stringBytes);
        result = MapBytes(sha256Bytes);
      }

      return result;
    }

    private static string ConvertToBase25(int value)
    {
      int i = 32;
      var buffer = new char[i];
      int targetBase = _base25chars.Length;

      do
      {
        buffer[--i] = _base25chars[value%targetBase];
        value = value/targetBase;
      } 
      while (value > 0);

      var result = new char[32 - i];
      Array.Copy(buffer, i, result, 0, 32 - i);

      return new string(result);
    }

    private static string MapBytes(byte[] bytes)
    {
      var result = new byte[bytes.Length * 2];
      for (var i = 0; i < bytes.Length; i++)
      {
        var b = bytes[i];
        result[i * 2] = (byte)(b / 10 + 97);
        result[i * 2 + 1] = (byte)(b % 10 + 97);
      }

      return Encoding.ASCII.GetString(result);
    }

  }
}
