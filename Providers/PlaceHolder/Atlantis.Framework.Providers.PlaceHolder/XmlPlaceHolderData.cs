using System.Collections.Generic;
using System.Xml.Serialization;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  [XmlRoot(ElementName = "Data")]
  public class XmlPlaceHolderData : IPlaceHolderData
  {
    [XmlAttribute(AttributeName = "location")]
    public string Location { get; set; }

    private IDictionary<string, IPlaceHolderParameter> _parameters;
    public IDictionary<string, IPlaceHolderParameter> Parameters 
    {
      get
      {
        if (_parameters == null)
        {
          _parameters = new Dictionary<string, IPlaceHolderParameter>(ParametersArray.Length);
          foreach (XmlPlaceHolderParameter placeHolderParameter in ParametersArray)
          {
            _parameters[placeHolderParameter.Key] = placeHolderParameter;
          }
        }

        return _parameters;
      }
    }

    [XmlArray(ElementName = "Parameters")]
    [XmlArrayItem(ElementName = "Parameter")]
    public XmlPlaceHolderParameter[] ParametersArray { get; set; }

    public XmlPlaceHolderData()
    {
      Location = string.Empty;
      ParametersArray = new XmlPlaceHolderParameter[0];
    }
  }
}
