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
using Atlantis.Framework.RuleEngine.Evidence.EvidenceValue;

namespace Atlantis.Framework.RuleEngine.Compiler
{
  public class Compiler
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
              throw new Exception("Fact has already been loaded: " + id);

            //determine priority
            int priority = 500;
            if (factNode.Attributes["priority"] != null)
              priority = Int32.Parse(factNode.Attributes["priority"].Value);


            IFact fact = null;
            var xmlElement = factNode["xpath"];
            if (xmlElement != null && xmlElement.InnerText != String.Empty)
            {
              //determine xpath
              var element = factNode["xpath"];
              if (element != null)
              {
                string xpath = element.InnerText;

                //determine value type
                Type valueType;
                switch (type) //deterrmine the type of value returned by xpath
                {
                  case "double":
                    valueType = typeof(double);
                    break;
                  case "boolean":
                    valueType = typeof(bool);
                    break;
                  case "string":
                    valueType = typeof(string);
                    break;
                  default:
                    throw new Exception("Invalid type: " + id + " " + type);
                }

                var xml = new Xml(xpath, valueType, modelId);
                if (xml.IsXmlValid)
                {
                  //create fact and add it to the rom
                  fact = new Fact(id, priority, xpath, valueType, modelId);
                }
              }
            }

            rom.AddEvidence(fact);
          }
        }
      }
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
            string id = ruleNode.Attributes["id"].Value;
            //ensure this has not already been entered
            if (rom[id] != null)
            {
              throw new Exception("Rule Ids must be unique: " + id);
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
            var xmlElement = ruleNode["Condition"];
            if (xmlElement != null)
            {
              string condition = xmlElement.InnerText;

              //actions
              int actionCounter = 0;
              var actions = new List<EvidenceSpecifier>();

              #region Evaluate

              var evaluateList = ruleNode.SelectNodes("Actions//Evaluate");
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

                      var actionId = string.Format("{0}-{1}-{2}", id, actionOperatingName, actionCounter++);

                      rom.AddEvidence(new ActionExpression(actionId, actionOperatingName, evaluationSetValue, actionPriority));

                      actions.Add(new EvidenceSpecifier(result, actionId));
                    }
                  }
                  catch (Exception e)
                  {
                    throw new Exception("Invalid action: " + evaluate.OuterXml, e);
                  }
                }
              }

              #endregion

              #region Execute

              var executeList = ruleNode.SelectNodes("Actions//Execute");
              if (executeList != null)
              {
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

                      var actionId = string.Format("{0}-{1}-{2}", id, actionOperatingName, actionCounter++);

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

              //now create the rule
              IRule rule = new Rule(id, condition, actions, priority, hasEvidence);
              rom.AddEvidence(rule);
            }
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
        if (rule != null  && rule.IsChainable)
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