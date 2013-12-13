using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Atlantis.Framework.CDS.Interface
{
  public class PackageJSONResponseData : CDSResponseData
  {
    private readonly Dictionary<string, string> _packageJsonDictionary;

    public ContentId Id { get; private set; }
    
    public static PackageJSONResponseData NotFound
    {
      get;
      private set;
    }

    static PackageJSONResponseData()
    {
      NotFound = new PackageJSONResponseData();
    }

    public static PackageJSONResponseData FromCDSResponse(string responseData)
    {
      return new PackageJSONResponseData(responseData);
    }
    private PackageJSONResponseData(string responseData) : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      Id = contentVersion._id;

      var packageJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentVersion.Content);
      _packageJsonDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      foreach (KeyValuePair<string, object> keyValuePair in packageJson)
      {
        _packageJsonDictionary[keyValuePair.Key] = keyValuePair.Value.ToString().Replace("\\\"", "\"");
      }
      
    }
       
    private PackageJSONResponseData()
      : base()
    {
      _packageJsonDictionary = new Dictionary<string, string>();
    }

    public bool TryGetValue(string packageId, out string packageJson)    
    {
      bool found = _packageJsonDictionary.TryGetValue(packageId.ToLowerInvariant(), out packageJson);
      if (!found)
      {
        packageJson = null;        
      }
      return found;
    }
  }
}