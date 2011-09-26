using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class CharacterFilter
  {
    /// <summary>
    /// Character Filter Does Not Support Greater Than
    /// </summary>
    [DataMember]
    public string FilterType { get; private set; }

    [DataMember]
    public int CharacterCount { get; private set; }

    internal CharacterFilter(XElement charactersElement)
    {
      int characters;
      string charactersValue = charactersElement.Value;
      if (!string.IsNullOrEmpty(charactersValue) && int.TryParse(charactersValue, out characters))
      {
        XAttribute conditionAttribute = charactersElement.Attribute("conditioner");
        string filterValue = conditionAttribute != null ? conditionAttribute.Value : string.Empty;
        if (string.IsNullOrEmpty(filterValue))
        {
          filterValue = NumericFilterType.LessThan;
        }

        FilterType = filterValue;
        CharacterCount = characters;
      }
    }

    /// <summary>
    /// DO NOT use this contructor, meant for serialization only
    /// </summary>
    public CharacterFilter()
    {
    }

    public CharacterFilter(string filterType, int characterCount)
    {
      FilterType = filterType;
      CharacterCount = characterCount;
    }

    internal XElement GetElement()
    {
      XElement charactersElement = new XElement("Characters");
      charactersElement.SetAttributeValue("conditioner", FilterType);
      charactersElement.SetValue(CharacterCount);

      return charactersElement;
    }
  }
}
