using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommDomainEmailCredit.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommDomainEmailCredit.Impl
{
  public class EcommDomainEmailCreditRequest:IRequest
  {

    private const string _PROCNAME = "gdshop_getDomainsByOrderIDForFreeEmail_sp";
    private const string _SHOPPERIDPARAM = "s_order_id";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;

      try
      {
        EcommDomainEmailCreditRequestData request = (EcommDomainEmailCreditRequestData)oRequestData;
        string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
        List<DomainResults> results = new List<DomainResults>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.OrderID));
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
            foreach(KeyValuePair<string,DomainResults> currentDomain in resultValues)
            {
              results.Add(currentDomain.Value);
            }
          }
        }
        oResponseData = new EcommDomainEmailCreditResponseData(results);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommDomainEmailCreditResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommDomainEmailCreditResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    //private string LookupConnectionString(EcommDomainEmailCreditRequestData request, ConfigElement config)
    //{
    //  string result = string.Empty;
    //  netConnect.Info oNetConnect = new netConnect.Info();
    //  string dataSource = config.GetConfigValue("DataSourceName");
    //  string applicationName = config.GetConfigValue("ApplicationName");
    //  string certificateName = config.GetConfigValue("CertificateName");
    //  if (!String.IsNullOrEmpty(dataSource) && !String.IsNullOrEmpty(applicationName) &&
    //    !String.IsNullOrEmpty(certificateName))
    //  {
    //    result = oNetConnect.Get(dataSource, applicationName, certificateName,
    //       ConnectTypeEnum.CONNECT_TYPE_NET);
    //  }

    //  //when an error occurs a ';' is returned not a valid connection string or empty
    //  if (result.Length <= 1)
    //  {
    //    throw new AtlantisException(request, "LookupConnectionString",
    //            "Database connection string lookup failed", "No ConnectionFound For:"
    //            + dataSource + ":"
    //            + applicationName
    //            + ":" + certificateName);
    //  }

    //  return result;
    //}

  }
}
