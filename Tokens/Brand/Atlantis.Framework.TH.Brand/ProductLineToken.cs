using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Brand
{
  public class ProductLineToken : XmlToken
  {
    public string RenderType
    {
      get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
    }

    public string ContextId { get; set; }

    public ProductLineToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
      ContextId = GetAttributeText("contextid", null);
    }
  }
}
