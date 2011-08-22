﻿using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileGetById.Interface
{
  public class BuyerProfileGetByIdResponseData : IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;
    private BuyerProfileDetails.BuyerProfileDetails _details = null;

    public BuyerProfileGetByIdResponseData(BuyerProfileDetails.BuyerProfileDetails details)
    {
      _details = details;
      _success = (details != null);
    }

    public BuyerProfileGetByIdResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "BuyerProfileGetByIdResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public BuyerProfileDetails.BuyerProfileDetails BuyerProfileDetail
    {
      get { return _details; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, _details);

      return writer.ToString();

    }

    #endregion

    public AtlantisException GetException()
    {
      return _atlException;
    }
  }
}
