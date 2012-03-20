using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Atlantis.Framework.Auth.Interface
{
  internal class XmlSerializer
  {
    private System.Xml.Serialization.XmlSerializer _xmlSerializer;

    public string Serialize<T>(T objectToSerialize) where T : class
    {
      string serializedObject;
      _xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

      XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
      xmlSerializerNamespaces.Add(string.Empty, string.Empty);

      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
      xmlWriterSettings.OmitXmlDeclaration = true;

      StringWriter stringWriter = new StringWriter();

      using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
      {
        _xmlSerializer.Serialize(xmlWriter, objectToSerialize, xmlSerializerNamespaces);
        serializedObject = stringWriter.ToString();
      }

      return serializedObject;
    }

    public T Deserialize<T>(string serializedObject) where T : class
    {
      T deserializedObject;
      _xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

      using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedObject)))
      {
        deserializedObject = _xmlSerializer.Deserialize(memoryStream) as T;
      }

      return deserializedObject;
    }
  }
}
