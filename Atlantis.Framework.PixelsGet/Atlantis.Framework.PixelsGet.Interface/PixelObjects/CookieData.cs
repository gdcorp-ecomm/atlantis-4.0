using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects
{
  public class CookieData
  {
    public CookieData(string name, int expirationDays, bool isEncoded, string value)
    {
      this.IsEncoded = IsEncoded;
      this.Name = name;
      this.Value = value;
      this.ExpirationDays = expirationDays;
    }

    public int ExpirationDays { get; set; }
    public bool IsEncoded { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
  }
}

