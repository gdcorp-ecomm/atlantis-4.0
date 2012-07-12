﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;


namespace Atlantis.Framework.ReceiptSurveyTypesGet.Interface
{
  public class ReceiptSurveyTypesGetRequestData: RequestData
  {
    public string CountryCode { get; private set; }

    //oth is the default values
    public ReceiptSurveyTypesGetRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string countryCode = "oth") :
      base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      CountryCode = countryCode;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(CountryCode);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
