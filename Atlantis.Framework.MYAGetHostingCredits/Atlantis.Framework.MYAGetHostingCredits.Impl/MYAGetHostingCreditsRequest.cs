using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetHostingCredits.Interface;

namespace Atlantis.Framework.MYAGetHostingCredits.Impl
{
  public class MYAGetHostingCreditsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MYAGetHostingCreditsResponseData responseData;

      try
      {
        var hostingCreditsRequestData = (MYAGetHostingCreditsRequestData)requestData;
        List<HostingCredit> hostingCredits = GetHostingCredits(hostingCreditsRequestData, config);

        responseData = new MYAGetHostingCreditsResponseData(hostingCredits);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MYAGetHostingCreditsResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MYAGetHostingCreditsResponseData(requestData, ex);
      }

      return responseData;
    }

    public List<HostingCredit> GetHostingCredits(MYAGetHostingCreditsRequestData requestData, ConfigElement config)
    {
      var hostingCredits = new List<HostingCredit>();
      const string procName = "dbo.gdshop_getUnifiedResourceByTypeID_sp";

      string procXml = CreateProcInputXml(requestData.ProductTypeIds);

      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(procName, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@s_shopper_id", requestData.ShopperID));
          cmd.Parameters.Add(new SqlParameter("@xmldoc", procXml));
          cn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader())
          {
            while (dr.Read())
            {
              hostingCredits.Add(PopulateObjectFromDb(dr));
            }
          }
        }
      }
      return hostingCredits;
    }

    private static HostingCredit PopulateObjectFromDb(IDataReader dr)
    {
      var hostingCredit = new HostingCredit
                            {
                              Id = Convert.ToInt32(dr["gdshop_product_typeId"]),
                              Count = Convert.ToInt32(dr["Credits"])
                            };

      return hostingCredit;
    }

    private static string CreateProcInputXml(List<int> productTypeIds)
    {
      var requestDoc = new XDocument();
      var requestRoot = new XElement("credits");
      requestDoc.Add(requestRoot);

      foreach (int id in productTypeIds)
      {
        requestRoot.Add(new XElement("type", new XAttribute("id", id)));
      }
      return requestDoc.ToString();
    }
  }
}
