using Atlantis.Framework.Interface;
using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Iris.Interface
{
  [DataContract]
  public class QuickCreateIncidentResponseData : ResponseData
  {

    public static QuickCreateIncidentResponseData FromData(long data)
    {
      return new QuickCreateIncidentResponseData(data);
    }

    private QuickCreateIncidentResponseData(long responseCode)
    {
      IncidentId = responseCode;
      IsSuccess = IncidentId != 0;
    }

    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public long IncidentId { get; set; }

  }
}
