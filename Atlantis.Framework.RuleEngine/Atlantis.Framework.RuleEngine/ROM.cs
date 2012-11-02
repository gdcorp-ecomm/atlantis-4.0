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
        public void AddModel(string modelId, XmlDocument model)
        {
            //add model to collection
            _models.Add(modelId, model);
        }

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

        /// <summary>
        /// 
        /// </summary>
        public void Evaluate()
        {
            var decision = (new Decisions.Decision());
            decision.EvidenceLookup += evidence_EvidenceLookup;
            decision.ModelLookup += evidence_ModelLookup;
            decision.Evaluate(_evidenceCollection, _dependentEvidence);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var rom = new ROM {_callback = new Dictionary<string, Delegate>(_callback), _dependentEvidence = new Dictionary<string, List<string>>()};

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
        private XmlNode evidence_ModelLookup(object sender, ModelLookupArgs args)
        {
            try
            {
                return _models[args.Key];
            }
            catch
            {
                throw new Exception("Could not find model: " + args.Key);
            }
        }

        #endregion
    }
}