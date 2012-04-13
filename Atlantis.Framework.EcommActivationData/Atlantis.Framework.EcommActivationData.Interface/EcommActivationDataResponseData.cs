using System;
using System.Xml;
using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.EcommActivationData.Interface
{
  public class EcommActivationDataResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private bool _isAllActivated = true;

    private List<ProductInfo> _products = new List<ProductInfo>();

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public List<ProductInfo> FreeProducts
    {
      get
      {
        return _products;
      }
    }

    public bool IsAllActivated
    {
      get
      {
        return _isAllActivated;
      }
    }

    public bool IsAllTerminalFailure()
    {
      bool isAllFailed = true;
      foreach (ProductInfo currentProduct in _products)
      {
        foreach (ActivatedProducts activationInfo in currentProduct.ActivatedProducts)
        {
          bool isNotFailed = currentProduct.ActivationStatusID != ProductInfo.ACTIVATION_STATUS_FAILURE;
          if (isNotFailed)
          {
            isAllFailed = false;
            break;
          }
        }
      }
      return isAllFailed;
    }

    public bool IsAllTypeActivated(string activationType)
    {
      bool isAllActive = true;
      foreach (ProductInfo currentProduct in _products)
      {
        foreach (ActivatedProducts activationInfo in currentProduct.ActivatedProducts)
        {
          if (activationInfo.ProductType == activationType)
          {
            bool isActivated = currentProduct.ActivationStatusID == ProductInfo.ACTIVATION_STATUS_ACCOUNT_ACTIVATED || currentProduct.ActivationStatusID == ProductInfo.ACTIVATION_STATUS_BILLING_CONSOLIDATED;
            if (!isActivated)
            {
              isAllActive = false;
              break;
            }
          }
        }        
      }
      return isAllActive;
    }

    public EcommActivationDataResponseData(XmlDocument responseDoc)
    {
      this._success = true;
      if (responseDoc != null)
      {
        this._resultXML = responseDoc.OuterXml;
      }
      XmlNodeList freeProducts= responseDoc.SelectNodes("//Activation/FreeProducts/item");
      if (freeProducts.Count > 0)
      {
        foreach (XmlNode currentNode in freeProducts)
        {
          ProductInfo currentProduct = new ProductInfo(currentNode);
          _products.Add(currentProduct);
          bool isActivated = currentProduct.ActivationStatusID == ProductInfo.ACTIVATION_STATUS_ACCOUNT_ACTIVATED || currentProduct.ActivationStatusID == ProductInfo.ACTIVATION_STATUS_BILLING_CONSOLIDATED;
          if (!isActivated)
          {
            _isAllActivated = false;
          }

        }
      }
      else
      {
        _isAllActivated = false;
      }
    }

    public EcommActivationDataResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;      
    }

    public EcommActivationDataResponseData(string responseXML, AtlantisException atlantisException)
    {
      this._exception = atlantisException;
      this._resultXML = responseXML;
    }

    public EcommActivationDataResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "EcommActivationDataResponseData",
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
