using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataProvider.Interface
{
  public class DataProviderRequestData : RequestData
  {
    public DataProviderRequestData(string sShopperID,
                             string sSourceURL,
                             string sOrderID,
                             string sPathway,
                             int iPageCount)
                             : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount) 
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public DataProviderRequestData(string sShopperID,
                             string sSourceURL,
                             string sOrderID,
                             string sPathway,
                             int iPageCount,
                             string requestName,
                             Dictionary<string, object> parameters)
                             : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestName = requestName;
      Params = parameters;
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public string RequestName { get; set; }
    public Dictionary<string, object> Params { get; set; }

    #region NimtizProperties

    private string _dsn = string.Empty;
    private string _appName = string.Empty;
    private string _certName = string.Empty;

    public string DataSourceName
    {
      get { return _dsn; }
      set { _dsn = value; }
    }

    public string ApplicationName
    {
      get { return _appName; }
      set { _appName = value; }
    }

    public string CertificateName
    {
      get { return _certName; }
      set { _certName = value; }
    }

    #endregion

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("DataProviderRequestData is not a cacheable request");
    }

    public override string ToXML()
    {
      return string.Empty;
    }

    #endregion

  }
}
