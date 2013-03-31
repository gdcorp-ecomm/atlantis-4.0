﻿using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class ValidCountrySubdomainsRequestData : RequestData
  {
    public ValidCountrySubdomainsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {

    }

    public override string GetCacheMD5()
    {
      return "SALES";
    }
  }
}
