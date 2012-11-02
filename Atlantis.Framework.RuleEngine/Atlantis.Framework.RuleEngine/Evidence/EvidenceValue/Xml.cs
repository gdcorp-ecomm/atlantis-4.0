/*
Simple Rule Engine
Copyright (C) 2005 by Sierra Digital Solutions Corp

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Xml;
using System.Xml.XPath;

namespace Atlantis.Framework.RuleEngine.Evidence.EvidenceValue
{
  public class Xml : IEvidenceValue
  {
    #region instance variables
    private object previousValue;
    private readonly string _xPath;
    private readonly Type _valueType;
    private readonly string _modelId;
    public event ModelLookupHandler ModelLookup;
    public event ChangedHandler Changed;
    public event EvidenceLookupHandler EvidenceLookup;
    public bool IsXmlValid { get; private set; }
    #endregion
    #region constructor
    //[System.Diagnostics.DebuggerHidden]
    public Xml(string xPath, Type valueType, string modelId)
    {
      _modelId = modelId;
      _xPath = xPath;
      _valueType = valueType;

      IsXmlValid = !string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(xPath) && _valueType != null;
    }
    #endregion
    #region core
    /// <summary>
    /// 
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public object Value
    {
      get
      {
        return previousValue;
      }
      set
      {
        //if the incoming value is the previous value we have nothing to do
        if (previousValue != value)
        {
          var model = ModelLookup(this, new ModelLookupArgs(_modelId));

          setValue(model, value);
          previousValue = value;
          Changed(this, new ChangedModelArgs(_modelId));
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public Type ValueType
    {
      get
      {
        return _valueType;
      }
    }

    /// <summary>
    /// Does the actual work of getting the value of the xpath expression.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    private object getValue(XmlNode model)
    {
      object result = null;

      if (!string.IsNullOrEmpty(_xPath))
      {
        //XPathNavigator requires an xmldocument to operate on, a model is not required therefore we must supply an empty one
        if (model == null)
        {
          model = new XmlDocument().DocumentElement;
        }

        //execute the xpath
        try
        {
          //Create XPathNavigator
          if (model != null)
          {
            XPathNavigator xpathNav = model.CreateNavigator();

            //Compile the XPath expression
            XPathExpression xpathExpr = xpathNav.Compile(_xPath);

            //Display the results depending on type of result
            switch (xpathExpr.ReturnType)
            {
              case XPathResultType.Number:
              case XPathResultType.Boolean:
              case XPathResultType.String:
                result = xpathNav.Evaluate(xpathExpr);
                break;
              case XPathResultType.NodeSet:
                XPathNodeIterator nodeIter = xpathNav.Select(xpathExpr);
                nodeIter.MoveNext();
                result = ((IHasXmlNode) nodeIter.Current).GetNode();
                break;
              case XPathResultType.Error:
                break;
            }
          }
        }
        catch
        {
          result = null;
        }

        //returned nothing
        if (result is XmlNode && !_valueType.IsAssignableFrom(typeof (XmlNode)))
        {
          var node = (XmlNode) result;
          switch (node.NodeType)
          {
            case XmlNodeType.Attribute:
              result = node.Value;
              break;
            case XmlNodeType.Element:
              result = node.InnerText;
              break;
            case XmlNodeType.Text:
              result = node.Value;
              break;
            default:
              throw new Exception(String.Format("Node type '{0}' not implemented", node.NodeType));
          }
        }

        if (result != null)
        {
          //cast the result to the expected type
          if (_valueType == typeof (string))
          {
            result = result.ToString();
          }
          else if (_valueType == typeof (double))
          {
            if (result.ToString() == String.Empty)
            {
              result = 0; // no value means default value
            }
            else
            {
              result = Double.Parse(result.ToString());
            }
          }
          else if (_valueType == typeof (bool))
          {
            if (result.ToString() == String.Empty)
            {
              result = false; //no value means default value
            }
            else
            {
              result = Boolean.Parse(result.ToString());
            }
          }
          else
          {
            throw new Exception("unsupported type: " + typeof (ValueType));
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Does the actual work of setting the value of the xpath expression.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    private void setValue(XmlNode model, object value)
    {
      try
      {
        //empty string is equivalent to null
        if (_xPath == "" || model == null)
          return;

        //suppress all events being processed or else we will end up in an infinite loop
        XmlNode y = model.SelectSingleNode(_xPath);

        if (_valueType.IsAssignableFrom(typeof(XmlNode)))
        {
          throw new Exception("Type of XmlNode cannot be set.");
        }

        if (y != null)
        {
          switch (y.NodeType)
          {
            case XmlNodeType.Element:
              y.InnerText = value.ToString();
              break;
            case XmlNodeType.Attribute:
              y.Value = value.ToString();
              break;
            case XmlNodeType.Text:
              y.Value = value.ToString();
              break;
            default:
              throw new Exception("Not a supported node type.");
          }
        }
      }
      catch (Exception e)
      {
        throw new Exception("Invalid _xPath: " + _xPath, e);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void Evaluate()
    {
      XmlNode model = ModelLookup(this, new ModelLookupArgs(_modelId));

      object x = getValue(model);

      if (x == null)
      {
        throw new Exception("no value for fact");
      }

      if (previousValue == null || !x.Equals(previousValue))
      {
        previousValue = x;
        Changed(this, new ChangedModelArgs(_modelId));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    public object Clone()
    {
      var xml = new Xml(_xPath, _valueType, _modelId);
      return xml;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public string ModelId
    {
      get
      {
        return _modelId;
      }
    }
    #endregion
  }
}
