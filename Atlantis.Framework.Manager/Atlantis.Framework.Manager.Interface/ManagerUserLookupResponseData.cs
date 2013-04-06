using System;
using Atlantis.Framework.Interface;
using System.Xml.Linq;
using Atlantis.Framework.Manager.Interface.ManagerUser;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerUserLookupResponseData : IResponseData
  {
    public ManagerUserData ManagerUser { get; private set; }
    private readonly AtlantisException _ex;

    public bool IsSuccess
    {
      get { return _ex == null; }
    }

    public static ManagerUserLookupResponseData Empty { get; private set; }

    public ManagerUserLookupResponseData(ManagerUserData managerUser)
    {
      ManagerUser = managerUser;
    }

    public ManagerUserLookupResponseData(RequestData requestData, Exception ex)
    {
      _ex = new AtlantisException(requestData,
                                   "ManagerUserLookupResponseData",
                                   ex.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      var managerUserXml = new XElement("manageruser",
                                             new XAttribute("userid", ManagerUser.UserId),
                                             new XAttribute("fullname", ManagerUser.FullName),
                                             new XAttribute("loginname", ManagerUser.LoginName),
                                             new XAttribute("mstk", ManagerUser.Mstk));

      return managerUserXml.ToString();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
