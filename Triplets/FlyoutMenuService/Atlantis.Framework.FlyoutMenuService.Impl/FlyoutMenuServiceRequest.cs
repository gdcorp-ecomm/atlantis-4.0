using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.FlyoutMenuService.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.FlyoutMenuService.Impl
{
  public class FlyoutMenuServiceRequest : IRequest
  {
    #region Database Constants

    private const string MENU_ITEM_PROC = "menu_item_get_sp";
    private const string MENU_SITE_PROC = "menuSite_get_sp";

    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FlyoutMenuServiceResponseData responseData;
      var ds = new DataSet();

      try
      {
        var request = (FlyoutMenuServiceRequestData)requestData;
        var procName = request.MenuServiceType.Equals(FlyoutMenuServiceRequestData.ServiceType.MenuItem) ? MENU_ITEM_PROC : MENU_SITE_PROC;

        using (var cn = new SqlConnection(NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand(procName, cn) { CommandType = CommandType.StoredProcedure, CommandTimeout = (int) requestData.RequestTimeout.TotalSeconds })
          {
            cn.Open();
            using (var da = new SqlDataAdapter(cmd))
            {
              da.Fill(ds);
              responseData = new FlyoutMenuServiceResponseData(ds);              
            }
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new FlyoutMenuServiceResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new FlyoutMenuServiceResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
