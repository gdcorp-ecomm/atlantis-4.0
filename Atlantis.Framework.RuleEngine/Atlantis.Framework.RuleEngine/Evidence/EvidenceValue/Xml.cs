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
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using Atlantis.Framework.RuleEngine.Evidence.Expression;
using Atlantis.Framework.RuleEngine.Model;
using Atlantis.Framework.RuleEngine.Results;

namespace Atlantis.Framework.RuleEngine.Evidence.EvidenceValue
{
  public class Xml : IEvidenceValue
  {
    #region instance variables

    private object previousValue;
    private readonly string _xPath;
    private readonly Type _valueType;
    private readonly string _modelId;
    private readonly string _evidenceValueKey;
    private readonly bool _isResult;

    public event ModelLookupHandler ModelLookup;

    public event ChangedHandler Changed;
    public event EvidenceLookupHandler EvidenceLookup;
    public bool IsXmlValid { get; private set; }

    #endregion

    #region constructor

    //[System.Diagnostics.DebuggerHidden]
    //public Xml(string xPath, Type valueType, string modelId)
    //{
    //  _modelId = modelId;
    //  _xPath = xPath;
    //  _valueType = valueType;

    //  IsXmlValid = !string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(xPath) && _valueType != null;
    //}

    public Xml(string modelId, string modelKey, Type valueType)
    {
      _modelId = modelId;
      _evidenceValueKey = modelKey;
      _valueType = valueType;

      IsXmlValid = !string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(modelKey) && _valueType != null;
    }

    #endregion

    public string EvidenceValueKey
    {
      get { return _evidenceValueKey; }
    }

    #region core

    /// <summary>
    /// 
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public object Value
    {
      get { return previousValue; }
      set
      {
        //if the incoming value is the previous value we have nothing to do
        if (previousValue != value)
        {
          //ResultModel model = ModelLookup(this, new ModelLookupArgs(_modelId));
          Dictionary<string, string> model = ModelLookup(this, new ModelLookupArgs(_modelId));

          //var validationResult = model.Find(m => m.Field == _modelLookupKey);
          SetValue(model, value);
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
      get { return _valueType; }
    }

    private object GetValue(Dictionary<string, string> model)
    {
      string value = string.Empty;

      if (model.TryGetValue(_evidenceValueKey, out value))
      {
        value = model[_evidenceValueKey];
      }
      
      object result = value;

      if (result != null)
      {
        //cast the result to the expected type
        if (_valueType == typeof(string))
        {
          result = result.ToString();
        }
        else if (_valueType == typeof(double))
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
        else if (_valueType == typeof(bool))
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
          throw new Exception("unsupported type: " + typeof(ValueType));
        }
      }

      return result;
    }

    //private static void SetStatus(IValidationResult model, object value)
    //{

    //  model.Status = ValidationResultStatus.NotSet;

    //  bool isValid;
    //  if (bool.TryParse(Convert.ToString(value), out isValid))
    //  {
    //    if (isValid)
    //    {
    //      model.Status = ValidationResultStatus.Valid;
    //    }
    //    else
    //    {
    //      model.Status = ValidationResultStatus.InValid;
    //    }
    //  }
    //}

    private void SetValue(Dictionary<string, string> inputModel, object value)
    {
      if (inputModel.ContainsKey(_evidenceValueKey))
      {
        inputModel[_evidenceValueKey] = Convert.ToString(value);
      }
    }


    /// <summary>
    /// Does the actual work of setting the value of the xpath expression.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    private void SetValue(XmlNode model, object value)
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
    //public void Evaluate()
    //{
    //  XmlNode model = ModelLookup(this, new ModelLookupArgs(_modelId));

    //  object x = getValue(model);

    //  if (x == null)
    //  {
    //    throw new Exception("no value for fact");
    //  }

    //  if (previousValue == null || !x.Equals(previousValue))
    //  {
    //    previousValue = x;
    //    Changed(this, new ChangedModelArgs(_modelId));
    //  }
    //}

    public void Evaluate()
    {
      Dictionary<string, string> model = ModelLookup(this, new ModelLookupArgs(_modelId));
     
      object x = GetValue(model);

      if (x == null)
      {
        x = string.Empty;
        //throw new Exception("no value for fact");
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
      var xml = new Xml(_modelId, _evidenceValueKey, _valueType);
      return xml;
    }

    /// <summary>
    /// 
    /// </summary>
    public string ModelId
    {
      get { return _modelId; }
    }

    public Dictionary<string, string> GetModel(string modelId)
    {
      Dictionary<string, string> model = ModelLookup(this, new ModelLookupArgs(_modelId));
      return model;
    }

    #endregion

    //public void SetStatus()
    //{
    //  Dictionary<string, string> model = ModelLookup(this, new ModelLookupArgs(_modelId));

    //  if (_isResult)
    //  {
    //    var validationResult = model.Find(m => m.Field == _evidenceValueKey);
    //    if (validationResult != null)
    //    {
    //      validationResult.Status = ValidationResultStatus.Valid;

    //      var valueTemp = Convert.ToString(Value);
    //      bool isValid;
    //      if (bool.TryParse(valueTemp, out isValid) && !isValid)
    //      {
    //        validationResult.Status = ValidationResultStatus.InValid;
    //        validationResult.Value = valueTemp;
    //      }
    //    }
    //  }
    //}
  }
}
