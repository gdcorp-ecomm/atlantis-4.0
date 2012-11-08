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

namespace Atlantis.Framework.RuleEngine.Evidence.EvidenceValue
{
  /// <summary>
  /// Comparable object that has no reference to the model
  /// </summary>
  public class Naked : IEvidenceValue
  {
    #region instance variables
    private object value;
    private readonly Type _valueType;
    private event ChangedHandler _changed;
    #endregion

    #region constructor

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueType"></param>
    ////[System.Diagnostics.DebuggerHidden]
    public Naked(object value, Type valueType)
    {
      this.value = value;
      _valueType = valueType;
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
        return value;
      }
      set
      {
        if (this.value == null || !this.value.Equals(value))
        {
          this.value = value;
          _changed(this, new ChangedArgs());
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
    /// 
    /// </summary>
    public void Evaluate()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    public object Clone()
    {
      var xml = new Naked(value, _valueType);
      return xml;
    }


// ReSharper disable EventNeverInvoked
    private event ModelLookupHandler _modelLookup;
// ReSharper restore EventNeverInvoked
    public virtual event ModelLookupHandler ModelLookup
    {
      add
      {
        _modelLookup = value;
      }
      remove
      {
        _modelLookup = null;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public virtual event ChangedHandler Changed
    {
      add
      {
        _changed = value;
      }
      remove
      {
        _changed = null;
      }
    }

// ReSharper disable EventNeverInvoked
    private event EvidenceLookupHandler _evidenceLookup;
// ReSharper restore EventNeverInvoked
    public virtual event EvidenceLookupHandler EvidenceLookup
    {
      add
      {
        _evidenceLookup = value;
      }
      remove
      {
        _evidenceLookup = null;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public string ModelId
    {
      get
      {
        return null;
      }
    }
    #endregion


    public void SetResult(IFact fact)
    {
      throw new NotImplementedException();
    }


    public void SetStatus(Expression.Symbol result)
    {
      throw new NotImplementedException();
    }


    public Dictionary<string, string> GetModel(string modelId)
    {
      throw new NotImplementedException();
    }


    public string EvidenceValueKey
    {
      get { throw new NotImplementedException(); }
    }
  }
}
