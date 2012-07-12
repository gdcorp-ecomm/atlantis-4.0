using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using System.Data.SqlClient;
using Atlantis.Framework.ReceiptSurveyInsert.Interface;
using System.Data;

namespace Atlantis.Framework.ReceiptSurveyInsert.Impl
{
  public class ReceiptSurveyInsertRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ReceiptSurveyInsertResponseData responseData = null;

      try
      {
        var request = (ReceiptSurveyInsertRequestData)requestData;
        if (!ValidateRequest(request))
        {
          string data = string.Concat("shopperid:", request.ShopperID, "|countryCode:", request.CountryCode,
            "|surveyTypeId:", request.SurveyTypeId.ToString(), "|surveySelectedPosition:", request.SurveySelectionPosition.ToString());

          throw new AtlantisException(requestData, "ReceiptSurveyInsertRequest::RequestHandler", "Invalid data in request", data);
        }
        else
        {
          string connectionString = NetConnect.LookupConnectInfo(config);
          using (var connection = new SqlConnection(connectionString))
          {
            using (var command = new SqlCommand("gdshop_receipt_survey_insert_sp", connection))
            {
              command.CommandType = CommandType.StoredProcedure;
              command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;

              command.Parameters.AddWithValue("@shopper_id", request.ShopperID);
              command.Parameters.AddWithValue("@gdshop_receipt_survey_typeID", request.SurveyTypeId);
              command.Parameters.AddWithValue("@i_displayPosition", request.SurveySelectionPosition);
              command.Parameters.AddWithValue("@countryCode", request.CountryCode);

              connection.Open();
              command.ExecuteNonQuery();
              bool isSuccess = true;

              responseData = new ReceiptSurveyInsertResponseData(isSuccess);

              if (connection.State == System.Data.ConnectionState.Open)
              {
                connection.Close();
              }
            }
          }

        }
      }
      catch (AtlantisException aex)
      {
        responseData = new ReceiptSurveyInsertResponseData(aex);
      }
      catch (Exception ex)
      {
        responseData = new ReceiptSurveyInsertResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Validate Request
    private bool ValidateRequest(ReceiptSurveyInsertRequestData request)
    {
      bool isValid = true;

      if (string.IsNullOrEmpty(request.ShopperID))
      {
        isValid = false;
      }
      else if (string.IsNullOrEmpty(request.CountryCode) || request.CountryCode.Length > 2)
      {
        isValid = false;
      }
      else if (request.SurveySelectionPosition <= 0 || request.SurveyTypeId <= 0)
      {
        isValid = false;
      }

      return isValid;
    }

    #endregion
  }
}

