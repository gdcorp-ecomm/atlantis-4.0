using Atlantis.Framework.Interface;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class ValidateNonOrderResponseData : IResponseData
  {
    private static ValidateNonOrderResponseData _inActiveResponse;

    static ValidateNonOrderResponseData()
    {
      _inActiveResponse = new ValidateNonOrderResponseData();
    }

    public static ValidateNonOrderResponseData InActiveResponse
    {
      get { return _inActiveResponse; }
    }

    public static ValidateNonOrderResponseData FromCacheDataXml(string cacheDataXml)
    {
      if (string.IsNullOrEmpty(cacheDataXml))
      {
        return _inActiveResponse;
      }

      XElement data = XElement.Parse(cacheDataXml);
      XElement item = data.Descendants("item").FirstOrDefault();
      if (item == null)
      {
        return _inActiveResponse;
      }

      XAttribute isActiveAtt = item.Attribute("isActive");
      XAttribute startDateAtt = item.Attribute("startDate");
      XAttribute endDateAtt = item.Attribute("endDate");

      bool isActive = "1".Equals(isActiveAtt.Value);
      DateTime startDate;
      if (!DateTime.TryParse(startDateAtt.Value, out startDate))
      {
        startDate = DateTime.MinValue;
      }

      DateTime endDate;
      if (!DateTime.TryParse(endDateAtt.Value, out endDate))
      {
        endDate = DateTime.MaxValue;
      }

      return new ValidateNonOrderResponseData(isActive, startDate, endDate);
    }

    public static ValidateNonOrderResponseData FromException(AtlantisException exception)
    {
      return new ValidateNonOrderResponseData(exception);
    }

    private AtlantisException _exception = null;

    private ValidateNonOrderResponseData()
    {
      IsActive = false;
      StartDate = DateTime.MinValue;
      EndDate = DateTime.MaxValue;
    }

    private ValidateNonOrderResponseData(AtlantisException exception)
      : this()
    {
      _exception = exception;
    }

    private ValidateNonOrderResponseData(bool isActive, DateTime startDate, DateTime endDate)
    {
      IsActive = isActive;
      StartDate = startDate;
      EndDate = endDate;
    }

    public bool IsActive {get; private set;}
    public DateTime StartDate {get; private set;}
    public DateTime EndDate {get; private set;}

    public string ToXML()
    {
      XElement element = new XElement("ValidateNonOrderResponseData");
      element.Add(
        new XAttribute("isactive", IsActive.ToString()),
        new XAttribute("startdate", StartDate.ToShortDateString()),
        new XAttribute("enddate", EndDate.ToShortDateString()));

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
