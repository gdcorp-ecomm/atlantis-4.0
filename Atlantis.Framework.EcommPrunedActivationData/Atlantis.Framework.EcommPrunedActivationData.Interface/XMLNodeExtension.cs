using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.EcommPrunedActivationData.Interface
{
  public static class XMLNodeExtension
  {
    public static VT GetAttribute<VT>(this XmlNode currentNode, string attributeName, VT defaultValue)
    {
      VT result = defaultValue;
      if (currentNode != null)
      {
        if (currentNode.Attributes != null)
        {
          if (currentNode.Attributes[attributeName] != null)
          {
            try
            {
              result = (VT)Convert.ChangeType(currentNode.Attributes[attributeName].Value, result.GetType());
            }
            catch (System.Exception ex)
            {
              System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
          }
        }
      }
      return result;
    }
  }
}
