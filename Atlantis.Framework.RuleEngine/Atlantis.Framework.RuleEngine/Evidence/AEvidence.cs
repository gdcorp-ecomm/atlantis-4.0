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
  /// <summary>
  /// 
  /// </summary>
  public abstract class AEvidence : IEvidence
  {
    #region instance variables
    private event ModelLookupHandler _modelLookup;
    private event ChangedHandler _changed;
    private event EvidenceLookupHandler _evidenceLookup;
    private event CallbackHandler _callbackLookup;

    protected string[] ClauseEvidences { get; set; }

    #endregion

    #region constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="priority"> </param>
    //[System.Diagnostics.DebuggerHidden]
    protected AEvidence(string id, int priority)
    {
      if (priority >= 1000 || priority <= 0)
      {
        throw new Exception("Evidence Priority must be greater than 0 and less than 1000. It was: " + priority);
      }


// ReSharper disable DoNotCallOverridableMethodsInConstructor
// Develpers Note about Resharper warning: Resharper is warning about virtual calls in a constructor; however, this is an abstract memeber so we don't have to worry about the base class 
// changing the Priority.
      priority = CalculateInternalPriority(priority); //the evidence implementing this class must determine its true Priority with respect to evidences
// ReSharper restore DoNotCallOverridableMethodsInConstructor

      Id = id;
      Priority = priority;
    }

    /// <summary>
    /// Constructor. For use by clone method, must override and make public.
    /// </summary>
    protected AEvidence(){}

    #endregion
    #region core
    #region events
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
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
    public virtual event CallbackHandler CallbackLookup
    {
      add
      {
        _callbackLookup = value;
      }
      remove
      {
        _callbackLookup = null;
      }
    }
    #endregion

    protected abstract int CalculateInternalPriority(int priority);


    /// <summary>
    /// Identified of the evidence
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public string Id { get; private set; }


    private IEvidenceValue _value;/// <summary>
    /// Contains the IRuleEngineComparable object that contains the _value for this evidence
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public virtual object Value
    {
      get
      {
        if (!_isEvaluatable)
        {
          throw new Exception("This fact currently is not evaluatable, it has no _value: " + Id);
        }
        
        return _value.Value;
      }
      set
      {
        if (!_isEvaluatable)
        {
          throw new Exception("This fact currently is not evaluatable, it has no _value: " + Id);
        }

        _value.Value = value;
      }
    }

    /// <summary>
    /// Contains the IRuleEngineComparable object that contains the _value for this evidence
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public virtual Type ValueType
    {
      get
      {
        return _value.ValueType;
      }
    }

    public IEvidenceValue ValueObject
    {
      get { return _value; }
    }

    /// <summary>
    /// IRuleEngineComparable thats responsible for the _value
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    internal IEvidenceValue EvidenceResult
    {
      get
      {
        return _value;
      }
      set
      {
        _value = value;

        _value.ModelLookup += Value_ModelLookup;
        _value.Changed += Value_Changed;
        _value.EvidenceLookup += Value_EvidenceLookup;
      }
    }

    /// <summary>
    /// Evidence, typically Actions, that are the clause statement to its evaluated conditional.
    /// Does not have a _value until its been evaluated.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public abstract string[] ClauseEvidence
    {
      get;
    }

    /// <summary>
    /// Executes and sets truthality of evidence. Will clear previous _value for new _value.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public abstract void Evaluate();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    public virtual int CompareTo(object obj)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    private bool _isEvaluatable;
    /// <summary>
    /// Whether or not this evidence is evaluatable or even has a _value.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public bool IsEvaluatable
    {
      get
      {
        return _isEvaluatable;
      }
      set
      {
        _isEvaluatable = value;
      }
    }

    public int Priority { get; private set; }

    #region eventhandlers

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    //[System.Diagnostics.DebuggerHidden]
    protected virtual void RaiseChanged(object sender, ChangedArgs args)
    {      
      if (_changed != null)
      {
        _changed(sender, args);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    protected virtual Dictionary<string, string> RaiseModelLookup(object sender, ModelLookupArgs args)
    {
      //must always have a model lookup if one is needed
      return _modelLookup(sender, args);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    protected virtual IEvidence RaiseEvidenceLookup(object sender, EvidenceLookupArgs args)
    {
      //must always have an evidence lookup if one is needed.
      return _evidenceLookup(sender, args);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    protected virtual void RaiseCallback(object sender, CallbackArgs args)
    {
      _callbackLookup(sender, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    protected abstract IEvidence Value_EvidenceLookup(object sender, EvidenceLookupArgs args);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected abstract void Value_Changed(object sender, ChangedArgs args);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    protected abstract Dictionary<string, string> Value_ModelLookup(object sender, ModelLookupArgs e);
    #endregion
    #endregion

    #region IEvidence Members

    #endregion
  }

}
