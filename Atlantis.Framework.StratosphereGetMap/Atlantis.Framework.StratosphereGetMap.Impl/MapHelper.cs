using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.StratosphereGetMap.Interface;
using Atlantis.Framework.StratosphereGetMapUrl.Interface;

namespace Atlantis.Framework.StratosphereGetMap.Impl
{
  public class MapHelper
  {
    #region Get Stratosphere RequestUrl

    public static string GetStratosphereRequestUrl(StratosphereGetMapRequestData request, ConfigElement config, X509Certificate2 cert)
    {
      string url = string.Empty;
      StratosphereGetMapUrlResponseData response = null;

      StratosphereGetMapUrlRequestData urlRequest = new StratosphereGetMapUrlRequestData(request.ShopperID
        , request.SourceURL
        , request.OrderID
        , request.Pathway
        , request.PageCount
        , request.MapType
        , request.LookupValue
        , cert);

      int requestType = request.UpdatedStratosphereGetMapUrlRequest.HasValue ? request.UpdatedStratosphereGetMapUrlRequest.Value : urlRequest.StratosphereGetMapUrlRequestType;

      try
      {
        response = (StratosphereGetMapUrlResponseData)Engine.Engine.ProcessRequest(urlRequest, requestType);
      }
      catch (Exception ex)
      {
        throw new AtlantisException(urlRequest
          , "MapHelper::GetStratosphereRequestUrl"
          , ex.Message
          , string.Empty
          , ex);
      }

      if (response.IsSuccess && !string.IsNullOrEmpty(response.ToXML()))
      {
        url = response.ToXML();
      }
      else
      {
        response = (StratosphereGetMapUrlResponseData)Engine.Engine.ProcessRequest(urlRequest, requestType);
        if (response.IsSuccess && !string.IsNullOrEmpty(response.ToXML()))
        {
          url = response.ToXML();
        }
      }

      //Append the domain number limit if provided
      if (request.NumberOfDomains > 0)
      {
        url = url + string.Format("?pagesize={0}", request.NumberOfDomains);
      }

      return url;
    }
    #endregion

    #region Get Certificate
    public static X509Certificate2 FindCertificate(StoreLocation location, StoreName name, X509FindType findType, string findValue)
    {
      X509Store store = new X509Store(name, location);

      try
      {
        // create and open store for read-only access
        store.Open(OpenFlags.ReadOnly);

        // search store
        X509Certificate2Collection col = store.Certificates.Find(findType, findValue, true);

        // return first certificate found
        return col[0];
      }
      // always close the store
      finally
      {
        store.Close();
      }
    }
    #endregion
  }
}
