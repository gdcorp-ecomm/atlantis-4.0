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
using System.Diagnostics;
using System.Xml;
using Atlantis.Framework.RuleEngine.Evidence.EvidenceValue;

namespace Atlantis.Framework.RuleEngine.Evidence.Actions
{
  public class ActionExpression : AEvidence, IAction
  {
    #region instance variables

    protected string Equation { get; set; }
    private ExpressionEvaluator _evaluator;
    private string _operatingId;
    #endregion
    #region constructor
    public ActionExpression(string ID, string operatingId, string equation, int priority)
      : base(ID, priority)
    {
      Equation = equation;
      _operatingId = operatingId;

      //determine the dependent facts
      var e = new ExpressionEvaluator();
      e.Parse(equation); //this method is slow, do it only when needed

      string[] dependents = ExpressionEvaluator.RelatedEvidence(e.Infix);
      DependentEvidenceItems = dependents;

      //assing a value
      var naked = new Naked(null, typeof(string));
      EvidenceValue = naked;

      //this is expensive and static, so compute now
      _evaluator = new ExpressionEvaluator();
      _evaluator.Parse(equation); //this method is slow, do it only when needed
      _evaluator.InfixToPostfix(); //this method is slow, do it only when needed
      _evaluator.GetEvidence += RaiseEvidenceLookup;
    }

    /// <summary>
    /// Constructor used by clone method. Do not use.
    /// </summary>
    //[System.Diagnostics.DebuggerHidden]
    public ActionExpression()
    {
    }
    #endregion
    #region core
    protected override int CalculateInternalPriority(int priority)
    {
      return 1000 * 1000 * priority;
    }

    public override void Evaluate()
    {
      if (!IsEvaluatable)
      {
        throw new Exception("This action cannot currently be evaluated.");
      }

      var result = _evaluator.Evaluate();

      //throw exception up stack if an error was present
      if (result.SymbolType == ExpressionEvaluator.Type.Invalid)
      {
        throw new Exception(String.Format("Invalid expression for action:{0}, equation:{1}", Id, Equation));
      }

      //get the fact that should be assigned too, throw exception if IFact is not returned
      var fact = RaiseEvidenceLookup(this, new EvidenceLookupArgs(_operatingId)) as IFact;

      if (fact == null)
      {
        throw new Exception(String.Format("operatingId was not of type IFact: {0}", _operatingId));
      }

      //set the value
      Trace.WriteLine("FACT " + _operatingId + "=" + result.Value.Value);

      fact.Value = result.Value.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    protected override IEvidence Value_EvidenceLookup(object sender, EvidenceLookupArgs args)
    {
      return RaiseEvidenceLookup(this, args);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected override void Value_Changed(object sender, ChangedArgs args)
    {
      RaiseChanged(this, args);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    protected override XmlNode Value_ModelLookup(object sender, ModelLookupArgs e)
    {
      return RaiseModelLookup(this, e);
    }
    /// <summary>
    /// 
    /// </summary>
    public override string[] ClauseEvidence
    {
      get
      {
        return new[] { _operatingId };
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    public override object Clone()
    {
      var f = (ActionExpression)base.Clone();
      f.Equation = Equation;
      f._operatingId = _operatingId;
      f._evaluator = _evaluator;
      return f;
    }
    #endregion

  }
}
