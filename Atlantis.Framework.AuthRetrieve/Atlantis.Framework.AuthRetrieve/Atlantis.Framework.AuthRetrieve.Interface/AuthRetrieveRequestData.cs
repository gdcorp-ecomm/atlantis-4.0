using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthRetrieve.Interface
{
  public class AuthRetrieveRequestData : RequestData
  {
    public string SpKey { get; private set; }
    public string Artifact { get; private set; }

    public AuthRetrieveRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string spKey
      , string artifact)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      SpKey = string.IsNullOrEmpty(spKey) ? string.Empty : spKey;
      Artifact = string.IsNullOrEmpty(artifact) ? string.Empty : artifact;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuthRetrieveRequestData");
    }
  }
}
