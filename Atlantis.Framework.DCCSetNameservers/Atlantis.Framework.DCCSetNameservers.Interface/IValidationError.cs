using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.DCCSetNameservers.Interface
{
  //<RESPONSE name="ValidateDomainNameservers" result="fail" desc="One or more validation errors"><ERRORS><ERROR objectid="1666955" errortype="Unavailable" desc="Unavailable" field="Nameserver" index="0" /><ERROR objectid="1666955" errortype="Unavailable" desc="Unavailable" field="Nameserver" index="1" /></ERRORS></RESPONSE>


  public interface IValidationError
  {
    string Id { get; }
    string Name { get; }
    string Description { get; }
    string ErrorType { get; }
  }
}
