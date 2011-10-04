using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegGetDotTypeProductIdList.Interface
{
  public class RegGetDotTypeProductIdListRequestData : RequestData
  {
    #region Properties

    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    private string _dotTypeName;
    public string DotTypeName
    {
      get { return _dotTypeName; }
      set { _dotTypeName = value; }
    }

    #endregion Properties

    #region Constructors

    public RegGetDotTypeProductIdListRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount,
                                         string dotTypeName)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this._dotTypeName = dotTypeName;
      RequestTimeout = _requestTimeout;
    }

    #endregion Constructors

    #region Public Methods

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));
      xtwRequest.WriteStartElement("request");
      xtwRequest.WriteAttributeString("tldname", this._dotTypeName);
      xtwRequest.WriteAttributeString("plgrouptype", "0");
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods
  }
}
