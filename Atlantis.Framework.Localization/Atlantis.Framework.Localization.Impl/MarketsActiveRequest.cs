using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class MarketsActiveRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      //  TODO:  Change to an actual DataCache request when it is available

      IResponseData result = MarketsActiveResponseData.DefaultMarkets;

      return result;
    }

    #endregion
  }
}
