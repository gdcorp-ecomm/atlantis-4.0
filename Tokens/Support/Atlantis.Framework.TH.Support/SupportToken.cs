using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Support
{
  public class SupportToken : XmlToken
  {
    public SupportToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
      CityId = base.GetAttributeText("cityId", string.Empty);
      if (string.IsNullOrEmpty(CityId))
      {
        CityId = base.GetAttributeText("cityid", string.Empty);
      }
    }

    public string CityId
    {
      get;
      private set;
    }

    public string RenderType
    {
      get
      {
        return (!ReferenceEquals(null, TokenData) && !string.Equals("empty", TokenData.Name.ToString(), StringComparison.OrdinalIgnoreCase)) ? TokenData.Name.LocalName : string.Empty;
      }
    }

    //public SupportRenderType RenderType
    //{
    //  get
    //  {
    //    SupportRenderType returnValue = SupportRenderType.None;

    //    if (!ReferenceEquals(null, TokenData))
    //    {
    //      if (!Enum.TryParse(TokenData.Name.ToString(), true, out returnValue))
    //      {
    //        returnValue = SupportRenderType.None;
    //      }
    //    }
    //    return returnValue;
    //  }
    //}
  }
}