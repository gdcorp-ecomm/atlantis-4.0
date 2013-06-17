using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class ReviewsSEOResponseData : IResponseData
  {

    public string HTML { get; protected set; }
    
    #region Constructors

    public ReviewsSEOResponseData(string html)
    {
      HTML = html;
    }

    #endregion

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
