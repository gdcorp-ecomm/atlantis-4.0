﻿using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccountList.Interface.Abstract;
using Atlantis.Framework.MyaAccountList.Interface.Concrete;

namespace Atlantis.Framework.MyaAccountList.Interface
{
  public class MyaAccountListResponseData : IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;
    private DataSet _productList = null;

    public IPageResult PageTotals { get; set; }

    public MyaAccountListResponseData(DataSet returnAccountList, int totalPages, int totalRecords)
    {
      PageTotals = new AccountListPagingResult(totalPages, totalRecords);
      _productList = returnAccountList;
      _success = (returnAccountList != null);
    }

    public MyaAccountListResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "MyaAccountListResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public DataSet AccountListDataSet
    {
      get { return _productList; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, _productList);

      return writer.ToString();
    }

    public string AlsoToXML()
    {
      return _productList.GetXml();
    }

    #endregion

    public AtlantisException GetException()
    {
      return _atlException;
    }

  }
}
