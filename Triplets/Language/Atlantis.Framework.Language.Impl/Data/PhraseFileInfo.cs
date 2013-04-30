using System.IO;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class PhraseFileInfo
  {
    private FileInfo _fileInfo;

    public string DictionaryName { get; private set; }
    public string Language { get; private set; }

    internal PhraseFileInfo(string fullFileName)
    {
      _fileInfo = new FileInfo(fullFileName);
      ParseFileName();
    }

    public bool IsLanguageDataValid
    {
      get
      {
        return (!string.IsNullOrEmpty(DictionaryName)) && (!string.IsNullOrEmpty(Language));
      }
    }

    private void ParseFileName()
    {
      DictionaryName = string.Empty;
      Language = string.Empty;

      string extentionlessFileName = _fileInfo.Name;

      int lastDot = extentionlessFileName.LastIndexOf('.');
      if (lastDot > -1)
      {
        extentionlessFileName = extentionlessFileName.Substring(0, lastDot);
      }

      int firstDash = extentionlessFileName.IndexOf('-');
      if (firstDash > -1)
      {
        DictionaryName = extentionlessFileName.Substring(0, firstDash);
        if (extentionlessFileName.Length > firstDash + 2)
        {
          Language = extentionlessFileName.Substring(firstDash + 1);
        }
      }
    }

  }
}
