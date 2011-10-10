using System.Xml;

namespace Atlantis.Framework.DCCGetExpirationCount.Interface
{
  public class ExpirationDomainCountsResult
  {
    XmlElement _resultElement = null;

    internal ExpirationDomainCountsResult(XmlElement resultElement)
    {
      _resultElement = resultElement;
    }

    public string GetResultAttribute(string name)
    {
      return _resultElement.GetAttribute(name);
    }

    private int GetIntResultAttribute(string name, int defaultValue)
    {
      int result = defaultValue;
      string resultText = GetResultAttribute(name);
      int parseResult;
      if (int.TryParse(resultText, out parseResult))
      {
        result = parseResult;
      }
      return result;
    }

    public string ShopperId
    {
      get { return GetResultAttribute("shopperid"); }
    }

    public bool IsValid
    {
      get
      {
        int compare = string.Compare("success", Processing, true);
        return (compare == 0);
      }
    }

    public int TotalDomains
    {
      get 
      {
        return GetIntResultAttribute("totaldomains", 0);
      }
    }

    public int AlreadyExpiredDomains
    {
      get 
      {
        return GetIntResultAttribute("alreadyexpireddomains", 0);
      }
    }

    public string Processing
    {
      get 
      {
        return GetResultAttribute("processing");
      }
    }

    public int ExpiringDomains
    {
      get 
      {
        return GetIntResultAttribute("expiringdomains", 0);
      }
    }
  }
}
