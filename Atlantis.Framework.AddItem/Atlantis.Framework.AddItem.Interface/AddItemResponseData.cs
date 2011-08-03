using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddItem.Interface
{
  public class AddItemResponseData : IResponseData
  {
    private readonly string m_sResponseXML;
    private readonly AtlantisException m_ex;

    public AddItemResponseData(string responseXml)
    {
      m_sResponseXML = responseXml;
      m_ex = null;
    }

    public AddItemResponseData(string responseXml, AtlantisException exAtlantis)
    {
      m_sResponseXML = responseXml;
      m_ex = exAtlantis;
    }

    public AddItemResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      m_sResponseXML = responseXml;
      m_ex = new AtlantisException(requestData, 
                                   "AddItemResponseData", 
                                   ex.Message, 
                                   requestData.ToXML());
    }

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return m_ex;
    }

    public string ToXML()
    {
      return m_sResponseXML;
    }

    #endregion
  }
}
