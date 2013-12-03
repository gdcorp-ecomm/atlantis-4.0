using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.TH.Localization
{
    public class LocalizationToken : XmlToken
    {
      public string RenderType
      {
        get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
      }

      public bool FullLanguage { get; private set; }

      public LocalizationToken(string key, string data, string fullTokenString) :base(key, data, fullTokenString)
      {
        FullLanguage = GetAttributeBool("full", false);
      }
    }
}
