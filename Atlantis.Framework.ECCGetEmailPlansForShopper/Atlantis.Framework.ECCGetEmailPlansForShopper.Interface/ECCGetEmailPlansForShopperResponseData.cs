using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.ECCGetEmailPlansForShopper.Interface
{
	public class ECCGetEmailPlansForShopperResponseData : EccResponseDataBase<EccEmailPlan>, ISessionSerializableResponse
	{
    public List<EccEmailPlan> EmailPlans
	  {
	    get
	    {
        if (Response != null)
        {
          if (Response.Item != null)
          {
            if (Response.Item.Results != null)
              return Response.Item.Results;
          }
        }
	      return null;
	    }
	  }
    
	  public ECCGetEmailPlansForShopperResponseData(string resultJson) : base(resultJson)
    {
    }


	  public ECCGetEmailPlansForShopperResponseData(AtlantisException atlantisException) : base(atlantisException)
	  {
	  }

	  public ECCGetEmailPlansForShopperResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
	  {
	  }

	  public ECCGetEmailPlansForShopperResponseData() : base()
	  {
	  }

	  #region Implementation of ISessionSerializableResponse

	  public string SerializeSessionData()
	  {
      var sb = new StringBuilder();
      XmlWriter xmlWriter = null;
      xmlWriter = XmlWriter.Create(sb);

      if (_fault == null)
      {
        var ser = new DataContractSerializer(Response.GetType());
        ser.WriteObject(xmlWriter, Response);
      }

      xmlWriter.Flush();
      xmlWriter.Close();

	    return sb.ToString();
	  }

	  public void DeserializeSessionData(string sessionData)
	  {
      var ms = new MemoryStream(Encoding.Unicode.GetBytes(sessionData));
      DataContractSerializer ser;

      try
      {
        ser = new DataContractSerializer(typeof(EccJsonResponse<EccEmailPlan>));
        Response = ser.ReadObject(ms) as EccJsonResponse<EccEmailPlan>;
        if (Response != null)
        {
          IsSuccess = (Response.Item.ResultCode == 0 ? true : false);
        }
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }
	  }

	  #endregion
	}
}
