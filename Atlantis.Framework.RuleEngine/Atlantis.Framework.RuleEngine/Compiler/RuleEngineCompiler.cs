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
using Atlantis.Framework.RuleEngine.Evidence;
using Atlantis.Framework.RuleEngine.Evidence.Actions;

namespace Atlantis.Framework.RuleEngine.Compiler
{
  public class RuleEngineCompiler
  {
    public static ROM Compile(XmlDocument rulesXml)
    {
      var rom = CreateRom();
      LoadFacts(rom, rulesXml);
      LoadRules(rom, rulesXml);
      AddFactRelationship(rom);
      return rom;
    }

    private static ROM CreateRom()
    {
      var rom = new ROM();
      return rom;
    }

    static Type GetFactType(string type)
    {
      Type valueType;
      switch (type) //deterrmine the type of value returned by xpath
      {
        case "double":
          valueType = typeof(double);
          break;
        case "boolean":
        case "bool":
          valueType = typeof(bool);
          break;
        case "string":
          valueType = typeof(string);
          break;
        default:
          throw new Exception("Invalid type: " + type);
      }

      return valueType;
    }

    private static void LoadFacts(ROM rom, XmlDocument rulesXml)
    {
      var facts = rulesXml.SelectNodes("RuleEngine/Facts//Fact");

      if (facts != null)
      {
        foreach (XmlNode factNode in facts)
        {
          if (factNode.Attributes != null)
          {
            string id = factNode.Attributes["id"].Value;
            string type = factNode.Attributes["type"].Value;
            string modelId = factNode.Attributes["modelId"].Value;

            //ensure same rule wont be entered twice
            if (rom[id] != null)
            {
              throw new Exception("Fact has already been loaded: " + id);
            }

            //determine priority
            int priority = 500;
            if (factNode.Attributes["priority"] != null)
            {
              priority = Int32.Parse(factNode.Attributes["priority"].Value);
            }

            //determine value type
            var valueType = GetFactType(type);

            var key = factNode.Attributes["key"].Value;

            IFact fact = new Fact(id, priority, key, valueType, modelId, true);

            rom.AddEvidence(fact);
          }
        }
      }
    }

    private static int LoadEvaluation(ROM rom, XmlNode ruleNode, string ruleNodeId, List<EvidenceSpecifier> actions, int actionCounter, ActionType actionType)
    {
      var path = string.Empty;

      switch (actionType)
      {
        case ActionType.EvaluteIsValid:
          path = "Actions//EvaluateIsValid";
          break;
        case ActionType.EvaluateMessage:
          path = "Actions//EvaluateMessage";
          break;
      }

      var evaluateList = ruleNode.SelectNodes(path);
      if (evaluateList != null)
      {
        foreach (XmlNode evaluate in evaluateList)
        {
          try
          {
            if (evaluate.Attributes != null)
            {
              var actionOperatingName = evaluate.Attributes["factId"].Value;
              var evaluationSetValue = evaluate.InnerText;

              var actionPriority = 500;
              if (evaluate.Attributes["priority"] != null)
              {
                actionPriority = Int32.Parse(evaluate.Attributes["priority"].Value);
              }

              var result = true;
              if (evaluate.Attributes["result"] != null)
              {
                result = Boolean.Parse(evaluate.Attributes["result"].Value);
              }

              var actionId = string.Format("{0}-{1}-{2}", ruleNodeId, actionOperatingName, actionCounter++);
              var action = new ActionExpression(actionId, actionOperatingName, evaluationSetValue, actionPriority) { ActionExpressionType = actionType };

              rom.AddEvidence(action);

              actions.Add(new EvidenceSpecifier(result, actionId));
            }
          }
          catch (Exception e)
          {
            throw new Exception("Invalid action: " + evaluate.OuterXml, e);
          }
        }
      }

      return actionCounter;
    }

    private static void LoadRules(ROM rom, XmlDocument rulesXml)
    {
      var rules = rulesXml.SelectNodes("RuleEngine/Rules//Rule");

      if (rules != null)
      {
        foreach (XmlNode ruleNode in rules)
        {
          if (ruleNode.Attributes != null)
          {
            string currentRuleId = ruleNode.Attributes["id"].Value;
            //ensure this has not already been entered
            if (rom[currentRuleId] != null)
            {
              throw new Exception("Rule Ids must be unique: " + currentRuleId);
            }

            // Add a main driver rule to run the only rule in the list.
            if (rules.Count == 1)
            {
              var uniqueName = Guid.NewGuid().ToString();
              var mainActionid = string.Format("{0}-{1}-0", uniqueName, currentRuleId);
              rom.AddEvidence(new ActionExecute(mainActionid, currentRuleId, 500));
              var actionMainList = new List<EvidenceSpecifier> {new EvidenceSpecifier(true, mainActionid)};
              IRule ruleMain = new Rule("mainRule", "true", actionMainList, 1, true);
              rom.AddEvidence(ruleMain);
            }

            bool hasEvidence = false;
            if (ruleNode.Attributes["chainable"] != null)
            {
              hasEvidence = Boolean.Parse(ruleNode.Attributes["chainable"].Value);
            }

            int priority = 500;
            if (ruleNode.Attributes["priority"] != null)
            {
              priority = Int32.Parse(ruleNode.Attributes["priority"].Value);
            }

            //expression
            string condition;
            var conditionNode = ruleNode["Condition"];
            if (conditionNode != null)
            {
              condition = conditionNode.InnerText;
            }
            else
            {
              condition = "true";
            }

            //actions
            int actionCounter = 0;
            var actions = new List<EvidenceSpecifier>();

            #region Evaluate

            actionCounter = LoadEvaluation(rom, ruleNode, currentRuleId, actions, actionCounter, ActionType.EvaluteIsValid);
            actionCounter = LoadEvaluation(rom, ruleNode, currentRuleId, actions, actionCounter, ActionType.EvaluateMessage);

            #endregion

            #region Execute

            var executeList = ruleNode.SelectNodes("Actions//Execute");
            if (executeList != null && executeList.Count > 0)
            {
              hasEvidence = true;
              foreach (XmlNode execute in executeList)
              {
                try
                {
                  if (execute.Attributes != null)
                  {
                    string actionOperatingName = execute.Attributes["factId"].Value;

                    bool result = true;

                    int actionPriority = 500;

                    if (execute.Attributes["priority"] != null)
                    {
                      actionPriority = Int32.Parse(execute.Attributes["priority"].Value);
                    }

                    if (execute.Attributes["result"] != null)
                    {
                      result = Boolean.Parse(execute.Attributes["result"].Value);
                    }

                    var actionId = string.Format("{0}-{1}-{2}", currentRuleId, actionOperatingName, actionCounter++);

                    rom.AddEvidence(new ActionExecute(actionId, actionOperatingName, actionPriority));
                    actions.Add(new EvidenceSpecifier(result, actionId));
                  }
                }
                catch (Exception e)
                {
                  throw new Exception("Invalid action: " + execute.OuterXml, e);
                }
              }
            }

            #endregion
            IRule rule = new Rule(currentRuleId, condition, actions, priority, hasEvidence);
            rom.AddEvidence(rule);
          }
        }

       
      }
    }

    private static void AddFactRelationship(ROM rom)
    {
      //go though each chainable rule and determine who they are dependent on.
      foreach (var evidence in rom.Evidence.Values)
      {
        var rule = evidence as IRule;
        if (rule != null && rule.IsChainable)
        {
          foreach (var depFacts in rule.DependentEvidence)
          {
            rom.AddDependentFact(depFacts, rule.Id);
          }
        }
      }
    }

  }
}
