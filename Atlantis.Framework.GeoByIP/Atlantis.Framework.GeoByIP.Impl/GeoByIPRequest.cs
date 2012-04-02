using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Atlantis.Framework.GeoByIP.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GeoByIP.Impl
{
  public class GeoByIPRequest : IRequest
  {
    static CountryData _countryData = null;
    static CityData _cityData = null;
    static string _countryErrorData;
    static string _cityErrorData;

    static GeoByIPRequest()
    {
      LoadCountryData();
      LoadCityData();
    }

    static void LoadCityData()
    {
      _cityErrorData = DataFiles.CityFile + ":" + DataFiles.PathType.ToString();
      try
      {
        string filePath = GetFilePath(DataFiles.CityFile);
        _countryData = new CountryData(filePath);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        AtlantisException aex = new AtlantisException("GeoByIP.Impl.LoadCityData", "0", message, _cityErrorData, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    static void LoadCountryData()
    {
      _countryErrorData = DataFiles.CountryFile + ":" + DataFiles.PathType.ToString();
      try
      {
        string filePath = GetFilePath(DataFiles.CountryFile);
        _countryData = new CountryData(filePath);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        AtlantisException aex = new AtlantisException("GeoByIP.Impl.LoadCountryData", "0", message, _countryErrorData, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    static string GetFilePath(string file)
    {
      string result = file;
      if (DataFiles.PathType == DataFilePathTypeEnum.AssemblyLocation)
      {
        result = Path.Combine(AssemblyPath, file);
      }
      else if (DataFiles.PathType == DataFilePathTypeEnum.WebVirtualPath)
      {
        result = HttpContext.Current.ApplicationInstance.Server.MapPath(file);
      }
      return result;
    }

    static string _assemblyPath;
    static string AssemblyPath
    {
      get
      {
        if (_assemblyPath == null)
        {
          Uri pathUri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
          _assemblyPath = pathUri.LocalPath;
        }

        return _assemblyPath;
      }
    }


    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      GeoByIPRequestData request = (GeoByIPRequestData)requestData;

      try
      {
        if (request.LookupType == LookupTypeEnum.City)
        {
          if (_cityData == null)
          {
            throw new ArgumentException("Geo City data was not loaded properly; lookup failed. " + _cityErrorData);
          }

          GeoLocation location = _cityData.GetLocation(request.IpAddress);
          result = new GeoByIPResponseData(location);
        }
        else if (request.LookupType == LookupTypeEnum.Country)
        {
          if (_countryData == null)
          {
            throw new ArgumentException("Geo Country data was not loaded properly; lookup failed. " + _countryErrorData);
          }

          GeoCountry country = _countryData.GetCountry(request.IpAddress);
          result = new GeoByIPResponseData(country);
        }
      }
      catch (Exception ex)
      {
        result = new GeoByIPResponseData(request, ex);
      }

      return result;
    }
  }
}
