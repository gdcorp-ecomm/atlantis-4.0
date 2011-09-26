using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class KeywordFilter
  {
    [DataMember]
    public string FilterType { get; private set; }

    [DataMember]
    public IList<string> KeywordList { get; private set; }

    internal KeywordFilter(XElement keywordsElement)
    {
      string keywordValue = keywordsElement.Value;
      if (!string.IsNullOrEmpty(keywordValue))
      {
        string[] keywords = keywordValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (keywords.Length > 0)
        {
          XAttribute conditionAttribute = keywordsElement.Attribute("conditioner");
          string filterValue = conditionAttribute != null ? conditionAttribute.Value : string.Empty;
          if (string.IsNullOrEmpty(filterValue))
          {
            filterValue = StringFilterType.Contains;
          }

          FilterType = filterValue;
          KeywordList = new List<string>(keywords);
        }
      }
    }

    /// <summary>
    /// DO NOT use this constructor, meant for serialization only
    /// </summary>
    public KeywordFilter()
    {
    }

    public KeywordFilter(string filterType, IList<string> keywords)
    {
      FilterType = filterType;
      KeywordList = keywords;
    }

    internal XElement GetElement()
    {
      XElement keywordsElement = new XElement("Keywords");
      keywordsElement.SetAttributeValue("conditioner", FilterType);
      keywordsElement.SetValue(string.Join(",", KeywordList.ToArray()));

      return keywordsElement;
    }
  }
}