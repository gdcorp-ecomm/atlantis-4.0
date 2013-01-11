using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLRegistration : TLDMLNamespaceElement
  {
    private int _minRegistrationYears;
    private int _maxRegistrationYears;

    internal TLDMLRegistration(XDocument tldmlDocument)
      : base(tldmlDocument)
    {
      _minRegistrationYears = GetExpectedPeriodValue("minregistrationperiod", "year", 1);
      _maxRegistrationYears = GetExpectedPeriodValue("maxregistrationperiod", "year", 10);
    }

    protected override string Namespace
    {
      get { return "urn:godaddy:ns:registration-1.0"; }
    }

    protected override string LocalName
    {
      get { return "registration"; }
    }

    public int MinRegistrationYears
    {
      get { return _minRegistrationYears; }
    }

    public int MaxRegistrationYears
    {
      get { return _maxRegistrationYears; }
    }
  }
}
