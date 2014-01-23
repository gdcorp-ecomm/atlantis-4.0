using Atlantis.Framework.Interface;
using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Iris.Interface
{
  [DataContract]
  public class AddIncidentNoteResponseData : ResponseData
  {

    public static AddIncidentNoteResponseData FromData(int data)
    {
      return new AddIncidentNoteResponseData(data);
    }

    private AddIncidentNoteResponseData(int responseCode)
    {
      IncidentNoteId = responseCode;

      IsSuccess = IncidentNoteId != 0;
    }

    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public int IncidentNoteId { get; set; }
  }
}
