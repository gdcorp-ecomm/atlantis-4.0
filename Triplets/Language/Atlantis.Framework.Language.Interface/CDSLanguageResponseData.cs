using Atlantis.Framework.Interface;
using Atlantis.Framework.Parsers.LanguageFile;

namespace Atlantis.Framework.Language.Interface
{
  public class CDSLanguageResponseData : IResponseData
  {
    private readonly AtlantisException _exception = null;
    public PhraseDictionary Phrases { get; private set; }
    public static CDSLanguageResponseData NotFound { get; private set; }

    static CDSLanguageResponseData()
    {
      NotFound = new CDSLanguageResponseData(new PhraseDictionary());
    }

    public CDSLanguageResponseData(PhraseDictionary phrases)
    {
      Phrases = phrases;
    }

    //do i need this if I am just throwing the exception?
    //public CDSLanguageResponseData(AtlantisException exception)
    //{
    //  _exception = exception;
    //}

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}