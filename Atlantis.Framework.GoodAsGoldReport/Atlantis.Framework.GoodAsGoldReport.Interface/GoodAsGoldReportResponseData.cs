using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GoodAsGoldReport.Interface
{
  public class GoodAsGoldReportResponseData :IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;
    private DataSet _report = null;

    public GoodAsGoldReportResponseData(DataSet returnAllReport)
    {
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
