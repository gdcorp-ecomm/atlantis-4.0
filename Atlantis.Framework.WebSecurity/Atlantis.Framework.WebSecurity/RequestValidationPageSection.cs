using System;
using System.Configuration;

namespace Atlantis.Framework.WebSecurity
{
  public class RequestValidationPages : ConfigurationElementCollection
  {
    public override
        ConfigurationElementCollectionType CollectionType
    {
      get
      {
        return

            ConfigurationElementCollectionType.AddRemoveClearMap;
      }
    }

    protected override
        ConfigurationElement CreateNewElement()
    {
      return new PageConfigElement();
    }
    

    protected override Object
        GetElementKey(ConfigurationElement element)
    {
      return ((PageConfigElement)element).Key;
    }


    public new string AddElementName
    {
      get
      { return base.AddElementName; }

      set
      { base.AddElementName = value; }

    }

    public new string ClearElementName
    {
      get
      { return base.ClearElementName; }

      set
      { base.ClearElementName = value; }

    }

    public new string RemoveElementName
    {
      get
      { return base.RemoveElementName; }
    }

    public new int Count
    {
      get { return base.Count; }
    }


    public PageConfigElement this[int index]
    {
      get
      {
        return (PageConfigElement)BaseGet(index);
      }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    new public PageConfigElement this[string Name]
    {
      get
      {
        return (PageConfigElement)BaseGet(Name);
      }
    }

    public int IndexOf(PageConfigElement page)
    {
      return BaseIndexOf(page);
    }

    public void Add(PageConfigElement page)
    {
      BaseAdd(page);
    }

    protected override void
        BaseAdd(ConfigurationElement element)
    {
      BaseAdd(element, false);
    }

    public void Remove(PageConfigElement page)
    {
      if (BaseIndexOf(page) >= 0)
        BaseRemove(page.Name);
    }

    public void RemoveAt(int index)
    {
      BaseRemoveAt(index);
    }

    public void Remove(string name)
    {
      BaseRemove(name);
    }

    public void Clear()
    {
      BaseClear();
    }
  }
  
  /// <summary>
  ///  Define a custom section containing an individual element and a collection of elements.
  /// </summary>
  public class RequestValidationPageSection : ConfigurationSection
  {
    // Declare a collection element represented 
    // in the configuration file by the sub-section
    // <pages> <page .../> </pages> 
    // instructs the .NET Framework to build a nested 
    // section like <pages> ...</pages>.
    [ConfigurationProperty("pages",
        IsDefaultCollection = false)]
    [ConfigurationCollection(typeof(RequestValidationPages), AddItemName = "page")]
    public RequestValidationPages Pages
    {
      get
      {
        var pages = (RequestValidationPages)base["pages"];
        return pages;
      }
    }
    
    protected override string SerializeSection(
        ConfigurationElement parentElement,
        string name, ConfigurationSaveMode saveMode)
    {
      string s =
          base.SerializeSection(parentElement,
          name, saveMode);
      return s;
    }

  }

  public class PageConfigElement : ConfigurationElement
  {
    // Constructor allowing path, source, and key to be specified.
    public PageConfigElement(String path, String source, String name)
    {
      RelativePath = path;
      Source = source;
      Name = name;
      this["key"] = Guid.NewGuid().ToString();
    }

    public PageConfigElement()
    {
      this["key"] = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Dynamically add a key, otherwise there is a possibility that web config attributes might not be unique.
    /// </summary>
    [ConfigurationProperty("key",
       DefaultValue = "",
       IsRequired = true,
       IsKey = true)]
    public string Key
    {

      get
      {
        return (string)this["key"];
      }
      set
      {
        this["key"] = value;
      }

    }

    [ConfigurationProperty("relativePath",
       DefaultValue = "",
       IsRequired = true,
       IsKey = false)]
    public string RelativePath
    {

      get
      {
        return (string)this["relativePath"];
      }
      set
      {
        this["relativePath"] = value;
      }

    }

    
    [ConfigurationProperty("name",
        IsRequired = true,
        IsKey = false)]
    public string Name
    {
      get
      {
        return (string)this["name"];
      }
      set
      {
        this["name"] = value;
      }
    }

    [ConfigurationProperty("source",
        IsRequired = false)]
    public string Source
    {
      get
      {
        return (string)this["source"];
      }
      set
      {
        this["source"] = value;
      }
    }
    

    protected override bool SerializeElement(
        System.Xml.XmlWriter writer,
        bool serializeCollectionKey)
    {
      bool ret = base.SerializeElement(writer,
          serializeCollectionKey);
      return ret;

    }

    protected override bool IsModified()
    {
      bool ret = base.IsModified();
      return ret;
    }
  }
}

