using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProfileGetById.Interface;
using Atlantis.Framework.BuyerProfileDetails.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProfileGetById.Impl
{
  public class BuyerProfileGetByIdRequest : IRequest
  {
    private const string PROC_NAME = "mya_GetBuyerProfileDetails_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProfileGetByIdRequestData request = (BuyerProfileGetByIdRequestData)oRequestData;

      DataSet ds = null;
      ProfileDetail _details = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@profile_id", request.ProfileID));
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));

            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }
        _details = GetBuyerProfileDetail(ds);
        oResponseData = new BuyerProfileGetByIdResponseData(_details);
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProfileGetByIdResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private ProfileDetail GetBuyerProfileDetail(DataSet buyerProfileDetail)
    {
      ProfileDetail _details = null;
      int regLength = 0;

      foreach (DataRow dr in buyerProfileDetail.Tables[0].Rows)
      {
        string profileName = dr["profileName"] == DBNull.Value ? "" : dr["profileName"].ToString().Trim();
        int.TryParse(Convert.ToString(dr["registrationYears"]), out regLength);
        bool autoRenew = dr["autoRenew"] == DBNull.Value ? false : dr["autoRenew"].ToString().Trim() == "1" ? true : false;
        bool parkDns = dr["parkDNS"] == DBNull.Value ? false : dr["parkDNS"].ToString().Trim() == "1" ? true : false;
        bool defaultProfileFlag = dr["defaultProfileFlag"] == DBNull.Value ? false : dr["defaultProfileFlag"].ToString().Trim() == "1" ? true : false;
        bool quickCheckoutFlag = dr["quickCheckoutFlag"] == DBNull.Value ? false : dr["quickCheckoutFlag"].ToString().Trim() == "1" ? true : false;

        int contactTypeId = -1;
        List<AddressList> addressList = new List<AddressList>();
        foreach (DataRow address in buyerProfileDetail.Tables[1].Rows)
        {
          int.TryParse(Convert.ToString(address["gdshop_domainContactTypeID"]), out contactTypeId);
          string contactTypeShort = address["contactTypeShort"] == DBNull.Value ? "" : address["contactTypeShort"].ToString().Trim();
          string contactTypeDesc = address["contactTypeDesc"] == DBNull.Value ? "" : address["contactTypeDesc"].ToString().Trim();
          string firstName = address["firstName"] == DBNull.Value ? "" : address["firstName"].ToString().Trim();
          string middleName = address["middleName"] == DBNull.Value ? "" : address["middleName"].ToString().Trim();
          string lastName = address["LastName"] == DBNull.Value ? "" : address["LastName"].ToString().Trim();
          string organization = address["organization"] == DBNull.Value ? "" : address["organization"].ToString().Trim();
          string address1 = address["address1"] == DBNull.Value ? "" : address["address1"].ToString().Trim();
          string address2 = address["address2"] == DBNull.Value ? "" : address["address2"].ToString().Trim();
          string city = address["city"] == DBNull.Value ? "" : address["city"].ToString().Trim();
          string stateOrProvince = address["stateOrProvince"] == DBNull.Value ? "" : address["stateOrProvince"].ToString().Trim();
          string zipCode = address["zipCode"] == DBNull.Value ? "" : address["zipCode"].ToString().Trim();
          string country = address["country"] == DBNull.Value ? "" : address["country"].ToString().Trim();
          string daytimePhone = address["daytimePhone"] == DBNull.Value ? "" : address["daytimePhone"].ToString().Trim();
          string eveningPhone = address["eveningPhone"] == DBNull.Value ? "" : address["eveningPhone"].ToString().Trim();
          string fax = address["fax"] == DBNull.Value ? "" : address["fax"].ToString().Trim();
          string email = address["email"] == DBNull.Value ? "" : address["email"].ToString().Trim();

          addressList.Add(new AddressList(contactTypeId, contactTypeShort, contactTypeDesc, firstName,
              middleName, lastName, organization, address1, address2, city, stateOrProvince, zipCode,
              country, daytimePhone, eveningPhone, fax, email));

        }

        List<string> hostList = new List<string>();
        foreach (DataRow host in buyerProfileDetail.Tables[2].Rows)
        {
          string hostName = host["hostName"] == DBNull.Value ? "" : host["hostName"].ToString().Trim();
          hostList.Add(hostName);
        }

        _details = new ProfileDetail(profileName, regLength, autoRenew, parkDns, defaultProfileFlag, quickCheckoutFlag, addressList, hostList);

      }

      return _details;
    }
  }


}
