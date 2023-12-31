﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PixelsGetXML.Interface;

namespace Atlantis.Framework.PixelsGetXML.Impl
{
  public class PixelsGetXMLRequest : IRequest
  {
    const string _PROCNAME = "gdshop_pixelConfigGetActive_sp";
    const string _APPNAMEPARAM = "@s_appName";
    const string _PAGENAMEPARAM = "@s_pageName";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PixelsGetXMLResponseData result = new PixelsGetXMLResponseData();

      try
      {
        PixelsGetXMLRequestData pixelRequestData = (PixelsGetXMLRequestData)requestData;
        if (!string.IsNullOrEmpty(pixelRequestData.AppName) && !(string.IsNullOrEmpty(pixelRequestData.AppName) && string.IsNullOrEmpty(pixelRequestData.PageName)))
        {
          string connectionString = NetConnect.LookupConnectInfo(config);
          using (SqlConnection connection = new SqlConnection(connectionString))
          {
            using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
            {
              command.CommandType = CommandType.StoredProcedure;
              command.Parameters.Add(new SqlParameter(_APPNAMEPARAM, pixelRequestData.AppName));
              command.Parameters.Add(new SqlParameter(_PAGENAMEPARAM, pixelRequestData.PageName));

              command.CommandTimeout = (int)pixelRequestData.RequestTimeout.TotalSeconds;

              XDocument xmlResults = XDocument.Parse("<pixels />");

              using (SqlDataAdapter adapter = new SqlDataAdapter(command))
              {
                DataTable pixelDataTable = new DataTable();
                adapter.Fill(pixelDataTable);

                foreach (DataRow pixelRow in pixelDataTable.Rows)
                {
                  string pixel = string.Empty;
                  string trigger = string.Empty;
                  string additionalData = string.Empty;
                  try
                  {
                    if (!(pixelRow["pixelXml"] is DBNull))
                    {
                      pixel = Convert.ToString(pixelRow["pixelXml"]);
                    }
                    if (!(pixelRow["triggerXml"] is DBNull))
                    {
                      trigger = Convert.ToString(pixelRow["triggerXml"]);
                    }
                    if (!(pixelRow["additionalData"] is DBNull))
                    {
                      additionalData = Convert.ToString(pixelRow["additionalData"]);
                    }
                    if (!string.IsNullOrEmpty(pixel.Trim()))
                    {
                      XElement pixelXml = XElement.Parse(pixel);

                      if (!string.IsNullOrEmpty(trigger.Trim()))
                      {
                        XElement triggerXml = XElement.Parse(trigger);
                        pixelXml.Add(triggerXml);
                      }
                      if (!string.IsNullOrEmpty(additionalData.Trim()))
                      {
                        XElement additionalDataXml = XElement.Parse(additionalData);
                        pixelXml.Add(additionalDataXml);
                      }
                      xmlResults.Root.Add(pixelXml);
                    }
                  }
                  catch (Exception e)
                  {
                    var aex = new AtlantisException(requestData, "PixelsGetXMLRequest:RequestHandler", "Pixel XML failed to parse " + e.Message, pixel + trigger + additionalData);
                    Engine.Engine.LogAtlantisException(aex);
                    result = new PixelsGetXMLResponseData(requestData, e);
                  }
                }
              }
              result.PixelsXML = xmlResults;
            }
          }
        }
      }
      catch (Exception ex)
      {
        result = new PixelsGetXMLResponseData(requestData, ex);
      }

      return result;
    }
  }
}
