using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Atlantis.Framework.PresentationCentral.Interface
{
  // **************************************************************** //
  /// <summary>
  /// Class to provide Serialization methods
  /// </summary>
  public static class ObjectSerializer
  {

    // **************************************************************** //
    /// <summary>
    /// Converts a UTF8 byte array to a string
    /// </summary>
    /// <param name="bytes">byteArray</param>
    /// <returns>string</returns>

    public static string UTF8ByteArrayToString(byte[] bytes)
    {
      UTF8Encoding encoding = new UTF8Encoding();
      string constructedString = encoding.GetString(bytes);
      return (constructedString);
    }

    // **************************************************************** //
    /// <summary>
    /// Converts a string to a UTF8
    /// </summary>
    /// <param name="text">text to convert</param>
    /// <returns>byte array</returns>

    public static byte[] StringToUTF8ByteArray(string text)
    {
      UTF8Encoding encoding = new UTF8Encoding();
      byte[] byteArray = encoding.GetBytes(text);
      return byteArray;
    }

    // **************************************************************** //
    /// <summary>
    /// Serializes an object to Xml using an XmlSerializer
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    /// <param name="objectToSerialize">object to serialize</param>
    /// <returns>serialized xml string</returns>

    public static string SerializeToXml<T>(T objectToSerialize)
    {
      return SerializeToXml(objectToSerialize, false);
    }

    // **************************************************************** //
    /// <summary>
    /// Serializes an object to Xml using an XmlSerializer
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    /// <param name="objectToSerialize">object to serialize</param>
    /// <param name="omitXmlDeclaration">if true the xml declaration will be omitted from the xml</param>
    /// <returns>serialized xml string</returns>

    public static string SerializeToXml<T>(T objectToSerialize, bool omitXmlDeclaration)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = omitXmlDeclaration;
        settings.Encoding = Encoding.UTF8;

        XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings);

        XmlSerializer xs = new XmlSerializer(typeof(T));
        xs.Serialize(xmlWriter, objectToSerialize);
        string result = UTF8ByteArrayToString(memoryStream.ToArray());
        return result;
      }
    }

    // **************************************************************** //
    /// <summary>
    /// Deserialize xml into an object
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    /// <param name="xmlToDeserialize">Xml to deserialize</param>
    /// <returns>deserialized object</returns>

    public static T DeserializeXml<T>(string xmlToDeserialize)
    {
      XmlSerializer xs = new XmlSerializer(typeof(T));
      using (MemoryStream memoryStream =
                    new MemoryStream(StringToUTF8ByteArray(xmlToDeserialize)))
      {
        return (T)xs.Deserialize(memoryStream);
      }
    }
  }

}
