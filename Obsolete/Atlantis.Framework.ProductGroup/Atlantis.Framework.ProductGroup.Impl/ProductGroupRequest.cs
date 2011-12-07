using System;
using System.Data;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductGroup.Interface;

namespace Atlantis.Framework.ProductGroup.Impl
{
  public class ProductGroupRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      DataTable dtResult = null;

      try
      {
        dtResult = DataCache.DataCache.GetCacheDataTable(oRequestData.ToXML());

        oResponseData = new ProductGroupResponseData(dtResult);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ProductGroupResponseData(dtResult, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ProductGroupResponseData(dtResult, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
