using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaGetDelegatedRoles.Interface
{
  public class CarmaGetDelegatedRolesResponseData : IResponseData
  {
    #region Properties
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public bool HasTrustedShoppers { get; private set; }
    public List<int> Roles { get; private set; }
    #endregion

    public CarmaGetDelegatedRolesResponseData(string xml)
    {
      _resultXML = xml;
      Roles = new List<int>();

      XDocument xDoc = XDocument.Parse(xml);

      if (xDoc != null)
      {
        var roles = (from role in xDoc.Descendants("Manager") select role.Attribute("roles").Value);
        foreach (var sRolesList in roles)
        {
          var rolesList = sRolesList.Trim();

          // Select each of the specified roles.
          while (rolesList.Length > 0)
          {
            int iCommaPos = rolesList.IndexOf(",", 0);
            string sRole = "";
            if (iCommaPos >= 0)
            {
              sRole = rolesList.Substring(0, iCommaPos);
              rolesList = rolesList.Substring(iCommaPos + 1);
            }
            else
            {
              sRole = rolesList;
              rolesList = "";
            }

            if (sRole.Length > 0)
            {
              Roles.Add(Convert.ToInt32(sRole));
            }

          }
        }
      }

    }

    public bool HasRole(int role)
    {
      return Roles != null && Roles.Contains(role);
    }

     public CarmaGetDelegatedRolesResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

     public CarmaGetDelegatedRolesResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "CarmaGetDelegatedRolesResponseData"
        , exception.Message
        , requestData.ToXML());
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
