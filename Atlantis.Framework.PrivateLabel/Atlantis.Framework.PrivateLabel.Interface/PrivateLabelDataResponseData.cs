using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelDataResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }
    public string DataValue { get; private set; }

    private PrivateLabelDataResponseData()
    {
      DataValue = string.Empty;
      Exception = null;
    }

    public static PrivateLabelDataResponseData FromException(AtlantisException exception)
    {
      PrivateLabelDataResponseData result = new PrivateLabelDataResponseData();
      result.Exception = exception;
      return result;
    }

    public static PrivateLabelDataResponseData FromDataValue(string dataValue)
    {
      PrivateLabelDataResponseData result = new PrivateLabelDataResponseData();
      if (dataValue != null)
      {
        result.DataValue = dataValue;
      }
      return result;
    }

    public string ToXML()
    {
      XElement element = new XElement("PrivateLabelDataResponseData");
      element.Add(new XAttribute("data", DataValue));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return Exception;
    }

  }
}
