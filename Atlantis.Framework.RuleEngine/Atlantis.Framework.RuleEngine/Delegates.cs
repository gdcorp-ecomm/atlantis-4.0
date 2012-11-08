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
using Atlantis.Framework.RuleEngine.Model;
using Atlantis.Framework.RuleEngine.Results;

namespace Atlantis.Framework.RuleEngine
{
    /// <summary>
    /// States that an object has changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate void ChangedHandler(object sender, ChangedArgs args);

    /// <summary>
    /// Requests the specified IEvidence
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate IEvidence EvidenceLookupHandler(object sender, EvidenceLookupArgs args);

  /// <summary>
  /// ActionCallback object fires off a event to the client
  /// </summary>
  /// <param name="sender">action that caused the event</param>
  /// <param name="args"> </param>
  /// <returns>truthality of the callback</returns>
  public delegate void CallbackHandler(object sender, CallbackArgs args);

  /// <summary>
  /// Requests the specified Model. Note: a model could be either a client supplied model through the rom OR the parent node of a nested fact.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"> </param>
  /// <returns></returns>
  public delegate  Dictionary<string, string> ModelLookupHandler(object sender, ModelLookupArgs e);

    /// <summary>
    /// Specific registered mediator events operate of this type of delegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate object RegisteredMethodHandler(object sender, EventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class EvidenceLookupArgs : EventArgs
    {
        private readonly string _key;

        //[System.Diagnostics.DebuggerHidden]
        public EvidenceLookupArgs(string Key)
        {
            _key = Key;
        }

        //[System.Diagnostics.DebuggerHidden]
        public string Key
        {
            get
            {
                return _key;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ModelLookupArgs : EventArgs
    {
        private readonly string key;
        
        //[System.Diagnostics.DebuggerHidden]
        public ModelLookupArgs(string Key)
        {
            key = Key;
        }

        //[System.Diagnostics.DebuggerHidden]
        public string Key
        {
            get
            {
                return key;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChangedArgs : EventArgs
    {
    }
    public class ChangedModelArgs : ChangedArgs
    {
        private readonly string modelId;

        //[System.Diagnostics.DebuggerHidden]
        public ChangedModelArgs(string modelId)
        {
            this.modelId = modelId;
        }

        //[System.Diagnostics.DebuggerHidden]
        public string ModelId
        {
            get
            {
                return modelId;
            }
        }
    }

    /// <summary>
    /// EventArgs for requesting the mediator list
    /// </summary>
    public class MediatorListArgs : EventArgs
    {
    }

    /// <summary>
    /// Event args for when a model is updated
    /// </summary>
    public class ModelUpdateArgs : EventArgs
    {
        private readonly string _modelId;

        //[System.Diagnostics.DebuggerHidden]
        public ModelUpdateArgs(string modelId)
        {
            _modelId = modelId;
        }

        //[System.Diagnostics.DebuggerHidden]
        public string ModelId
        {
            get
            {
                return _modelId;
            }
        }
    }


    public class CallbackArgs : EventArgs
    {
        private readonly string callbackId;

        //[System.Diagnostics.DebuggerHidden]
        public CallbackArgs(string callbackId)
        {
            this.callbackId = callbackId;
        }

        //[System.Diagnostics.DebuggerHidden]
        public string CallbackId
        {
            get
            {
                return callbackId;
            }
        }
    }
}
