using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.CDS.Interface
{
  public class PackageGroupResponseData : CDSResponseData
  {
    public ContentId Id { get; private set; }
    private readonly Dictionary<string, IPackageGroup> _packageGroupsDictionary;
    private readonly Dictionary<string, List<string>> _packageIdsByGroup;   

    public static PackageGroupResponseData NotFound
    {
      get;
      private set;
    }

    static PackageGroupResponseData()
    {
      NotFound = new PackageGroupResponseData();
    }

    public static PackageGroupResponseData FromCDSResponse(string responseData)
    {
      return new PackageGroupResponseData(responseData);
    }
    private PackageGroupResponseData(string responseData) : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      Id = contentVersion._id;      

      if (!string.IsNullOrEmpty(contentVersion.Content))
      {
        _packageIdsByGroup = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        _packageGroupsDictionary = new Dictionary<string, IPackageGroup>(StringComparer.OrdinalIgnoreCase);
        IPackageGroup packageGroup;

        XDocument xDoc = XDocument.Parse(contentVersion.Content);
        
        foreach (XElement packageGroupElement in xDoc.Elements("PackageGroups").Elements("PackageGroup"))
        {
          int productGroupId;

          if (int.TryParse(packageGroupElement.Attribute("ProductGroupId").Value, out productGroupId))
          {
            List<string> packageIds = new List<string>();
            packageGroup = new PackageGroup { ProductGroupId = productGroupId, Name = packageGroupElement.Attribute("Name").Value.ToLowerInvariant() };
            foreach (XElement package in packageGroupElement.Elements("Packages").Elements("Package"))
            {
              _packageGroupsDictionary.Add(package.Attribute("id").Value.ToLowerInvariant(), packageGroup);
              packageIds.Add(package.Attribute("id").Value);
            }
            _packageIdsByGroup.Add(packageGroup.Name, packageIds);
          }
        }
      }
    }
       

    private PackageGroupResponseData()
      : base()
    {
      _packageGroupsDictionary = new Dictionary<string, IPackageGroup>(StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGetValue(string packageGroup, out IEnumerable<string> packageIds)
    {

      List<string> packageIdList;
      bool found = _packageIdsByGroup.TryGetValue(packageGroup, out packageIdList);

      if (!found)
      {
        packageIds = null;
      }      
      else
      {
        packageIds = packageIdList;
      }

      return found;
    }

    public bool TryGetValue(string packageId, out IPackageGroup packageGroup)    
    {
      bool found = _packageGroupsDictionary.TryGetValue(packageId.ToLowerInvariant(), out packageGroup);

      if (!found)
      {
        packageGroup = null;        
      }
      return found;
    }
  }
}