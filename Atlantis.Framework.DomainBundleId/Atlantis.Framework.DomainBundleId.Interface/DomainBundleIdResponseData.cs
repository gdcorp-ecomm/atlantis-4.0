using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.DomainBundleId.Interface
{
  public class DomainBundleIdResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties
    private AtlantisException _exception;
    private bool _success;
    public bool IsSuccess
    {
      get { return _success; }
    }

    public bool HasBundleId { get; private set; }

    /// <summary>
    /// Bundle Id or Resource Id of the domain.
    /// </summary>
    public int? BundleId { get; private set; }
    public int? ProductId {get; private set; }
    #endregion

    public DomainBundleIdResponseData(){}

    public DomainBundleIdResponseData(int? bundleId, int? productId)
    {
      if (bundleId != null && bundleId.Value > 0)
      {
        BundleId = bundleId;
        ProductId = productId;
        HasBundleId = true;
      }
      _success = true;
    }
    
    public DomainBundleIdResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public DomainBundleIdResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "DomainBundleIdResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder();

      using (XmlWriter writer = XmlWriter.Create(sb))
      {
        writer.WriteStartElement("bundle");
        writer.WriteAttributeString("bundle_id", BundleId != null ? BundleId.Value.ToString() : string.Empty);
        writer.WriteAttributeString("pf_id", ProductId != null ? ProductId.ToString() : string.Empty);
        writer.WriteEndElement();
      }

      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion


    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      try
      {
        XElement dataElement = XElement.Parse(sessionData);
        foreach (XElement itemElement in dataElement.Descendants("bundle"))
        {
          int itemValue;
          XAttribute itemAttribute = itemElement.Attribute("bundle_id");
          if (itemAttribute != null && int.TryParse(itemAttribute.Value, out itemValue))
          {
            BundleId = itemValue;
          }
          
          itemAttribute = itemElement.Attribute("pf_id");
          if (itemAttribute != null && int.TryParse(itemAttribute.Value, out itemValue))
          {
            ProductId = itemValue;
          }

          _success = true;
          break;
        }
      }
      catch (Exception ex)
      {
        throw new AtlantisException("DomainBundleIdResponseData.DeserializeSessionData", "0", ex.Message + ex.StackTrace, sessionData, null, null);
      }
    }

    #endregion
  }
}
