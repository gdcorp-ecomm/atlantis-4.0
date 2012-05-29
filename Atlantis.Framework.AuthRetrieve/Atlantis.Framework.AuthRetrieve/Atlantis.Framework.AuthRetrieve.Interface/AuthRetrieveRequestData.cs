using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthRetrieve.Interface
{
  public class AuthRetrieveRequestData : RequestData
  {

    public string SPKey { get; set; }
    public string Artifact { get; set; }

    public AuthRetrieveRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string spKey,
                                  string artifact)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(spKey))
      {
        SPKey = string.Empty;
      }
      else
      {
        SPKey = spKey;
      }
      if (string.IsNullOrEmpty(artifact))
      {
        Artifact = string.Empty;
      }
      else
      {
        Artifact = artifact;
      }

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuthRetrieveRequestData");
    }


  }
}
