using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MYAGetExpiringProductsDetail.Interface
{
  [DataContract]
  public class MYAGetExpiringProductsDetailResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _atlException;

    [DataMember]
    public int TotalPages { get; set; }

    [DataMember]
    public int TotalRecords { get; set; }

    [DataMember]
    public DataSet MyRenewalsSet { get; set; }

    [DataMember]
    public List<RenewingProductObject> RenewingProductsList { get; set; }

    public bool IsSuccess { get; private set; }

    public MYAGetExpiringProductsDetailResponseData()
    {
    }

    public MYAGetExpiringProductsDetailResponseData(DataSet ds, List<RenewingProductObject> productList, int totalRecords, int totalPages)
    {
      MyRenewalsSet = ds;
      IsSuccess = true;
      TotalRecords = totalRecords;
      TotalPages = totalPages;
      RenewingProductsList = productList;
    }

    public MYAGetExpiringProductsDetailResponseData(AtlantisException exAtlantis)
    {
      RenewingProductsList = new List<RenewingProductObject>(1);
      _atlException = exAtlantis;
    }

    public MYAGetExpiringProductsDetailResponseData(DataSet ds, RequestData oRequestData, Exception ex)
    {
      RenewingProductsList = new List<RenewingProductObject>(1);
      MyRenewalsSet = ds;
      _atlException = new AtlantisException(oRequestData, "MYAGetExpiringProductsDetailResponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion

    public string SerializeSessionData()
    {
      string serialized;

      try
      {
        DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(MYAGetExpiringProductsDetailResponseData));
        using (MemoryStream ms = new MemoryStream())
        {
          json.WriteObject(ms, this);
          ms.Position = 0;
          serialized = new StreamReader(ms).ReadToEnd();
        }
      }
      catch
      {
        serialized = string.Empty;
      }

      return serialized;
    }

    public void DeserializeSessionData(string sessionData)
    {
      try
      {
        byte[] ba = Encoding.ASCII.GetBytes(sessionData);
        using (MemoryStream ms = new MemoryStream(ba))
        {
          DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(MYAGetExpiringProductsDetailResponseData));
          MYAGetExpiringProductsDetailResponseData result = (MYAGetExpiringProductsDetailResponseData)dcs.ReadObject(ms);
          if (result != null)
          {
            MyRenewalsSet = result.MyRenewalsSet;
            TotalRecords = result.TotalRecords;
            TotalPages = result.TotalPages;
            RenewingProductsList = result.RenewingProductsList;
            IsSuccess = true;
          }
        }
      }
      catch
      {
        MyRenewalsSet = new DataSet();
        TotalRecords = 0;
        TotalPages = 0;
        RenewingProductsList = new List<RenewingProductObject>(1);
        IsSuccess = false;
        throw; // Rethrow exception so SessionCache logs the exception
      }
    }
  }
}
