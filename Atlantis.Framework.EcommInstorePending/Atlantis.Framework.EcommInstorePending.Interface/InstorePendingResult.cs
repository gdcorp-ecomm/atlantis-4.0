
namespace Atlantis.Framework.EcommInstorePending.Interface
{
  public enum InstorePendingResult : int
  {
    UnknownResult = -2,
    UnexpectedError = -1,
    Success = 0,
    ResellerNotOptedIn = 1,
    NoCreditsToConsume = 2,
    EmptyShopper = 98,
    EmptyCurrency = 99
  }
}
