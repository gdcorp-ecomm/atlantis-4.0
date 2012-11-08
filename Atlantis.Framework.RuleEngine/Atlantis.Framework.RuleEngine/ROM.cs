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
using Atlantis.Framework.RuleEngine.Evidence;
using Atlantis.Framework.RuleEngine.Model;
using Atlantis.Framework.RuleEngine.Results;

namespace Atlantis.Framework.RuleEngine
{
  public class ROM : ICloneable
  {
    #region instance variables
    /// <summary>
    /// collection of all evidence objects
    /// </summary>
    private Dictionary<string, IEvidence> _evidenceCollection = new Dictionary<string, IEvidence>();
    /// <summary>
    /// specifies the models to be used
    /// </summary>
    private Dictionary<string, XmlDocument> _models = new Dictionary<string, XmlDocument>();

    /// <summary>
    /// specifies for a given evidence all evidence thats dependent on it
    /// </summary>
    private Dictionary<string, List<string>> _dependentEvidence = new Dictionary<string, List<string>>();
    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string, Delegate> _callback = new Dictionary<string, Delegate>();
    #endregion
    #region constructor

    #endregion
    #region core
    /// <summary>
    /// Add a model to the ROM. The name given to the model must match those specified in the ruleset.
    /// </summary>
    /// <param name="modelId"></param>
    /// <param name="model"></param>
    //public void AddModel(string modelId, XmlDocument model)
    //{
    //  //add model to collection
    //  _models.Add(modelId, model);
    //}

    public void AddModel(string modelId, Dictionary<string, string> models)
    {
      if (_inputModels.ContainsKey(modelId))
      {
        models.ToList().ForEach(m => _inputModels[modelId].Add(m.Key, m.Value));
      }
      else
      {
        _inputModels.Add(modelId, models);
      }
    }

    public void AddModel(string modelId, string key, string value)
    {
      Dictionary<string, string> facts = null;
      var hasModel = false;
      if (_inputModels.ContainsKey(modelId))
      {
        _inputModels[modelId].Add(key, value);
      }
      else
      {
        _inputModels.Add(modelId, new Dictionary<string, string> {{key, value}});
      }
    }

    private readonly Dictionary<string, Dictionary<string, string>> _inputModels = new Dictionary<string, Dictionary<string, string>>();

    readonly IList<IModelResult> _modelResults = new List<IModelResult>(0);
    public IList<IModelResult> ModelResults { get { return _modelResults; } }

    /// <summary>
    /// Add a fact, action, or rule to the ROM.
    /// </summary>
    public void AddEvidence(IEvidence evidence)
    {
      //add evidence to collection
      _evidenceCollection.Add(evidence.Id, evidence);
    }

    /// <summary>
    /// specifies for a given evidence all evidence thats dependent on it
    /// </summary>
    /// <param name="evidence"></param>
    /// <param name="dependentEvidence"></param>
    public void AddDependentFact(string evidence, string dependentEvidence)
    {
      if (!_dependentEvidence.ContainsKey(evidence))
      {
        _dependentEvidence.Add(evidence, new List<string>());
      }

      _dependentEvidence[evidence].Add(dependentEvidence);
    }

    /// <summary>
    /// 
    /// </summary>
    internal Dictionary<string, IEvidence> Evidence
    {
      get
      {
        return _evidenceCollection;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IEvidence this[string id]
    {
      get
      {
        try
        {
          return _evidenceCollection[id];
        }
        catch
        {
          return null;
        }
      }
      set
      {
        _evidenceCollection[id] = value;
      }
    }

    void SetResults()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public void Evaluate()
    {
      var decision = (new Decisions.Decision());
      decision.EvidenceLookup += evidence_EvidenceLookup;
      decision.ModelLookup += EvidenceModelLookup;
      decision.Evaluate(_evidenceCollection, _dependentEvidence);
      
      var modelId = string.Empty;
      var factModels = new Dictionary<string, IList<IFactResult>>(0);
      foreach (var evidenceKey in Evidence.Keys)
      {
        var evidence = Evidence[evidenceKey];
        var fact = evidence as IFact;

        if (modelId == string.Empty)
        {
          modelId = evidence.ValueObject.ModelId;
        }

        
        if (fact != null)
        {
          var status = fact.IsValid ? ValidationResultStatus.Valid : ValidationResultStatus.InValid;
          var result = new FactResult(fact.Key, status) {Messages = fact.Messages};
          
          if (factModels.ContainsKey(modelId))
          {
            factModels[modelId].Add(result);
          }
          else
          {
            factModels.Add(modelId, new List<IFactResult>(){result});
          }
        }
      }

      foreach (var factModelKey in factModels.Keys)
      {
        var modelResult = new ModelResult();
        var invalidFact = factModels[factModelKey].FirstOrDefault(f => f.Status == ValidationResultStatus.InValid);
        var isInvalid = invalidFact != null;

        modelResult.ModelId = factModelKey;
        modelResult.ContainsInvalids = isInvalid;
        modelResult.Facts = factModels[factModelKey];

        ModelResults.Add(modelResult);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
      var rom = new ROM { _callback = new Dictionary<string, Delegate>(_callback), _dependentEvidence = new Dictionary<string, List<string>>() };

      foreach (string key in _dependentEvidence.Keys)
      {
        rom._dependentEvidence.Add(key, new List<string>(_dependentEvidence[key]));
      }

      rom._evidenceCollection = new Dictionary<string, IEvidence>();
      foreach (string key in _evidenceCollection.Keys)
      {
        var evidence = (IEvidence)_evidenceCollection[key].Clone();
        rom._evidenceCollection.Add(key, evidence);
      }

      rom._models = new Dictionary<string, XmlDocument>(_models);

      return rom;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void RegisterCallback(string name, Delegate callback)
    {
      _callback.Add(name, callback);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    private IEvidence evidence_EvidenceLookup(object sender, EvidenceLookupArgs args)
    {
      try
      {
        return _evidenceCollection[args.Key];
      }
      catch (Exception e)
      {
        throw new Exception("Could not find evidence: " + args.Key, e);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    private Dictionary<string, string> EvidenceModelLookup(object sender, ModelLookupArgs args)
    {
      try
      {
        return _inputModels[args.Key];
      }
      catch
      {
        throw new Exception("Could not find model: " + args.Key);
      }
    }

    #endregion
  }
}