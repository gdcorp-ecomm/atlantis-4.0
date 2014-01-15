using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Iris.Interface
{
  public class GetIncidentCustomerNotesRequestData : RequestData
  {
    public GetIncidentCustomerNotesRequestData(long incidentId, int noteId)
    {
      IncidentId = incidentId;
      NoteId = noteId;
    }

    public long IncidentId { get; set; }
    public int NoteId { get; set; }
  }
}
