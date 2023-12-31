﻿using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductOfferingFreeOffers.Interface
{
  public class ProductOfferingFreeOffersRequestData : RequestData
  {

    public int ResellerId { get; set; }

    public ProductOfferingFreeOffersRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount, int resellerId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResellerId = resellerId;
    }

    #region Overridden Methods
    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(ResellerId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
    #endregion
  }
}
