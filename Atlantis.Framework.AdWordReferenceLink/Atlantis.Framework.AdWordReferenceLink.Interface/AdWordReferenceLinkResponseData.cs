using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AdWordReferenceLink.Interface
{
  public class AdWordReferenceLinkResponseData : IResponseData
  {
    private AtlantisException _atlException;

    public bool IsSuccess { get; private set; }
    public string AdWordReferenceLink = string.Empty;

    public AdWordReferenceLinkResponseData(string referenceLink)
    {
      IsSuccess = true;
      AdWordReferenceLink = referenceLink;
    }

    public AdWordReferenceLinkResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "AdWordReferenceLinkResponseData", ex.Message, string.Empty);
    }


    #region Implementation of IResponseData

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }

    #endregion
  }
}
