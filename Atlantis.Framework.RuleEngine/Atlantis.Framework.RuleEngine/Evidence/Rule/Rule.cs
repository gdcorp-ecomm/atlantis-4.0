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
using System.Collections;
using System.Collections.Generic;
using Atlantis.Framework.RuleEngine.Evidence.Expression;

namespace Atlantis.Framework.RuleEngine.Evidence
{
  public class Rule : AEvidence, IRule
  {
    #region IRule Members
    private readonly bool _chainable;
    private readonly bool _isMainRule;
    protected string Equation { get; set; }
    private readonly List<Symbol> _postfixExpression;
    private readonly List<EvidenceSpecifier> _actions;
    #endregion

    #region constructor
    public Rule(string id, string equation, List<EvidenceSpecifier> actions, int priority, bool chainable, bool isMainRule)
      : base(id, priority)
    {
      if (actions == null || actions.Count < 1)
      {
        throw new Exception("Rules must have at least one action.");
      }

      //foreach (var action in actions)
      //{
      //  if (!action.Truthality && chainable)
      //  {
      //    throw new Exception("Chainable rules are not allowed to contain actions whos result is false.");
      //  }
      //}

      _actions = actions;
      _chainable = chainable;
      _isMainRule = isMainRule;
      Equation = equation;

      var al = new ArrayList();
      foreach (var es in actions)
      {
        al.Add(es.EvidenceId);
      }

      ClauseEvidences = (string[])al.ToArray(typeof(string));

      //this is expensive and static, so compute now
      var e = new ExpressionEvaluator();
      e.Parse(equation); //this method is slow, do it only when needed
      e.InfixToPostfix(); //this method is slow, do it only when needed
      _postfixExpression = e.Postfix; //this method is slow, do it only when needed

      //change event could set its value when a model is attached
      var naked = new Naked(false, typeof(bool));
      EvidenceResult = naked;
    }

    /// <summary>
    /// Constructor used by clone method. Do not use.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public Rule()
    {
    }
    #endregion
    #region core
    protected override int CalculateInternalPriority(int priority)
    {
      int result;

      if (IsChainable)
      {
        result = priority;
      }
      else
      {
        result = 1000 * priority;
      }

      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    public override event ChangedHandler Changed
    {
      add
      {
        base.Changed += value;
      }
      remove
      {
        base.Changed -= value;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public override event EvidenceLookupHandler EvidenceLookup
    {
      add
      {
        base.EvidenceLookup += value;
      }
      remove
      {
        base.EvidenceLookup -= value;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public override event ModelLookupHandler ModelLookup
    {
      add
      {
        base.ModelLookup += value;
      }
      remove
      {
        base.ModelLookup -= value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public bool IsChainable
    {
      get { return _chainable; }
    }

    /// <summary>
    /// 
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public bool IsMainRule
    {
      get { return _isMainRule; }
    }

    public override void Evaluate()
    {
      var e = new ExpressionEvaluator();
      e.GetEvidence += RaiseEvidenceLookup;
      e.Postfix = _postfixExpression;

      var expressionResult = e.Evaluate(); //PERFORMANCE: this method is slow.

      //result must be of this type or the expression is invalid, throw exception
      var resultValue = expressionResult.Value;

      if (expressionResult.SymbolType != ExpressionEvaluator.Type.Invalid)
      {
        //see if its value has changed, if so then set the value and call the events
        if (!base.Value.Equals(resultValue.Value))
        {
          base.Value = resultValue.Value;
          RaiseChanged(this, new ChangedArgs());
        }
      }
      
    }

    protected override IEvidence Value_EvidenceLookup(object sender, EvidenceLookupArgs args)
    {
      return RaiseEvidenceLookup(this, args);
    }

    protected override void Value_Changed(object sender, ChangedArgs args)
    {
      RaiseChanged(this, args);
    }

    protected override Dictionary<string, string> Value_ModelLookup(object sender, ModelLookupArgs e)
    {
      return RaiseModelLookup(this, e);
    }

    public override string[] ClauseEvidence
    {
      get
      {
        var list = new List<string>();

        foreach (EvidenceSpecifier es in _actions)
        {
          if ((bool)base.Value == es.Truthality)
          {
            list.Add(es.EvidenceId);
          }
        }
        return list.ToArray();
      }
    }
    #endregion

  }
}
