using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ReceiptSurveyTypesGet.Interface;
using Atlantis.Framework.Nimitz;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReceiptSurveyTypesGet.Impl
{
  public class ReceiptSurveyTypesGetRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ReceiptSurveyTypesGetResponseData responseData = null;

      try
      {
        var request = (ReceiptSurveyTypesGetRequestData)requestData;

        string connectionString = NetConnect.LookupConnectInfo(config);
        using (var connection = new SqlConnection(connectionString))
        {
          using (var command = new SqlCommand("gdshop_ReceiptSurveyTypes_GetByCountryCode_sp", connection))
          {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandTimeout = (int)requestData.RequestTimeout.Milliseconds;
            command.Parameters.AddWithValue("@countryCode", request.CountryCode);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            IEnumerable<SurveyItem> surveyItems = CreateSurveyItemsFromReader(dataReader);
            responseData = new ReceiptSurveyTypesGetResponseData(surveyItems);

            if (connection.State == System.Data.ConnectionState.Open)
            {
              connection.Close();
            }
          }
        }
      }
      catch (AtlantisException aex)
      {
        responseData = new ReceiptSurveyTypesGetResponseData(aex);
      }
      catch (Exception ex)
      {
        responseData = new ReceiptSurveyTypesGetResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Create Survey Items

    public const string _TYPEID = "gdshop_shopper_survey_typeID";
    public const string _DESCRIPTION = "typeDescription";
    public const string _GROUPID = "gdshop_shopper_survey_groupID";

    public IEnumerable<SurveyItem> CreateSurveyItemsFromReader(SqlDataReader reader)
    {
      List<SurveyItem> items = new List<SurveyItem>();

      while (reader.Read())
      {
        items.Add(new SurveyItem(
          reader[_TYPEID].ToString(),
          reader[_DESCRIPTION].ToString(),
          reader[_GROUPID].ToString().ToLower()
        ));
      }

      return items;
    }
    #endregion
  }

}
