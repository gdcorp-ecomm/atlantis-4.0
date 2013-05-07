using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class XmlDataSerializer
  {
    private XmlSerializer _xmlSerializer;

    public string Serialize<T>(T objectToSerialize) where T : class
    {
      string serializedObject;
      _xmlSerializer = new XmlSerializer(typeof(T));

      XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
      xmlSerializerNamespaces.Add(string.Empty, string.Empty);

      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
      xmlWriterSettings.OmitXmlDeclaration = true;

      StringWriter stringWriter = new StringWriter();

      try
      {
        using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
        {
          _xmlSerializer.Serialize(xmlWriter, objectToSerialize, xmlSerializerNamespaces);
          serializedObject = stringWriter.ToString();
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Unable to serialize {0}. {1}", objectToSerialize == null ? "unknown type" : objectToSerialize.GetType().FullName, ex.Message));
      }

      return serializedObject;
    }

    public T Deserialize<T>(string serializedObject) where T : class
    {
      T deserializedObject = default(T);

      _xmlSerializer = new XmlSerializer(typeof(T));

      try
      {
        using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedObject)))
        {
          deserializedObject = _xmlSerializer.Deserialize(memoryStream) as T;
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Unable to deserialize {0}. {1}", deserializedObject == null ? "unknown type" : deserializedObject.GetType().FullName, ex.Message));
      }

      return deserializedObject;
    }
  }
}
