using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Interface;
using Atlantis.Framework.Iris.Interface.Objects;
using Atlantis.Framework.Providers.Iris.Interface;
using System.Threading;

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

      public Incident CreateIrisIncident(int subscriberId, string subject, string note, string customerEmailAddress, string ipAddress,
        string createdBy, int privateLableId, int groupId, int serviceId, string shopperId)
      {
        Incident createdIncident = null;
        long incidentId;
        try
        {
          var requestData = new CreateIncidentRequestData(subscriberId, subject, note, customerEmailAddress,
            ipAddress, groupId, serviceId, createdBy, privateLableId, shopperId);
          DateTime requestDateTime = DateTime.Now;
          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisCreate) as CreateIncidentResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            incidentId = responseData.IncidentId;
            if (incidentId != 0)
            {
              createdIncident = FetchNewIncidentWithRetry(shopperId, incidentId, requestDateTime);
            }
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.CreateIrisIncident", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return createdIncident;
      }

      private Incident FetchNewIncidentWithRetry(string shopperId, long incidentId, DateTime since)
      {
        Incident incident =  FetchNewIncident(shopperId, incidentId, since);
        if (incident == null)
        {
          Thread.Sleep(500);
          incident = FetchNewIncident(shopperId, incidentId, since);
        }
        return incident;
      }

      private Incident FetchNewIncident(string shopperId, long incidentId, DateTime since)
      {
        Incident newIncident = null;
        // I don't see a method for grabbing a specific incident by ID
        // So, for now, we'll just grab for a very recent timeframe, and step through the return list and match ID - mjw 1/9/14
        var incidents = GetIncidents(shopperId, since, DateTime.Now, true);
        foreach (Incident incident in incidents)
        {
          if (incident.IncidentId.Equals(incidentId))
          {
            newIncident = incident;
            break;
          }
        }
        return newIncident;
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

      public List<Incident> GetIncidents(string shopperId, DateTime startDate, DateTime endDate, bool deepLoad)
      {
        var retValue = new List<Incident>();

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
              foreach (var incident in responseData.Incidents)
              {
                //1 denotes ALL notes for ticket should be retrieved
                incident.Notes = GetIncidentNotes(incident.IncidentId, 1);
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

      public List<Note> GetIncidentNotes(long incidentId, int noteId)
      {
        var retValue = new List<Note>();

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
