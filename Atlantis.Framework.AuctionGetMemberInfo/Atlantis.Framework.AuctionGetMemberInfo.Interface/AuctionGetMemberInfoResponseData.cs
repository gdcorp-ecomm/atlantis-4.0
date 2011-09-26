using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.AuctionGetMemberInfo.Interface
{
  public class AuctionGetMemberInfoResponseData : IResponseData, ISessionSerializableResponse
  {
    private readonly AtlantisException _ex;
    private string _responseXml = string.Empty;

    public bool IsSuccess { get; set; }

    public AuctionMemberInfo Member { get; set; }

    public AuctionGetMemberInfoResponseData(string responseXml)
    {
      _responseXml = responseXml;

      XDocument doc = XDocument.Parse(responseXml);
      if (doc.Root != null)
      {
        bool boolValue;
        var member = new AuctionMemberInfo();
        bool.TryParse(GetAttributeValue("AdultFilterFlag", doc.Root), out boolValue);
        member.HasAdultFilter = boolValue;

        bool.TryParse(GetAttributeValue("GoodAsGoldEnabled", doc.Root), out boolValue);
        member.IsGoodAsGoldEnabled = boolValue;

        member.MemberId = GetAttributeValue("MemberID", doc.Root) ?? string.Empty;
        member.PrivateLabelId = GetAttributeValue("PrivateLabelID", doc.Root) ?? string.Empty;
        member.ShopperId = GetAttributeValue("ShopperID", doc.Root) ?? string.Empty;

        int intValue;
        int.TryParse(GetAttributeValue("StatusCodeID", doc.Root), out intValue);
        member.StatusCodeId = intValue;

        member.RawXml = _responseXml;

        Member = member;
        IsSuccess = true;
      }
      else
      {
        Member = new AuctionMemberInfo();
        IsSuccess = false;
      }
    }

    static string GetAttributeValue(string attributeName, XElement element)
    {
      var result = string.Empty;
      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        result = attribute.Value;
      }

      return result;
    }

    public AuctionGetMemberInfoResponseData(RequestData requestData, Exception exception)
    {
      _ex = new AtlantisException(requestData, "AuctionGetMemberInfoResponseData", exception.Message, string.Empty, exception);
      IsSuccess = false;
      Member = null;
    }

    /// <summary>
    /// Used only for serialization
    /// </summary>
    public AuctionGetMemberInfoResponseData()
    {
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion

    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      string serialized;

      try
      {
        DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(AuctionMemberInfo));
        using (MemoryStream ms = new MemoryStream())
        {
          json.WriteObject(ms, Member);
          ms.Position = 0;
          serialized = new StreamReader(ms).ReadToEnd();
        }
      }
      catch
      {
        serialized = string.Empty;
      }

      return serialized;
    }

    public void DeserializeSessionData(string sessionData)
    {
      try
      {
        byte[] ba = Encoding.ASCII.GetBytes(sessionData);
        using (MemoryStream ms = new MemoryStream(ba))
        {
          DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(AuctionMemberInfo));
          AuctionMemberInfo result = (AuctionMemberInfo)dcs.ReadObject(ms);
          if (result != null)
          {
            Member = result;
            _responseXml = Member.RawXml;
            IsSuccess = true;
          }
        }
      }
      catch
      {
        Member = null;
        _responseXml = string.Empty;
        IsSuccess = false;
        throw; // Rethrow exception so SessionCache logs the exception
      }
    }


    #endregion
  }
}
