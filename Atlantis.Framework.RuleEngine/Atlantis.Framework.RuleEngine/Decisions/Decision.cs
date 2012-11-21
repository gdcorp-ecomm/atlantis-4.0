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

using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.RuleEngine.Evidence;

namespace Atlantis.Framework.RuleEngine.Decisions
{
  /// <remarks>decisiontable or decisiontree for evaluation should be determined by parent in a state design pattern</remarks>
  public class Decision
  {
    protected ExecutionList Executions = new ExecutionList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="evidenceCollection"></param>
    public void Evaluate(Dictionary<string, IEvidence> evidenceCollection)
    {
      #region register all evidences in this rom with this instance of decision
      foreach (var evidence in evidenceCollection.Values)
      {
        evidence.EvidenceLookup += RaiseEvidenceLookup;
        evidence.ModelLookup += RaiseModelLookup;
      }
      #endregion

      #region load up the execution list with facts
      //load up the execution list with facts
      foreach (IEvidence fact in evidenceCollection.Values)
      {
        if (fact is IFact)
        {
          Executions.Add(fact);
          Debug.WriteLine("Added fact to execution list: " + fact.Id);
        }
      }
      #endregion

      #region load up the execution list with chainable rules
      //load up the execution list with main rule
      foreach (IEvidence rule in evidenceCollection.Values)
      {
        if (rule is IRule && ((IRule)rule).IsMainRule)
        {
          Executions.Add(rule);
          Debug.WriteLine("Added rule to execution list: " + rule.Id);
        }
      }
      #endregion

      #region execute list
      //execute list
      Debug.WriteLine("Iteration");
      Debug.IndentLevel++;
      const int maxExecutionCount = 1000; // Max relationships emergency out.
      int executionCount = 0;

      while (Executions.HasNext && executionCount < maxExecutionCount)
      {
        executionCount++;
        Debug.WriteLine("Execution List: " + Executions);
        Debug.WriteLine("Processing");
        Debug.IndentLevel++;
        //evaluate first item on list, it will always be the one of the lowest priority
        string evidenceId = Executions.Read();
        IEvidence evidence = evidenceCollection[evidenceId];
        Debug.WriteLine("EvidenceId: " + evidence.Id);

        //evaluate evidence
        evidence.Evaluate();

        //add its actions, if any, to Executions, for evidence that has clauses
        if (evidence.ClauseEvidence != null)
        {
          foreach (string clauseEvidenceId in evidence.ClauseEvidence)
          {
            var clauseEvidence = evidenceCollection[clauseEvidenceId];
            Executions.Add(clauseEvidence);
            Debug.WriteLine("Added evidence to execution list: " + clauseEvidence.Id);
          }
        }

        Debug.IndentLevel--;
        Debug.WriteLine("End Processing");
        Debug.WriteLine("");
      }
      Debug.IndentLevel--;
      Debug.WriteLine("End Iteration");

      #endregion
      //complete
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return Executions.ToString();
    }

    private event ModelLookupHandler modelLookup;
    private event EvidenceLookupHandler evidenceLookup;
    private event CallbackHandler callbackLookup;


    //[System.Diagnostics.DebuggerHidden]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="args"> </param>
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    //[System.Diagnostics.DebuggerHidden]
    protected virtual Dictionary<string, string> RaiseModelLookup(object sender, ModelLookupArgs args)
    {
      //must always have a model lookup if one is needed
      return modelLookup(sender, args);
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
      return evidenceLookup(sender, args);
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
      callbackLookup(sender, args);
    }


    /// <summary>
    /// 
    /// </summary>
    public virtual event ModelLookupHandler ModelLookup
    {
      add
      {
        modelLookup = value;
      }
      remove
      {
        modelLookup = null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual event EvidenceLookupHandler EvidenceLookup
    {
      add
      {
        evidenceLookup = value;
      }
      remove
      {
        evidenceLookup = null;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public virtual event CallbackHandler CallbackLookup
    {
      add
      {
        callbackLookup = value;
      }
      remove
      {
        callbackLookup = null;
      }
    }


  }
}
