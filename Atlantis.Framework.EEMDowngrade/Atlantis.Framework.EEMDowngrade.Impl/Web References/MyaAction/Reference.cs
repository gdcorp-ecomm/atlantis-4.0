﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.239.
// 
#pragma warning disable 1591

namespace Atlantis.Framework.EEMDowngrade.Impl.MyaAction {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSCmyaActionServiceSoap", Namespace="urn:WSCmyaActionService")]
    public partial class WSCmyaActionService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback QueueActionEventOperationCompleted;
        
        private System.Threading.SendOrPostCallback QueueActionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSCmyaActionService() {
            this.Url = global::Atlantis.Framework.EEMDowngrade.Impl.Properties.Settings.Default.Atlantis_Framework_EEMDowngrade_Impl_MyaAction_WSCmyaActionService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event QueueActionEventCompletedEventHandler QueueActionEventCompleted;
        
        /// <remarks/>
        public event QueueActionCompletedEventHandler QueueActionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#QueueActionEvent", RequestNamespace="urn:WSCmyaActionService", ResponseNamespace="urn:WSCmyaActionService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string QueueActionEvent(string bstrBody) {
            object[] results = this.Invoke("QueueActionEvent", new object[] {
                        bstrBody});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void QueueActionEventAsync(string bstrBody) {
            this.QueueActionEventAsync(bstrBody, null);
        }
        
        /// <remarks/>
        public void QueueActionEventAsync(string bstrBody, object userState) {
            if ((this.QueueActionEventOperationCompleted == null)) {
                this.QueueActionEventOperationCompleted = new System.Threading.SendOrPostCallback(this.OnQueueActionEventOperationCompleted);
            }
            this.InvokeAsync("QueueActionEvent", new object[] {
                        bstrBody}, this.QueueActionEventOperationCompleted, userState);
        }
        
        private void OnQueueActionEventOperationCompleted(object arg) {
            if ((this.QueueActionEventCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.QueueActionEventCompleted(this, new QueueActionEventCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#QueueAction", RequestNamespace="urn:WSCmyaActionService", ResponseNamespace="urn:WSCmyaActionService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string QueueAction(string bstrBody) {
            object[] results = this.Invoke("QueueAction", new object[] {
                        bstrBody});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void QueueActionAsync(string bstrBody) {
            this.QueueActionAsync(bstrBody, null);
        }
        
        /// <remarks/>
        public void QueueActionAsync(string bstrBody, object userState) {
            if ((this.QueueActionOperationCompleted == null)) {
                this.QueueActionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnQueueActionOperationCompleted);
            }
            this.InvokeAsync("QueueAction", new object[] {
                        bstrBody}, this.QueueActionOperationCompleted, userState);
        }
        
        private void OnQueueActionOperationCompleted(object arg) {
            if ((this.QueueActionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.QueueActionCompleted(this, new QueueActionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void QueueActionEventCompletedEventHandler(object sender, QueueActionEventCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class QueueActionEventCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal QueueActionEventCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void QueueActionCompletedEventHandler(object sender, QueueActionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class QueueActionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal QueueActionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591