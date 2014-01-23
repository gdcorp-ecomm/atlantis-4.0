using Atlantis.Framework.Interface;
using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Iris.Interface
{
  [DataContract]
  public class CreateIncidentResponseData : ResponseData
  {

    public static CreateIncidentResponseData FromData(long data)
    {
      return new CreateIncidentResponseData(data);
    }

    private CreateIncidentResponseData(long responseCode)
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
