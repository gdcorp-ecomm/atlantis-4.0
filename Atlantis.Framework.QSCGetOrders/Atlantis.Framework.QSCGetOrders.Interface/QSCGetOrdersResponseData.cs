using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCGetOrders.Interface
{
  [DataContract]
  public class QSCGetOrdersResponseData : IResponseData
  {
    private readonly AtlantisException _ex;
    private IList<orderSummary> _orderList;
    private getOrdersResponseDetail _response;

    // required for session cache
    public QSCGetOrdersResponseData()
    {
    }

    public QSCGetOrdersResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public QSCGetOrdersResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public QSCGetOrdersResponseData(getOrdersResponseDetail response)
    {
      _response = response;
      if (response.orders != null)
      {
        _orderList = response.orders.ToList();
        _resultSize = response.resultSize;
      }
      else
      {
        _orderList = new List<orderSummary>(1);
        _resultSize = 0;
      }
    }

    private QSCStatusCodes responseStatus
    {
      get
      {
        QSCStatusCodes temp;

        if (!Enum.TryParse(_response.responseStatus.statusCode.ToString(), out temp))
        {
          temp = QSCStatusCodes.FAILURE;
        }

        return temp;
      }
    }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get
      {
        _isSuccess = false;
        if (this._response != null)
        {
          _isSuccess = (responseStatus == QSCStatusCodes.SUCCESS);
        }

        return _isSuccess;
      }

      set { _isSuccess = value; }
    }

    private long _resultSize;
    [DataMember]
    public long ResultSize
    {
      get { return _resultSize; }
      set { _resultSize = value; }
    }

    [DataMember]
    public IList<orderSummary> OrderList
    {
      get { return _orderList; }
      set { _orderList = value; }
    }

    [DataMember]
    public getOrdersResponseDetail Response
    {
      get { return _response; }
      set { _response = value; }
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
  }
}
