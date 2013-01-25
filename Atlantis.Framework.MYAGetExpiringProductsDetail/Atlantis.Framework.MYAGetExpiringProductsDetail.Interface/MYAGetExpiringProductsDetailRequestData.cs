using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAGetExpiringProductsDetail.Interface
{
  public class MYAGetExpiringProductsDetailRequestData : RequestData
  {
    public string ProductTypeListString
    {
      get
      {
        string productTypeList = string.Empty;

        if (ProductTypeHashSet.Count > 0)
        {
          productTypeList = string.Join(",", ProductTypeHashSet);
        }

        return productTypeList;
      }
    }

    private int _days = 90;
    public int Days
    {
      get { return _days; }
      set
      {
        if (value > 0)
        {
          _days = value;
        }
      }
    }

    private int _pagenumber = 1;
    public int PageNumber
    {
      get { return _pagenumber; }
      set
      {
        if (value > 0)
        {
          _pagenumber = value;
        }
      }
    }

    private int _rowsperpage = 1;
    public int RowsPerPage
    {
      get { return _rowsperpage; }
      set
      {
        if (value > 0)
        {
          _rowsperpage = value;
        }
      }
    }

    private string _sortxml = "<orderBy><column sortCol='description' sortDir='ASC'/></orderBy>";
    public string SortXml
    {
      get { return _sortxml; }
      set { _sortxml = value; }
    }

    public bool ReturnAll { get; set; }

    public int SyncableOnly { get; set; }

    private DateTime _iscdate = DateTime.Now;
    public DateTime IscDate
    {
      get
      {
        return _iscdate;
      }
      set
      {
        _iscdate = value;
      }
    }

    private readonly HashSet<string> _productTypeHashSet = new HashSet<string>();
    public HashSet<string> ProductTypeHashSet
    {
      get { return _productTypeHashSet; }
    }

    public MYAGetExpiringProductsDetailRequestData(string shopperId,
                                                   string sourceUrl,
                                                   string orderId,
                                                   string pathway,
                                                   int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      StringBuilder dataBuilder = new StringBuilder();
      dataBuilder.Append(ShopperID);
      dataBuilder.AppendFormat(".{0}", Days);
      dataBuilder.AppendFormat(".{0}", RowsPerPage);
      dataBuilder.AppendFormat(".{0}", SortXml);
      dataBuilder.AppendFormat(".{0}", ReturnAll);
      dataBuilder.AppendFormat(".{0}", SyncableOnly);
      dataBuilder.AppendFormat(".{0}", IscDate.ToString("MM.dd.yyyy"));
      dataBuilder.AppendFormat(".{0}", ProductTypeListString);

      var data = Encoding.UTF8.GetBytes(dataBuilder.ToString());

      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}
