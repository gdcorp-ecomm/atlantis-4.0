using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyAppInsertUpdate.Interface
{
  public class PrivacyAppInsertUpdateResponseData : IResponseData
  {
    private string _pbstrOutput = string.Empty;
    private bool _hasError = false;
    private string _errorDescription = string.Empty;
    private string _hash = string.Empty;
    private int _result = 0;
    private int _statusId = 0;
    private AtlantisException _ex;

    public PrivacyAppInsertUpdateResponseData(int result, string pbstrOutput)
    {
      if(!string.IsNullOrEmpty(pbstrOutput))
      {
        ParseResponse(pbstrOutput);
      }

      _pbstrOutput = pbstrOutput;
      _result = result;
    }

    public int Result 
    {
      get { return _result; }
    }

    public string OutputValue
    {
      get { return _pbstrOutput; }
    }

    public bool HasError
    {
      get { return _hasError; }
    }

    public string ErrorDescription
    {
      get { return _errorDescription; }
    }

    public string Hash
    {
      get { return _hash; }
    }

    public int StatusID
    {
      get { return _statusId; }
    }

    private void ParseResponse(string pbstrOutput)
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(pbstrOutput);
      XmlNode xmlNode;
      
      xmlNode = xmlDoc.SelectSingleNode("Status/Hash");
      if (xmlNode != null)
      {
        _hash = xmlNode.InnerText;
      }
      
      xmlNode = xmlDoc.SelectSingleNode("Status/StatusID");
      if (xmlNode != null)
      {
        int status = 0;
        if (int.TryParse(xmlNode.InnerText, out status))
        {
          _statusId = status;
        }
      }

      if (string.IsNullOrEmpty(_hash))
      {
        xmlNode = xmlDoc.SelectSingleNode("Error/Reason");
        if (xmlNode != null)
        {
          _errorDescription = xmlNode.InnerText;
          _hasError = true;
        }
      }

      xmlDoc.RemoveAll();
    }

    public PrivacyAppInsertUpdateResponseData(int result, string pbstrOutput, AtlantisException ex)
    {
      _pbstrOutput = pbstrOutput;
      _result = result;
      _ex = ex;
    }

    public PrivacyAppInsertUpdateResponseData(int result, string pbstrOutput, RequestData oRequestData, Exception ex)
    {
      _pbstrOutput = pbstrOutput;
      _result = result;
      _ex = new AtlantisException(oRequestData, "PrivacyAppInsertUpdateResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }
    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion

  }
}
