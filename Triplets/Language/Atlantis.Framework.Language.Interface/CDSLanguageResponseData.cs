using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Parsers.LanguageFile;

namespace Atlantis.Framework.Language.Interface
{
  public class CDSLanguageResponseData : IResponseData
  {
    public AtlantisException Exception { get; set; }
    public bool Exists { get; set; }
    public PhraseDictionary Phrases { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return Exception;
    }
  }
}