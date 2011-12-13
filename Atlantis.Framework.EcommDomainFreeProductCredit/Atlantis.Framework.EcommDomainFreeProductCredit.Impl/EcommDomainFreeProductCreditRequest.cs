using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommDomainFreeProductCredit.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommDomainFreeProductCredit.Impl
{
  public class EcommDomainFreeProductCreditRequest : IRequest
  {

    private const string _PROCNAME = "gdshop_getDomainsByOrderidForFreeProduct_sp";
    private const string _SHOPPERIDPARAM = "s_order_id";
    private const string _PRODUCTTYPEIDPARAM = "n_gdshop_product_typeID ";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      try
      {
        EcommDomainFreeProductCreditRequestData request = (EcommDomainFreeProductCreditRequestData)oRequestData;
        string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
        List<DomainResults> results = new List<DomainResults>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.OrderID));
            command.Parameters.Add(new SqlParameter(_PRODUCTTYPEIDPARAM, request.ProductTypeID));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            Dictionary<string, DomainResults> resultValues = new Dictionary<string, DomainResults>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                string domainName = Convert.ToString(reader[0]);
                int emailPFid = Convert.ToInt32(reader[2]);
                int domainResourceID = Convert.ToInt32(reader[1]);
                if (resultValues.ContainsKey(domainName))
                {
                  resultValues[domainName].AddEmailPfid(emailPFid);
                }
                else
                {
                  resultValues.Add(domainName, new DomainResults(domainName, emailPFid, domainResourceID));
                }
              }
            }
            foreach (KeyValuePair<string, DomainResults> currentDomain in resultValues)
            {
              results.Add(currentDomain.Value);
            }
          }
        }
        oResponseData = new EcommDomainFreeProductCreditResponseData(results);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommDomainFreeProductCreditResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommDomainFreeProductCreditResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
