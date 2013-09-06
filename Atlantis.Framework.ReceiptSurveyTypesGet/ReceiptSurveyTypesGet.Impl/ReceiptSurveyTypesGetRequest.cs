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
            IEnumerable<SurveyItem> surveyItems = CreateSurveyItemsFromReader(dataReader, request);
            responseData = new ReceiptSurveyTypesGetResponseData(surveyItems, request.Culture);
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

    private const string _TYPEID = "gdshop_shopper_survey_typeID";
    private const string _GROUPID = "gdshop_shopper_survey_groupID";

    private IEnumerable<SurveyItem> CreateSurveyItemsFromReader(SqlDataReader reader, ReceiptSurveyTypesGetRequestData request)
    {
      List<SurveyItem> items = new List<SurveyItem>();
      LanguageKeyFactory languageKey = new LanguageKeyFactory();
      FetchResource resourcFetcher = new FetchResource("Atlantis.Framework.ReceiptSurveyTypesGet.Interface.LanguageResources.ReceiptSurveyTypes", request.Culture);

      while (reader.Read())
      {
        string typeId = reader[_TYPEID].ToString();
        string surveyLanguageKey = languageKey.GetLanguageKeyForSurveyTypeId(typeId);
        string surveyDescription = resourcFetcher.GetString(surveyLanguageKey);

        items.Add(new SurveyItem(
          typeId,
          surveyDescription,
          reader[_GROUPID].ToString().ToLower()
        ));
      }

      return items;
    }
    #endregion
  }

}
