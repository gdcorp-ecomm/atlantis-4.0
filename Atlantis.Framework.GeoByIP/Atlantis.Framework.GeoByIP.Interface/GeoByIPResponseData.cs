using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GeoByIP.Interface
{
  public class GeoByIPResponseData : IResponseData
  {
    AtlantisException _exception = null;
    GeoLocation _location;
    GeoCountry _country;

    public LookupTypeEnum LookupType { get; private set; }

    public GeoByIPResponseData(GeoCountry country)
    {
      LookupType = LookupTypeEnum.Country;
      _country = country;
    }

    public GeoByIPResponseData(GeoLocation location)
    {
      LookupType = LookupTypeEnum.City;
      _location = location;
    }

    public GeoByIPResponseData(GeoByIPRequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string data = requestData.IpAddress + " : " + requestData.LookupType.ToString();
      _exception = new AtlantisException(requestData, "GeoByResponseData.ctor", message, data);
    }

    public bool CountryFound
    {
      get
      {
        bool result = false;
        if (LookupType == LookupTypeEnum.City)
        {
          result = (_location != null) && (_location.Country != null) && (_location.Country != GeoCountry.UnknownCountry);
        }
        else
        {
          result = (_country != null) && (_country != GeoCountry.UnknownCountry);
        }
        return result;
      }
    }

    public GeoCountry Country
    {
      get
      {
        GeoCountry result = null;
        if (LookupType == LookupTypeEnum.City)
        {
          if (_location != null)
          {
            result = _location.Country;
          }
        }
        else
        {
          result = _country;
        }
        return result;
      }
    }

    public bool LocationFound
    {
      get
      {
        return (LookupType == LookupTypeEnum.City) && (_location != null);
      }
    }

    public GeoLocation Location
    {
      get
      {
        GeoLocation result = null;
        if (LookupType == LookupTypeEnum.City)
        {
          result = _location;
        }
        return result;
      }
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
