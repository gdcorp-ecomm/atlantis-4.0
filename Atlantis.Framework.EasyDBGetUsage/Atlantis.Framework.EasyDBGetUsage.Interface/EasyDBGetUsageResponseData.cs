using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EasyDBGetUsage.Interface
{
  public class EasyDBGetUsageResponseData : IResponseData
  {
    private AtlantisException _atlantisException = null;
    private string _resultXML = string.Empty;
    public double UsedDiskSpace { get; private set; }
    public double UsedBandwidth { get; private set; }
    public double TotalDiskSpace { get; private set; }
    public double TotalBandwidth { get; private set; }
    public string MeasurementUnit { get; private set; }
    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public EasyDBGetUsageResponseData()
    { }

    public EasyDBGetUsageResponseData(double usedDiskSpace, double totalDiskSpace, double usedBandwidth, double totalBandwidth, string measurementUnit)
    {
      UsedDiskSpace = usedDiskSpace;
      UsedBandwidth = usedBandwidth;
      TotalDiskSpace = totalDiskSpace;
      TotalBandwidth = totalBandwidth;
      MeasurementUnit = measurementUnit;
      _resultXML = new XElement("usagereport",
          new XAttribute("measurementunit", MeasurementUnit),
          new XElement("diskspace",
            new XAttribute("useddiskspace", UsedDiskSpace),
            new XAttribute("totaldiskspace", TotalDiskSpace)),
          new XElement("bandwidth",
            new XAttribute("usedbandwidth", UsedBandwidth),
            new XAttribute("totalbandwidth", TotalBandwidth))
      ).ToString();
    }

    public EasyDBGetUsageResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
    }

    public EasyDBGetUsageResponseData(RequestData requestData, Exception exception)
    {
      _atlantisException = new AtlantisException(requestData,
                                   "EasyDBGetUsageResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion
  }
}
