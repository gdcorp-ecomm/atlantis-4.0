using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Domains
{
  public class GrammaticalDelimiterToken : XmlToken
  {
    public GrammaticalDelimiterToken(string key, string data, string fullToken)
      : base(key, data, fullToken)
    {
      Delimiter = GetAttributeText("delimiter", string.Empty);
      GrammaticalDelimiter = GetAttributeText("grammatical", string.Empty);
    }

    public string Delimiter
    {
      get;
      set;
    }

    public string GrammaticalDelimiter
    {
      get;
      set;
    }
  }
}
