using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class StateNamesResponseData : LanguageNamesResponseData
  {
    public static StateNamesResponseData Empty { get; private set; }

    static StateNamesResponseData()
    {
      Empty = new StateNamesResponseData();
    }

    public static StateNamesResponseData FromServiceData(string serviceDataXml)
    {
      var stateNamesById = new Dictionary<int, string>();

      // <LocaleData lang="pt-br"><Item id="74" state="Alabama"/>etc

      var data = XElement.Parse(serviceDataXml);
      var stateElements = data.Descendants("Item");

      foreach (var stateElement in stateElements)
      {
        var stateId = stateElement.GetAttributeValueInt("id", -1);
        var name = stateElement.GetAttributeValue("state", null);

        if ((stateId != -1) && (name != null))
        {
          stateNamesById[stateId] = name;
        }
      }

      return new StateNamesResponseData(stateNamesById);
    }

    private StateNamesResponseData()
    {
    }

    private StateNamesResponseData(Dictionary<int, string> namesByStateId)
      : base(namesByStateId)
    {
    }
  }
}
