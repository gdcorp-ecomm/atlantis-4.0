﻿
namespace Atlantis.Framework.EcommInstoreAccept.Interface
{
  public enum InstoreAcceptResult : int
  {
    UnknownResult = -2,
    UnexpectedError = -1,
    Success = 0,
    ResellerNotOptedIn = 1,
    NoCreditsToConsume = 2,
    EmptyShopper = 98
  }
}
