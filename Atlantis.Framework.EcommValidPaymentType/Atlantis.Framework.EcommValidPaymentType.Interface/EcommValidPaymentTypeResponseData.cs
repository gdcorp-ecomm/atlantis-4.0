using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommValidPaymentType.Interface
{
  public class EcommValidPaymentTypeResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    private HashSet<string> _allowedPaymentTypes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    private HashSet<string> _allowedPaymentTypeNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    private IList<PaymentTypeInfo> _allowedPaymentTypeInfos = new List<PaymentTypeInfo>();

    public string ResultXml { get; private set; }

    private readonly bool _isSuccess;
    public bool IsSuccess
    {
      get { return _isSuccess && _atlantisException == null; }
    }


    public EcommValidPaymentTypeResponseData(RequestData requestData, string responseXml, int resultCode)
    {
      ResultXml = responseXml;
      _isSuccess = ParseResponseXml(responseXml);
      if(!_isSuccess)
      {
        _atlantisException = new AtlantisException(requestData,
                                                   MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                   string.Format("Unable to parse response Xml. Result code: {0}. Response XML: {1}", resultCode, responseXml),
                                                   string.Empty);
      }
    }

    public EcommValidPaymentTypeResponseData(RequestData requestData, Exception ex)
    {
      _isSuccess = false;
      _atlantisException = new AtlantisException(requestData,
                                                 MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                 ex.Message + " | " + ex.StackTrace,
                                                 string.Empty);
    }

    private bool ParseResponseXml(string responseXml)
    {
        XmlDocument activePaymentTypesXml = new XmlDocument();
        activePaymentTypesXml.LoadXml(responseXml);

        XmlNodeList paymentType = activePaymentTypesXml.SelectNodes("//PaymentType");

        if (paymentType != null)
        {
          foreach (XmlElement currentType in paymentType)
          {
            string type = currentType.GetAttribute("type");
            string name = currentType.GetAttribute("name");
            if (!string.IsNullOrEmpty(type))
            {
              PaymentTypeInfo newType = new PaymentTypeInfo(type, name);
              _allowedPaymentTypes.Add(type);
              _allowedPaymentTypeInfos.Add(newType);
              _allowedPaymentTypeNames.Add(type.ToLowerInvariant() + "." + name.ToLowerInvariant());
            }
          }
        }

      return _allowedPaymentTypes.Count >= 0;
    }

    public bool IsPaymentTypeAllowed(string paymentType, string paymentName)
    {
      bool result = false;
      if (!string.IsNullOrEmpty(paymentType))
      {
        result = _allowedPaymentTypeNames.Contains(paymentType.ToLowerInvariant() + "." + paymentName.ToLowerInvariant());
      }
      return result;
    }

    public IList<PaymentTypeInfo> GetPaymentTypeInfos(string paymentType)
    {
      IList<PaymentTypeInfo> result = new List<PaymentTypeInfo>();
      foreach (PaymentTypeInfo paymentTypeInfo in _allowedPaymentTypeInfos)
      {
        if (string.Compare(paymentTypeInfo.PaymentType, paymentType, true) == 0)
        {
          result.Add(paymentTypeInfo);
        }
      }
      return result;
    }

    public string ToXML()
    {
      return ResultXml;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }
  }
}
