using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GoodAsGoldReport.Interface.Abstract;
using Atlantis.Framework.GoodAsGoldReport.Interface.Concrete;

namespace Atlantis.Framework.GoodAsGoldReport.Interface
{
  public class GoodAsGoldReportResponseData :IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;
    private DataSet _report = null;

    public IPageResult PageTotals { get; set; }
    public string BeginningBalance { get; set; }
    public string EndingBalance { get; set; }
    public string CurrentBalance { get; set; }

    public GoodAsGoldReportResponseData(DataSet returnAllReport, int totalPages, int totalRecords, string beginningBalance, string endingBalance, string currentBalance)
    {
      PageTotals = new GAGPagingResult(totalPages, totalRecords);
      BeginningBalance = beginningBalance;
      EndingBalance = endingBalance;
      CurrentBalance = currentBalance;
      _report = returnAllReport;
      _success = (returnAllReport != null);
    }

    public GoodAsGoldReportResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "GoodAsGoldReportResponseData", ex.Message, string.Empty);
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
