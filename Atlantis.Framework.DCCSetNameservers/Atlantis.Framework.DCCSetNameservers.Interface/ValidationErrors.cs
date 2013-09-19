using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCSetNameservers.Interface
{
  //<RESPONSE name="ValidateDomainNameservers" result="fail" desc="One or more validation errors"><ERRORS><ERROR objectid="1666955" errortype="Unavailable" desc="Unavailable" field="Nameserver" index="0" /><ERROR objectid="1666955" errortype="Unavailable" desc="Unavailable" field="Nameserver" index="1" /></ERRORS></RESPONSE>


  public class ValidationErrors
  {
    public ValidationErrors(string validationResultXml)
    {
      var resultXml = XDocument.Parse(validationResultXml);

      if (resultXml.Root != null)
      {
        Description = resultXml.Root.Attribute("desc").Value;

        var errorsItems = resultXml.Root.Element("ERRORS");

        if (errorsItems != null)
        {
          var errors = errorsItems.Elements("ERROR");
          var errorsArray = errors as XElement[] ?? errors.ToArray();
          NameserverErrors = new List<IValidationError>(errorsArray.Length);

          foreach (var error in errorsArray)
          {
            NameserverErrors.Add(new NameserverError(error));
          }
        }
      }
    }

    public string Description { get; private set; }
    public IList<IValidationError> NameserverErrors { get; private set; }
  }
}
