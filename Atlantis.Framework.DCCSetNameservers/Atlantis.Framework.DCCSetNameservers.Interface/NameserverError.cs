
using System.Xml.Linq;

namespace Atlantis.Framework.DCCSetNameservers.Interface
{
  //<RESPONSE name="ValidateDomainNameservers" result="fail" desc="One or more validation errors"><ERRORS><ERROR objectid="1666955" errortype="Unavailable" desc="Unavailable" field="Nameserver" index="0" /><ERROR objectid="1666955" errortype="Unavailable" desc="Unavailable" field="Nameserver" index="1" /></ERRORS></RESPONSE>


  public class NameserverError : IValidationError
  {
    public NameserverError(XElement nameserverErrorElement)
    {
      if (nameserverErrorElement != null)
      {
        _id = nameserverErrorElement.Attribute("objectid").Value;
        _name = nameserverErrorElement.Attribute("field").Value;
        _description = nameserverErrorElement.Attribute("desc").Value;
        _errorType = nameserverErrorElement.Attribute("errortype").Value; 
      }
    }

    private readonly string _id = string.Empty;
    public string Id
    {
      get { return _id; }
    }

    private readonly string _name = string.Empty;
    public string Name
    {
      get { return _name; }
    }

    private readonly string _description = string.Empty;
    public string Description
    {
      get { return _description; }
    }

    private readonly string _errorType = string.Empty;
    public string ErrorType
    {
      get { return _errorType; }
    }
  }
}
