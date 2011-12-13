using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ReceiptSurvey.Interface;
using System.Data.SqlClient;
using System.Data;

namespace Atlantis.Framework.ReceiptSurvey.Impl
{
  public class ReceiptSurveyRequest : IRequest
  {
    private string procName = "dbo.gdshop_receipt_survey_Insert_sp";
    private string parm_shopperId = "@shopper_id";
    private string parm_shopper_survey_type = "@gdshop_receipt_survey_typeID";
    private string parm_display_position = "@i_displayPosition";
    private string parm_responseHowHeard = "@responseHowHeard";
    private string parm_response_section = "@responseFromSection";
    /*
     * [gdshop_receipt_survey_Insert_sp]
(	@shopper_id			VARCHAR(10),
	@gdshop_receipt_survey_typeID	INT,
	@i_displayPosition		INT = -1,
	@responseHowHeard		nvarchar(250) = NULL,
	@responseFromSection		int = NULL
)
     * */
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ReceiptSurveyResponseData responseData = null;
      ReceiptSurveyRequestData requestObject = null;
      try
      {
        requestObject = (ReceiptSurveyRequestData)requestData;

        using (var connection = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var command = new SqlCommand(procName, connection))
          {
            command.CommandTimeout = (int)requestObject.RequestTimeout.TotalSeconds;
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter(parm_shopperId, requestData.ShopperID));

            command.Parameters.Add(new SqlParameter(parm_shopper_survey_type, requestObject.SurveyTypeID));
            command.Parameters.Add(new SqlParameter(parm_display_position, requestObject.DisplayPosition));
            command.Parameters.Add(new SqlParameter(parm_responseHowHeard, requestObject.HowHeard));
            command.Parameters.Add(new SqlParameter(parm_response_section, requestObject.ResponseSection));

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            responseData = new ReceiptSurveyResponseData();
          }
        }
      }     
      catch (AtlantisException exAtlantis)
      {
        responseData = new ReceiptSurveyResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new ReceiptSurveyResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
