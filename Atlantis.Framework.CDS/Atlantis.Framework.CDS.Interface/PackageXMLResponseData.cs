using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.CDS.Interface
{
  public class PackageXMLResponseData : CDSResponseData
  {
    public ContentId Id { get; private set; }
    private readonly Dictionary<string, XDocument> _packagesDictionary;
    
    public static PackageXMLResponseData NotFound
    {
      get;
      private set;
    }

    static PackageXMLResponseData()
    {
      NotFound = new PackageXMLResponseData();
    }

    public static PackageXMLResponseData FromCDSResponse(string responseData)
    {
      return new PackageXMLResponseData(responseData);
    }
    private PackageXMLResponseData(string responseData) : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      Id = contentVersion._id;      

      if (!string.IsNullOrEmpty(contentVersion.Content))
      {
        _packagesDictionary = new Dictionary<string, XDocument>(StringComparer.OrdinalIgnoreCase);
        
        XDocument xDoc = XDocument.Parse(contentVersion.Content);
        
        foreach (XElement packageElement in xDoc.Elements("Packages").Elements("Package"))
        {
          var packageId = packageElement.Attribute("id").Value.ToLowerInvariant();
          _packagesDictionary[packageId] = XDocument.Parse(packageElement.ToString());
        }       
      }
    }
       

    private PackageXMLResponseData()
      : base()
    {
      _packagesDictionary = new Dictionary<string, XDocument>();
    }

    public bool TryGetValue(string packageId, out XDocument packageDoc)    
    {
      bool found = _packagesDictionary.TryGetValue(packageId.ToLowerInvariant(), out packageDoc);
      if (!found)
      {
        packageDoc = null;        
      }
      return found;
    }
  }
}