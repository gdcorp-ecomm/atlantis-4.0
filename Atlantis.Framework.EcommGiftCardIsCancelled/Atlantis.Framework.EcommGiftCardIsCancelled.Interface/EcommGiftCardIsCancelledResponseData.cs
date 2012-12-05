using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardIsCancelled.Interface
{
  public class EcommGiftCardIsCancelledResponseData : IResponseData
  {
    private AtlantisException _atlException;

    public bool IsSuccess { get; private set;}
    private DataSet _ds { get; set; }
  
    public EcommGiftCardIsCancelledResponseData(DataSet ds)
    {
      IsSuccess = true;
      _ds = ds;
    }

    public EcommGiftCardIsCancelledResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "EcommGiftCardIsCancelledResponseData", ex.Message, string.Empty);
    }

    public bool IsGiftCardCancelled
    {
      get
      {
        bool gcCancelled = false;
        if (_ds != null && _ds.Tables.Count > 0)
        {
          if (_ds.Tables[0].Rows.Count == 0)
          {
            gcCancelled = true;
          }
          else
          {
            int status = _ds.Tables[0].Rows[0].IsNull("gdshop_billing_statusID") ? 4 : (int)_ds.Tables[0].Rows[0]["gdshop_billing_statusID"];
            if (status == 4)
            {
              gcCancelled = true;
            }
          }
        }
        return gcCancelled;
      }
    }


    #region Implementation of IResponseData

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, _ds);
      
      return writer.ToString();

    }

    public AtlantisException GetException()
    {
      return _atlException;
    }

    #endregion
  }
}
