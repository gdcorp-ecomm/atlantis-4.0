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

namespace Atlantis.Framework.RuleEngine.Evidence
{
  public class EvidenceValue : IEvidenceValue
  {
    
    #region instance variables

    private object previousValue;
    private readonly Type _valueType;
    private readonly string _modelId;
    private readonly string _evidenceValueKey;

    public event ModelLookupHandler ModelLookup;

    public event ChangedHandler Changed;
    public event EvidenceLookupHandler EvidenceLookup;
    #endregion

    #region constructor

    public EvidenceValue(string modelId, string modelKey, Type valueType)
    {
      _modelId = modelId;
      _evidenceValueKey = modelKey;
      _valueType = valueType;
    }

    #endregion

    public string EvidenceValueKey
    {
      get { return _evidenceValueKey; }
    }

    #region core

    //[System.Diagnostics.DebuggerHidden]
    public object Value
    {
      get { return previousValue; }
      set
      {
        //if the incoming value is the previous value we have nothing to do
        if (previousValue != value)
        {
          Dictionary<string, string> model = ModelLookup(this, new ModelLookupArgs(_modelId));
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
      string value;

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
    
    private void SetValue(Dictionary<string, string> inputModel, object value)
    {
      if (inputModel.ContainsKey(_evidenceValueKey))
      {
        inputModel[_evidenceValueKey] = Convert.ToString(value);
      }
    }

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
  }
}
