using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Interface;
using Atlantis.Framework.Iris.Interface.Objects;
using Atlantis.Framework.Providers.Iris.Interface;

namespace Atlantis.Framework.Providers.Iris
{
    public class IrisProvider : ProviderBase, IIrisProvider
    {
      public IrisProvider(IProviderContainer container) : base(container)
      {
      }

      public long QuickCreateIrisIncident(int subscriberId, string subject, string note, string customerEmailAddress,
        string ipAddress, string createdBy, int privateLableId, string shopperId)
      {
        long retValue = -1;
        try
        {
          var requestData = new QuickCreateIncidentRequestData(subscriberId, subject, note, customerEmailAddress,
            ipAddress, createdBy, privateLableId, shopperId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisQuickCreate) as QuickCreateIncidentResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.IncidentId;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.QuickCreateIrisIncident", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

      public long CreateIrisIncident(int subscriberId, string subject, string note, string customerEmailAddress, string ipAddress,
        string createdBy, int privateLableId, int groupId, int serviceId, string shopperId)
      {
        long retValue = -1;
        try
        {
          var requestData = new CreateIncidentRequestData(subscriberId, subject, note, customerEmailAddress,
            ipAddress, groupId, serviceId, createdBy, privateLableId, shopperId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisCreate) as CreateIncidentResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.IncidentId;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.CreateIrisIncident", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

      public long AddIncidentNote(long incidentId, string note, string loginId)
      {
        long retValue = -1;
        try
        {
          var requestData = new AddIncidentNoteRequestData(incidentId, note, loginId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisAddNote) as AddIncidentNoteResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.IncidentNoteId;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.AddIncidentNote", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

      public IncidentsList GetIncidents(string shopperId, DateTime startDate, DateTime endDate, bool deepLoad)
      {
        var retValue = new IncidentsList();

        try
        {
          var requestData = new GetIncidentsByShopperIdAndDateRangeRequestData(shopperId, startDate, endDate);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisGetIncidents) as GetIncidentsByShopperIdAndDateRangeResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.Incidents;

            if (deepLoad)
            {
              foreach (var incident in responseData.Incidents.Incidents.Items)
              {
                //1 denotes ALL notes for ticket should be retrieved
                incident.Notes = GetIncidentNotes(incident.IncidentId, 1).Notes;
              }
            }

          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.GetIncidents", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

      public NotesList GetIncidentNotes(long incidentId, int noteId)
      {
        var retValue = new NotesList();

        try
        {
          var requestData = new GetIncidentCustomerNotesRequestData(incidentId, noteId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisGetIncidentNotes) as GetIncidentCustomerNotesResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.Notes;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.GetIncidentNotes", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

    }
}
