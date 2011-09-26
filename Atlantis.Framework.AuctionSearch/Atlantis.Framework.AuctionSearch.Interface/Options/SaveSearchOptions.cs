using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class SaveSearchOptions
  {
    [DataMember]
    public string SearchName { get; private set; }

    [DataMember]
    public string ActionType { get; private set; }

    [DataMember]
    public bool EmailResults { get; set; }

    /// <summary>
    /// DO NOT use this contructor, meant for serialization only
    /// </summary>
    public SaveSearchOptions()
    {
    }

    public SaveSearchOptions(string searchName, string actionType)
    {
      SearchName = searchName;
      ActionType = actionType;
    }

    internal XElement GetElement()
    {
      XElement savedSearchElement = new XElement("SavedSearch");
      savedSearchElement.SetAttributeValue("SearchName", SearchName);
      if (EmailResults)
      {
        savedSearchElement.SetAttributeValue("EmailResults", EmailResults);
      }
      savedSearchElement.SetAttributeValue("Action", ActionType);

      return savedSearchElement;
    }
  }
}
