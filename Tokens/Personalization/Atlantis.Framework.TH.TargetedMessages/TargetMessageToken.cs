using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.TargetedMessages
{
  public class TargetMessageToken : XmlToken
  {
    public string RenderType
    {
      get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
    }

    public string MessageTagName { get; set; }
    public string AppId { get; set; }
    public string InteractionPoint { get; set; }

    public TargetMessageToken(string key, string data, string fullTokenString) : base(key, data, fullTokenString)
    {
      MessageTagName = GetAttributeText("name", null);
      AppId = GetAttributeText("appid", null);
      InteractionPoint = GetAttributeText("interactionpoint", null);
    }
  }
}
