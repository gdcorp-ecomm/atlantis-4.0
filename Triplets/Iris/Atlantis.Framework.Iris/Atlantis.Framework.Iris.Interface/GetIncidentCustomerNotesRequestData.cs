using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Iris.Interface
{
  public class GetIncidentCustomerNotesRequestData : RequestData
  {
    public GetIncidentCustomerNotesRequestData(long incidentId, long noteId)
    {
      IncidentId = incidentId;
      NoteId = noteId;
    }

    public long IncidentId { get; set; }
    public long NoteId { get; set; }
  }
}
