using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;

namespace Atlantis.Framework.Ecc.Interface
{
  public static class EccJsonResponseHandler
  {
    public static EccJsonFault ParseJsonFault(string json)
    {
      var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
      EccJsonFault oResponse;
      DataContractJsonSerializer ser;

      try
      {
        ser = new DataContractJsonSerializer(typeof(EccJsonFault));
        oResponse = ser.ReadObject(ms) as EccJsonFault;
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }

      return oResponse;
    }

  }
  public static class EccJsonResponseHandler<T>
  {

    public static EccJsonResponse<T> ParseJsonContent(string json)
    {
      var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
      EccJsonResponse<T> oResponse;
      DataContractJsonSerializer ser;

      try
      {
        ser = new DataContractJsonSerializer(typeof(EccJsonResponse<T>));
        oResponse = ser.ReadObject(ms) as EccJsonResponse<T>;
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }

      return oResponse;
    }

  }

  [DataContract(Namespace = "")]
  public class EccJsonResponseItem<T>
  {
    public EccJsonResponseItem()
    {
      Results = new List<T>();
    }

    /// <summary>
    /// http://wiki.stgwbe.com/index.php/Error_Codes
    /// </summary>
    [DataMember]
    public int ResultCode { get; set; }

    [DataMember]
    public string Message { get; set; }

    [DataMember]
    public string Timer { get; set; }
    public decimal TimerElapsed
    {
      get
      {
        decimal outTimer;
        decimal.TryParse(Timer, out outTimer);
        return outTimer;
      }
    }

    [DataMember]
    public List<T> Results { get; set; }

    [DataMember(Name = "state")]
    public string State { get; set; }
  }

  [DataContract(Name = "EccJsonResponse", Namespace = "")]
  public class EccJsonResponse<T>
  {
    public EccJsonResponse()
    {
      Item = new EccJsonResponseItem<T>();
    }

    [DataMember(Name = "response")]
    public EccJsonResponseItem<T> Item { get; set; }
  }
}
