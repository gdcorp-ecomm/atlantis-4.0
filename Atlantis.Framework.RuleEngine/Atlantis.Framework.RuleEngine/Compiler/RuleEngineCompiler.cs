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
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Atlantis.Framework.RuleEngine.Evidence;
using Atlantis.Framework.RuleEngine.Evidence.Actions;

namespace Atlantis.Framework.RuleEngine.Compiler
{
  public class RuleEngineCompiler
  {
    public static ROM Compile(XDocument rulesXml)
    {
      var rom = CreateRom();
      LoadFacts(rom, rulesXml);
      LoadRules(rom, rulesXml);
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

    private static void LoadFacts(ROM rom, XDocument rulesXml)
    {
      var facts = rulesXml.XPathSelectElements("RuleEngine/Facts/Fact");
      foreach (XElement factNode in facts)
      {
        if (factNode.HasAttributes)
        {
          string id = factNode.Attribute("id").Value;
          string type = factNode.Attribute("type").Value;
          string modelId = factNode.Attribute("modelId").Value;

          //ensure same rule wont be entered twice
          if (rom[id] != null)
          {
            throw new Exception("Fact has already been loaded: " + id);
          }

          //determine priority
          int priority = 500;
          if (factNode.Attribute("priority") != null)
          {
            priority = Int32.Parse(factNode.Attribute("priority").Value);
          }

          //determine value type
          var valueType = GetFactType(type);

          var key = factNode.Attribute("key").Value;

          IFact fact = new Fact(id, priority, key, valueType, modelId, true);

          rom.AddEvidence(fact);
        }
      }
    }
    private static int LoadEvaluation(ROM rom, XElement ruleNode, string ruleNodeId, List<EvidenceSpecifier> actions, int actionCounter, ActionType actionType)
    {
      var path = string.Empty;

      switch (actionType)
      {
        case ActionType.EvaluteIsValid:
          path = "./Actions/EvaluateIsValid";
          break;
        case ActionType.EvaluateMessage:
          path = "./Actions/EvaluateMessage";
          break;
        case ActionType.EvaluateValue:
          path = "./Actions/EvaluateValue";
          break;
      }

      var evaluateList = ruleNode.XPathSelectElements(path);
      var xElements = evaluateList as XElement[] ?? evaluateList.ToArray();
      if (xElements.Any())
      {
        foreach (XElement evaluate in xElements)
        {
          try
          {
            if (evaluate.HasAttributes)
            {
              var actionOperatingName = evaluate.Attribute("factId").Value;
              var evaluationSetValue = evaluate.Value;

              var actionPriority = 500;
              if (evaluate.Attribute("priority") != null)
              {
                actionPriority = Int32.Parse(evaluate.Attribute("priority").Value);
              }

              var result = true;
              if (evaluate.Attribute("result") != null)
              {
                result = Boolean.Parse(evaluate.Attribute("result").Value);
              }

              var actionId = string.Format("{0}-{1}-{2}", ruleNodeId, actionOperatingName, actionCounter++);
              var action = new ActionExpression(actionId, actionOperatingName, evaluationSetValue, actionPriority) { ActionExpressionType = actionType };

              rom.AddEvidence(action);

              actions.Add(new EvidenceSpecifier(result, actionId));
            }
          }
          catch (Exception e)
          {
            throw new Exception("Invalid action: " + evaluate, e);
          }
        }
      }

      return actionCounter;
    }

    private static void LoadRules(ROM rom, XDocument rulesXml)
    {
      var ruleMainNode = rulesXml.XPathSelectElements("RuleEngine/Rules/RuleMain").FirstOrDefault();
      if (ruleMainNode != null)
      {
        LoadRule(rom, ruleMainNode, true);
      }
      else
      {
        throw new Exception("RuleMain node is missing.");
      }

      var rules = rulesXml.XPathSelectElements("RuleEngine/Rules/Rule");
      foreach (XElement ruleNode in rules)
      {
        LoadRule(rom, ruleNode, false);
      }
    }

    private static void LoadRule(ROM rom, XElement ruleElement, bool isMain)
    {
      if (ruleElement.HasAttributes)
      {
        string currentRuleId = ruleElement.Attribute("id").Value;
        //ensure this has not already been entered
        if (rom[currentRuleId] != null)
        {
          throw new Exception("Rule Ids must be unique: " + currentRuleId);
        }

        bool hasEvidence = false;
        if (ruleElement.Attribute("chainable") != null)
        {
          hasEvidence = Boolean.Parse(ruleElement.Attribute("chainable").Value);
        }

        int priority = 500;
        if (ruleElement.Attribute("priority") != null)
        {
          priority = Int32.Parse(ruleElement.Attribute("priority").Value);
        }

        //expression
        string condition;
        var conditionNode = ruleElement.XPathSelectElement("./Condition");
        if (conditionNode != null)
        {
          condition = conditionNode.Value;
        }
        else
        {
          condition = "true";
        }

        //actions
        int actionCounter = 0;
        var actions = new List<EvidenceSpecifier>();

        #region Evaluate

        actionCounter = LoadEvaluation(rom, ruleElement, currentRuleId, actions, actionCounter, ActionType.EvaluteIsValid);
        actionCounter = LoadEvaluation(rom, ruleElement, currentRuleId, actions, actionCounter, ActionType.EvaluateMessage);
        actionCounter = LoadEvaluation(rom, ruleElement, currentRuleId, actions, actionCounter, ActionType.EvaluateValue);

        #endregion

        #region Execute

        var executeList = ruleElement.XPathSelectElements("./Actions/Execute");
        var xElements = executeList as XElement[] ?? executeList.ToArray();
        if (xElements.Any())
        {
          hasEvidence = true;
          foreach (XElement execute in xElements)
          {
            try
            {
              if (execute.HasAttributes)
              {
                string actionOperatingName = execute.Attribute("ruleId").Value;

                bool result = true;

                int actionPriority = 500;

                if (execute.Attribute("priority") != null)
                {
                  actionPriority = Int32.Parse(execute.Attribute("priority").Value);
                }

                if (execute.Attribute("result") != null)
                {
                  result = Boolean.Parse(execute.Attribute("result").Value);
                }

                var actionId = string.Format("{0}-{1}-{2}", currentRuleId, actionOperatingName, actionCounter++);

                rom.AddEvidence(new ActionExecute(actionId, actionOperatingName, actionPriority));
                actions.Add(new EvidenceSpecifier(result, actionId));
              }
            }
            catch (Exception e)
            {
              throw new Exception("Invalid action: " + execute, e);
            }
          }
        }

        #endregion

        IRule rule = new Rule(currentRuleId, condition, actions, priority, hasEvidence, isMain);
        rom.AddEvidence(rule);
      }
    }
  }
}
