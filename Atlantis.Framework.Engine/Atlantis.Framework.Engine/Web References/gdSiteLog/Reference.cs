﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace Atlantis.Framework.Engine.gdSiteLog {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="WSCgdSiteLogServiceSoap", Namespace="urn:WSCgdSiteLogService")]
    public partial class WSCgdSiteLogService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback LogErrorOperationCompleted;
        
        private System.Threading.SendOrPostCallback LogErrorExOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSCgdSiteLogService() {
            this.Url = global::Atlantis.Framework.Engine.Properties.Settings.Default.Atlantis_Framework_Engine_gdSiteLog_WSCgdSiteLogService;
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
        public event LogErrorCompletedEventHandler LogErrorCompleted;
        
        /// <remarks/>
        public event LogErrorExCompletedEventHandler LogErrorExCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#LogError", RequestNamespace="urn:WSCgdSiteLogService", ResponseNamespace="urn:WSCgdSiteLogService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string LogError(string bstrInput) {
            object[] results = this.Invoke("LogError", new object[] {
                        bstrInput});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LogErrorAsync(string bstrInput) {
            this.LogErrorAsync(bstrInput, null);
        }
        
        /// <remarks/>
        public void LogErrorAsync(string bstrInput, object userState) {
            if ((this.LogErrorOperationCompleted == null)) {
                this.LogErrorOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLogErrorOperationCompleted);
            }
            this.InvokeAsync("LogError", new object[] {
                        bstrInput}, this.LogErrorOperationCompleted, userState);
        }
        
        private void OnLogErrorOperationCompleted(object arg) {
            if ((this.LogErrorCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LogErrorCompleted(this, new LogErrorCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#LogErrorEx", RequestNamespace="urn:WSCgdSiteLogService", ResponseNamespace="urn:WSCgdSiteLogService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string LogErrorEx(string bstrSourceServer, string bstrSourceFunction, string bstrURL, uint lErrorNumber, string bstrErrorDescription, string bstrInputData, string bstrShopperId, string bstrOrderId, string bstrClientIP, string bstrPathway, int lPageCount) {
            object[] results = this.Invoke("LogErrorEx", new object[] {
                        bstrSourceServer,
                        bstrSourceFunction,
                        bstrURL,
                        lErrorNumber,
                        bstrErrorDescription,
                        bstrInputData,
                        bstrShopperId,
                        bstrOrderId,
                        bstrClientIP,
                        bstrPathway,
                        lPageCount});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LogErrorExAsync(string bstrSourceServer, string bstrSourceFunction, string bstrURL, uint lErrorNumber, string bstrErrorDescription, string bstrInputData, string bstrShopperId, string bstrOrderId, string bstrClientIP, string bstrPathway, int lPageCount) {
            this.LogErrorExAsync(bstrSourceServer, bstrSourceFunction, bstrURL, lErrorNumber, bstrErrorDescription, bstrInputData, bstrShopperId, bstrOrderId, bstrClientIP, bstrPathway, lPageCount, null);
        }
        
        /// <remarks/>
        public void LogErrorExAsync(string bstrSourceServer, string bstrSourceFunction, string bstrURL, uint lErrorNumber, string bstrErrorDescription, string bstrInputData, string bstrShopperId, string bstrOrderId, string bstrClientIP, string bstrPathway, int lPageCount, object userState) {
            if ((this.LogErrorExOperationCompleted == null)) {
                this.LogErrorExOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLogErrorExOperationCompleted);
            }
            this.InvokeAsync("LogErrorEx", new object[] {
                        bstrSourceServer,
                        bstrSourceFunction,
                        bstrURL,
                        lErrorNumber,
                        bstrErrorDescription,
                        bstrInputData,
                        bstrShopperId,
                        bstrOrderId,
                        bstrClientIP,
                        bstrPathway,
                        lPageCount}, this.LogErrorExOperationCompleted, userState);
        }
        
        private void OnLogErrorExOperationCompleted(object arg) {
            if ((this.LogErrorExCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LogErrorExCompleted(this, new LogErrorExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void LogErrorCompletedEventHandler(object sender, LogErrorCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LogErrorCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LogErrorCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void LogErrorExCompletedEventHandler(object sender, LogErrorExCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LogErrorExCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LogErrorExCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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