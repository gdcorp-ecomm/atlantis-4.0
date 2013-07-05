using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  [XmlRoot(ElementName = "Data")]
  public class XmlPlaceHolderData : IPlaceHolderData
  {
    private readonly XmlDataSerializer _xmlDataSerializer = new XmlDataSerializer();

    [XmlAttribute(AttributeName = "id")]
    public string Id { get; set; }

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

    public XmlPlaceHolderData(string location, IDictionary<string, string> parameters)
    {
      Location = location;
      ParametersArray = GetParametersArray(parameters);
    }

    public XmlPlaceHolderData()
    {
      Location = string.Empty;
      ParametersArray = new XmlPlaceHolderParameter[0];
    }

    private XmlPlaceHolderParameter[] GetParametersArray(IDictionary<string, string> parameters)
    {
      XmlPlaceHolderParameter[] parametersArray;

      if (parameters == null)
      {
        parametersArray = new XmlPlaceHolderParameter[0];
      }
      else
      {
        IList<XmlPlaceHolderParameter> parameterList = new List<XmlPlaceHolderParameter>(parameters.Count);
        foreach (string parameterKey in parameters.Keys)
        {
          XmlPlaceHolderParameter parameter = new XmlPlaceHolderParameter { Key = parameterKey, Value = parameters[parameterKey] };
          parameterList.Add(parameter);
        }

        parametersArray = parameterList.ToArray();
      }


      return parametersArray;
    }

    public string ToXml()
    {
      string xml = string.Empty;

      try
      {
        xml = _xmlDataSerializer.Serialize(this);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("XmlPlaceHolderData.ToXml()",
                                                      "0",
                                                      "Unable to serialize XmlPlaceHolderData",
                                                      ex.StackTrace,
                                                      null,
                                                      null);

        Engine.Engine.LogAtlantisException(aex);
      }

      return xml;
    }
  }
}
