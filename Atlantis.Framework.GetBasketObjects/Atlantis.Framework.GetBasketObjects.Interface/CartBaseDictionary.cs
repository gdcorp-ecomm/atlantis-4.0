using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  /// <summary>
  /// Summary description for BaseDictionary
  ///   Implement ISerializable and IXMLSerializable for custom serialization
  ///   All Derived classes need only to be marked with the serializable attribute
  ///   They will then be serialized as an XML string
  /// </summary>
  [Serializable]
  public class CartBaseDictionary : Dictionary<string, string>, ISerializable
  {
    protected CartBaseDictionary(SerializationInfo si,
      StreamingContext context)
      : base(si, context)
    {

    }

    /// <summary>
    /// Returns a property in a string dictionary as an int value.
    /// This extension method is defined in <typeparamref name="StringDictionaryExtensions"/>
    /// </summary>
    /// <param name="key">key to search for.</param>
    /// <exception cref="ArgumentException">Thrown if key is not found or value is not a valid integer.</exception>
    /// <returns>Int value found.</returns>
    public int GetIntProperty(string key)
    {
      int result;
      string stringValue;
      if (!TryGetValue(key, out stringValue))
      {
        throw new ArgumentException("Key " + key + " not found in dictionary.");
      }
      else
      {
        if (!Int32.TryParse(stringValue, out result))
        {
          throw new ArgumentException("Value for key " + key + " not a valid integer.");
        }
      }
      return result;
    }

    /// <summary>
    /// Returns a property in a string dictionary as an int value, or a default value.
    /// This extension method is defined in <typeparamref name="StringDictionaryExtensions"/>
    /// </summary>
    /// <param name="key">key to search for.</param>
    /// <param name="defaultValue">default value to return if <paramref name="key"/> is not found or value is not a valid integer.</param>
    /// <returns>Int value found or <paramref name="defaultValue"/>.</returns>
    public int GetIntProperty(string key, int defaultValue)
    {
      int result;
      string stringValue;
      if (!TryGetValue(key, out stringValue))
      {
        result = defaultValue;
      }
      else
      {
        if (!Int32.TryParse(stringValue, out result))
        {
          decimal testValue = 0;
          if (!decimal.TryParse(stringValue, out testValue))
          {
            result = defaultValue;
          }
          else
          {
            result = (int)testValue;
          }
        }
      }
      return result;
    }

    /// <summary>
    /// Returns a property in a string dictionary as a double value.
    /// This extension method is defined in <typeparamref name="StringDictionaryExtensions"/>
    /// </summary>
    /// <param name="key">key to search for.</param>
    /// <exception cref="ArgumentException">Thrown if key is not found or value is not a valid double.</exception>
    /// <returns>Double value found.</returns>
    public double GetDoubleProperty(string key)
    {
      double result;
      string stringValue;
      if (!TryGetValue(key, out stringValue))
      {
        throw new ArgumentException("Key " + key + " not found in dictionary.");
      }
      else
      {
        if (!Double.TryParse(stringValue, out result))
        {
          throw new ArgumentException("Value for key " + key + " not a valid double.");
        }
      }
      return result;
    }

    /// <summary>
    /// Returns a property in a string dictionary as an double value, or a default value.
    /// This extension method is defined in <typeparamref name="StringDictionaryExtensions"/>
    /// </summary>
    /// <param name="key">key to search for.</param>
    /// <param name="defaultValue">default value to return if <paramref name="key"/> is not found or value is not a valid double.</param>
    /// <returns>Double value found or <paramref name="defaultValue"/>.</returns>
    public double GetDoubleProperty(string key, double defaultValue)
    {
      double result;
      string stringValue;
      if (!TryGetValue(key, out stringValue))
      {
        result = defaultValue;
      }
      else
      {
        if (!Double.TryParse(stringValue, out result))
        {
          result = defaultValue;
        }
      }
      return result;
    }

    /// <summary>
    /// Returns a property in a string dictionary as an string value, or a default value.
    /// This extension method is defined in <typeparamref name="StringDictionaryExtensions"/>
    /// </summary>
    /// <param name="key">key to search for.</param>
    /// <param name="defaultValue">default value to return if <paramref name="key"/> is not found.</param>
    /// <returns>String value found or <paramref name="defaultValue"/>.</returns>
    public string GetStringProperty(string key, string defaultValue)
    {
      string result = defaultValue;
      if (!TryGetValue(key, out result))
      {
        result = defaultValue;
      }
      return result;
    }

    public bool GetBoolProperty(string propertyName, bool defaultValue)
    {
      bool boolProp = false;
      string temp = GetStringProperty(propertyName, string.Empty);
      if (string.IsNullOrEmpty(temp))
      {
        boolProp = defaultValue;
      }
      else
      {
        if (!bool.TryParse(temp, out boolProp))
        {
          if (temp == "1")
            boolProp = true;
          else
            boolProp = false;
        }
      }
      return boolProp;
    }

    public void LoadValues(Dictionary<string, string> newValues)
    {
      this.Clear();
      foreach (KeyValuePair<string, string> currentEntry in newValues)
      {
        Add(currentEntry.Key, currentEntry.Value);
      }
    }

    public CartBaseDictionary()
    {

    }

    public CartBaseDictionary Clone()
    {
      CartBaseDictionary cloneData = new CartBaseDictionary();
      foreach (KeyValuePair<string, string> currentEntry in this)
      {
        cloneData[currentEntry.Key] = currentEntry.Value;
      }
      return cloneData;
    }

    #region ISerializable Members

    #endregion
  }
}
