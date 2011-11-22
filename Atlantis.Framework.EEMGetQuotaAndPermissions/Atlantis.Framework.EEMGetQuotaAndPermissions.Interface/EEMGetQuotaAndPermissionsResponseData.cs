using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetQuotaAndPermissions.Interface
{
  public class EEMGetQuotaAndPermissionsResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public int Quota { get; private set; }
    public int Permissions { get; private set; }
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public EEMGetQuotaAndPermissionsResponseData(int quota, int permissions)
    {
      Quota = quota;
      Permissions = permissions;
    }

     public EEMGetQuotaAndPermissionsResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EEMGetQuotaAndPermissionsResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "EEMGetQuotaAndPermissionsResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      XElement result = new XElement("eem",
        new XAttribute("quota", Quota.ToString()),
        new XAttribute("permissions", Permissions.ToString()));

      return result.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
