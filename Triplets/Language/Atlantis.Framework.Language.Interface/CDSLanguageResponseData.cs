using Atlantis.Framework.Interface;
using Atlantis.Framework.Parsers.LanguageFile;

namespace Atlantis.Framework.Language.Interface
{
  public class CDSLanguageResponseData : IResponseData
  {
    public PhraseDictionary Phrases { get; private set; }
    public string VersionId { get; private set; }
    public string DocumentId { get; private set; }
    public static CDSLanguageResponseData NotFound { get; private set; }

    static CDSLanguageResponseData()
    {
      NotFound = new CDSLanguageResponseData(new PhraseDictionary());
    }

    public CDSLanguageResponseData(PhraseDictionary phrases) : this(phrases, string.Empty, string.Empty)
    {
    }

    public CDSLanguageResponseData(PhraseDictionary phrases, string versionId, string documentId)
    {
      Phrases = phrases;
      VersionId = versionId;
      DocumentId = documentId;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}