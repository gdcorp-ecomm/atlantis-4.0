using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HDVDGetAccountList.Interface
{
  [DataContract]
  public class HDVDGetAccountListResponseData : IResponseData, ISessionSerializableResponse {

    private readonly AtlantisException _ex;
    private IList<HDVDAccountListItem> _accountList;
    private int _resellerId = -1;
    private int _totalRowCount = -1;

    public HDVDGetAccountListResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    public HDVDGetAccountListResponseData(AtlantisException aex)
    {
      _ex = aex;
      IsSuccess = false;
    }

    public HDVDGetAccountListResponseData(IList<HDVDAccountListItem> accountList, int resellerId, int totalRowCount)
    {
      AccountList = accountList ?? new List<HDVDAccountListItem>();
      ResellerId = resellerId;
      TotalRowCount = totalRowCount;
      IsSuccess = true;
    }

    [DataMember]
    public bool IsSuccess { get; private set; }
    [DataMember]
    public IList<HDVDAccountListItem> AccountList
    {
      get { return _accountList; }
      set { _accountList = value; }
    }
    [DataMember]
    public int ResellerId
    {
      get { return _resellerId; }
      set { _resellerId = value; }
    }
    [DataMember]
    public int TotalRowCount
    {
      get { return _totalRowCount; }
      set { _totalRowCount = value; }
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      string xml;
      try
      {
        var serializer = new DataContractSerializer(this.GetType());
        using (var backing = new System.IO.StringWriter())
        using (var writer = new System.Xml.XmlTextWriter(backing))
        {
          serializer.WriteObject(writer, this);
          xml = backing.ToString();
        }
      }
      catch (Exception)
      {
        xml = string.Empty;
      }
      return xml;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion

    #region Implementation of ISessionSerializableResponse


    public string SerializeSessionData()
    {
      string sessionString = string.Empty;
      MemoryStream ms = new MemoryStream();
      DataContractSerializer ser;

      try
      {
        ser = new DataContractSerializer(this.GetType());
        ser.WriteObject(ms, this.AccountList);
        sessionString = Encoding.Default.GetString(ms.ToArray());
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }

      return sessionString;
    }

    public void DeserializeSessionData(string sessionData)
    {
      MemoryStream ms = null;
      DataContractSerializer ser;

      try
      {
        ms = new MemoryStream(Encoding.Unicode.GetBytes(sessionData));
        ser = new DataContractSerializer(this.GetType());
        AccountList = ser.ReadObject(ms) as IList<HDVDAccountListItem>;
        IsSuccess = true;
        ms.Close();
      }
      finally
      {
        if (ms != null)
        {
          ms.Dispose();
        }
      }
    }


    #endregion
  }
}
