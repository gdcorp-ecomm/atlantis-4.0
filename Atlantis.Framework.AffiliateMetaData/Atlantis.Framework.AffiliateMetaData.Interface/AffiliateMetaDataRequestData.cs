﻿using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AffiliateMetaData.Interface
{
  public class AffiliateMetaDataRequestData : RequestData
  {
    #region Properties
    private int _affiliateMetaDataRequestType = 532;
    public int AffiliateMetaDataRequestType
    {
      get { return _affiliateMetaDataRequestType; }
      set { _affiliateMetaDataRequestType = value; }
    }

    public int PrivateLabelId { get; private set; }

    #endregion

    public AffiliateMetaDataRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Empty);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");  
    }
  }
}
