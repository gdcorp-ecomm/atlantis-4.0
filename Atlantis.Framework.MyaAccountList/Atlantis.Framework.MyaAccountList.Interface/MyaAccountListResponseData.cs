using System;
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
    private DataSet _report = null;

    public IPageResult PageTotals { get; set; }

    public MyaAccountListResponseData(DataSet returnAllReport, int totalPages, int totalRecords)
    {
      PageTotals = new AccountListPagingResult(totalPages, totalRecords);
      _report = returnAllReport;
      _success = (returnAllReport != null);
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

    public DataSet ReportDataSet
    {
      get { return _report; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, _report);

      return writer.ToString();
    }

    #endregion

    public AtlantisException GetException()
    {
      return _atlException;
    }

  }
}
