using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCancelResource.Interface
{
  public class EcommCancelResourceResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _resultXML = string.Empty;
    private bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public List<string> ErrorMsg { get; private set; }


    public EcommCancelResourceResponseData(string xml)
    {     
      _resultXML = xml;

      var xDoc = new XmlDocument();
      xDoc.LoadXml(xml);
      XmlNode errorNode = xDoc.SelectSingleNode("Status/Error");

      if (errorNode != null)
      {
        ErrorMsg = new List<string>();
        // Skip first two children and last child, grab all others...
        for (int i = 2; i < errorNode.ChildNodes.Count - 1; i++)
        {
          ErrorMsg.Add(string.Format("ID: {0} | ErrorMsg: {1}"
            , errorNode.ChildNodes[i].Attributes["id"].Value, errorNode.ChildNodes[i].Attributes["error"].Value));
        }
      }
      else
      {
        // Successful cancellation returns -- <Status>Success</Status>
        _success = true;
      }
    }

    public EcommCancelResourceResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EcommCancelResourceResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "EcommCancelResourceResponseData",
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
      return _exception;
    }

    #endregion

  }
}
