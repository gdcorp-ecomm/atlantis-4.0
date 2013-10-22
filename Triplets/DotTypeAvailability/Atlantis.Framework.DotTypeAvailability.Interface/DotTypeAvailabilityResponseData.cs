using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  [KnownType(typeof(TldAvailability))]
  [KnownType(typeof(TldPhase))]
  [DataContract]
  public class DotTypeAvailabilityResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _xmlData;

    private readonly Dictionary<string, ITldAvailability> _tldAvailabilityDictionary;

    [DataMember]
    public IDictionary<string, ITldAvailability> TldAvailabilityDictionary
    {
      get
      {
        return _tldAvailabilityDictionary;
      }
    }

    public static DotTypeAvailabilityResponseData FromException(RequestData requestData, Exception ex)
    {
      return new DotTypeAvailabilityResponseData(requestData, ex);
    }

    private DotTypeAvailabilityResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "DotTypeAvailabilityResponseData.ctor", message, inputData);
    }

    public static DotTypeAvailabilityResponseData FromTldAvailabilityDictionary(IDictionary<string, ITldAvailability> tldAvailabilityDictionary)
    {
      return new DotTypeAvailabilityResponseData(tldAvailabilityDictionary);
    }

    private DotTypeAvailabilityResponseData(IDictionary<string, ITldAvailability> tldAvailabilityDictionary)
    {
      _tldAvailabilityDictionary = tldAvailabilityDictionary as Dictionary<string, ITldAvailability>;
      _xmlData = GetTldAvailabilityXml(tldAvailabilityDictionary);
    }

    private static string GetTldAvailabilityXml(IDictionary<string, ITldAvailability> tldAvailabilityDictionary)
    {
      string result = tldAvailabilityDictionary.ToString();

      return result;
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_tldAvailabilityDictionary != null)
      {
        try
        {
          var serializer = new DataContractSerializer(this.GetType());
          using (var backing = new System.IO.StringWriter())
          using (var writer = new System.Xml.XmlTextWriter(backing))
          {
            serializer.WriteObject(writer, this);
            result = backing.ToString();
          }
        }
        catch (Exception ex)
        {
          result = string.Empty;
        }
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
