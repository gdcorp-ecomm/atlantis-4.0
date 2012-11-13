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
  public class Fact : AEvidence, IFact
  {
    #region instance variables
    #endregion
    #region constructor

    /// <summary>
    /// Construct Fact. 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="priority"> </param>
    /// <param name="value"> </param>
    /// <param name="valueType"> </param>
    //[System.Diagnostics.DebuggerHidden]
    public Fact(string id, int priority, object value, Type valueType)
      : base(id, priority)
    {

      //naked
      IEvidenceValue x = new Naked(value, valueType);
      EvidenceResult = x;
    }

    public Fact(string id, int priority, string key, Type valueType, string modelId, bool isValid)
      : base(id, priority)
    {
      //xml
      IEvidenceValue x = new EvidenceValue(modelId, key, valueType);
      EvidenceResult = x;
      IsValid = isValid;
      _key = key;
    }

    /// <summary>
    /// Constructor used by clone method. Do not use.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public Fact()
    {
    }
    #endregion
    #region core

    /// <summary>
    /// 
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public override void Evaluate()
    {
      if (IsEvaluatable)
      {
        EvidenceResult.Evaluate();
      }
    }

    protected override int CalculateInternalPriority(int priority)
    {
      return 1000 * 1000 * 1000 * priority;
    }

    public override string[] ClauseEvidence
    {
      get { return null; }
    }

    #endregion
    #region events
    //[System.Diagnostics.DebuggerHidden]
    protected override IEvidence Value_EvidenceLookup(object sender, EvidenceLookupArgs args)
    {
      return RaiseEvidenceLookup(this, args);
    }

    //[System.Diagnostics.DebuggerHidden]
    protected override void Value_Changed(object sender, ChangedArgs args)
    {
      RaiseChanged(this, args);
      //UpdateResults();
    }

    //[System.Diagnostics.DebuggerHidden]
    protected override Dictionary<string, string> Value_ModelLookup(object sender, ModelLookupArgs e)
    {
      return RaiseModelLookup(this, e);
    }
    #endregion
    
    public bool IsValid { get; set; }

    public string _messages;
    public string Messages
    {
      get
      {
        if (_messages == null)
        {
          _messages = string.Empty;
        }

        return _messages;
      }
      set { _messages = value; }
    }
    
    public string InputValue { get; set; }

    public void UpdateResults()
    {
      {
        var resultValue = EvidenceResult;
        Value = resultValue.Value;
      }
    }

    private readonly string _key;
    public string Key
    {
      get { return _key; }
    }
  }
}
