using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAGetExpiringProductsDetail.Interface
{
  public class MYAGetExpiringProductsDetailRequestData : RequestData
  {
    private int _days = 90;
    public int Days
    {
      get
      {
        return _days;
      }
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
      get
      {
        return _pagenumber;
      }
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
      get
      {
        return _rowsperpage;
      }
      set
      {
        if (value > 0)
        {
          _rowsperpage = value;
        }
      }
    }

    private string _sortxml = "<orderBy><column sortCol='description' sortDir='ASC'/></orderBy>";
    public string SortXML
    {
      get
      {
        return _sortxml;
      }
      set
      {
        _sortxml = value;
      }
    }

    private int _returnall = 1;
    public int ReturnAll
    {
      get
      {
        return _returnall;
      }
      set
      {
        _returnall = value;
      }
    }

    private int _syncableonly = 0;
    public int SyncableOnly
    {
      get
      {
        return _syncableonly;
      }
      set
      {
        _syncableonly = value;
      }
    }

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

    private string _producttypeidlist = string.Empty;
    public string ProductTypeIdList
    {
      get
      {
        return _producttypeidlist;
      }
      set
      {
        _producttypeidlist = value;
      }
    }

    public MYAGetExpiringProductsDetailRequestData(
      string shopperID,
      string sourceUrl,
      string orderID,
      string pathway,
      int pageCount)
      : base(shopperID, sourceUrl, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public MYAGetExpiringProductsDetailRequestData(
      string shopperID,
      string sourceUrl,
      string orderID,
      string pathway,
      int pageCount, 
      int days, int pageNumber, int rowsPerPage,
      string sortXml,
      int returnAll, int syncableOnly, DateTime iscDate, string productTypeIdList)
      : base(shopperID, sourceUrl, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      Days = days;
      PageNumber = pageNumber;
      RowsPerPage = rowsPerPage;
      SortXML = sortXml;
      ReturnAll = returnAll;
      SyncableOnly = syncableOnly;
      IscDate = iscDate;
      ProductTypeIdList = productTypeIdList;
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      StringBuilder dataBuilder = new StringBuilder();
      dataBuilder.Append(ShopperID);
      dataBuilder.AppendFormat(".{0}", Days);
      dataBuilder.AppendFormat(".{0}", RowsPerPage);
      dataBuilder.AppendFormat(".{0}", SortXML);
      dataBuilder.AppendFormat(".{0}", ReturnAll);
      dataBuilder.AppendFormat(".{0}", SyncableOnly);
      dataBuilder.AppendFormat(".{0}", IscDate.ToString("MM.dd.yyyy"));
      dataBuilder.AppendFormat(".{0}", ProductTypeIdList);

      var data = Encoding.UTF8.GetBytes(dataBuilder.ToString());

      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}
