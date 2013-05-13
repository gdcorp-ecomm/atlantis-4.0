﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperSegment.Interface;

namespace Atlantis.Framework.ShopperSegment.Impl
{
  public class ShopperSegmentRequest : IRequest
  {
    private const string PROCNAME = "dbo.fb_ShopperHeaderSegmentCodeGet_sp";
    private const string SHOPPER_ID_COL_NAME = "shopper_id";
    private const string PRIVATE_LABEL_ID_COL_NAME = "PrivateLabelID";
    private const string SEGMENT_CODE_COL_NAME = "SegmentCode";
    private const string SHOPPER_ID_PARAM_NAME = "@shopper_id";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperSegmentResponseData returnValue = new ShopperSegmentResponseData();

      try
      {
        if (!string.IsNullOrEmpty(requestData.ShopperID))
        {
          using (SqlConnection connection = new SqlConnection(NetConnect.LookupConnectInfo(config)))
          {
            using (SqlCommand command = new SqlCommand(PROCNAME, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds })
            {
              command.Parameters.Add(new SqlParameter(SHOPPER_ID_PARAM_NAME, requestData.ShopperID));
              connection.Open();
              using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
              {
                if (reader.HasRows)
                {
                  while (reader.Read())
                  {
                    int shopperIdOrdinal = reader.GetOrdinal(SHOPPER_ID_COL_NAME);
                    int plIdOrdinal = reader.GetOrdinal(PRIVATE_LABEL_ID_COL_NAME);
                    int segmentIdOrdinal = reader.GetOrdinal(SEGMENT_CODE_COL_NAME);

                    returnValue = new ShopperSegmentResponseData(
                      !reader.IsDBNull(segmentIdOrdinal) ? reader.GetByte(segmentIdOrdinal) : -1);
                  }
                }
              }
            }
          } 
        }
      }

      catch (AtlantisException exAtlantis)
      {
        returnValue = new ShopperSegmentResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        returnValue = new ShopperSegmentResponseData(requestData, ex);
      }

      return returnValue;
    }
  }
}
