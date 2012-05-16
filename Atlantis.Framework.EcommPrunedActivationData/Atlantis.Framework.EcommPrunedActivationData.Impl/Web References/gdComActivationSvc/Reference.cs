﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17379.
// 
#pragma warning disable 1591

namespace Atlantis.Framework.EcommPrunedActivationData.Impl.gdComActivationSvc {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17379")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Service1Soap", Namespace="http://tempuri.org/")]
    public partial class Service1 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetSetupDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetSetupStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetSetupDataLiteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service1() {
            this.Url = global::Atlantis.Framework.EcommPrunedActivationData.Impl.Properties.Settings.Default.Atlantis_Framework_EcommPrunedActivationData_Impl_gdComActivationSvc_Service1;
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
        public event GetSetupDataCompletedEventHandler GetSetupDataCompleted;
        
        /// <remarks/>
        public event GetSetupStatusCompletedEventHandler GetSetupStatusCompleted;
        
        /// <remarks/>
        public event GetSetupDataLiteCompletedEventHandler GetSetupDataLiteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetSetupData", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetSetupData(string sShopperID, string sOrderID) {
            object[] results = this.Invoke("GetSetupData", new object[] {
                        sShopperID,
                        sOrderID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetSetupDataAsync(string sShopperID, string sOrderID) {
            this.GetSetupDataAsync(sShopperID, sOrderID, null);
        }
        
        /// <remarks/>
        public void GetSetupDataAsync(string sShopperID, string sOrderID, object userState) {
            if ((this.GetSetupDataOperationCompleted == null)) {
                this.GetSetupDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSetupDataOperationCompleted);
            }
            this.InvokeAsync("GetSetupData", new object[] {
                        sShopperID,
                        sOrderID}, this.GetSetupDataOperationCompleted, userState);
        }
        
        private void OnGetSetupDataOperationCompleted(object arg) {
            if ((this.GetSetupDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSetupDataCompleted(this, new GetSetupDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetSetupStatus", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetSetupStatus(string sShopperID, string sOrderID) {
            object[] results = this.Invoke("GetSetupStatus", new object[] {
                        sShopperID,
                        sOrderID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetSetupStatusAsync(string sShopperID, string sOrderID) {
            this.GetSetupStatusAsync(sShopperID, sOrderID, null);
        }
        
        /// <remarks/>
        public void GetSetupStatusAsync(string sShopperID, string sOrderID, object userState) {
            if ((this.GetSetupStatusOperationCompleted == null)) {
                this.GetSetupStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSetupStatusOperationCompleted);
            }
            this.InvokeAsync("GetSetupStatus", new object[] {
                        sShopperID,
                        sOrderID}, this.GetSetupStatusOperationCompleted, userState);
        }
        
        private void OnGetSetupStatusOperationCompleted(object arg) {
            if ((this.GetSetupStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSetupStatusCompleted(this, new GetSetupStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetSetupDataLite", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetSetupDataLite(string sShopperID, string sOrderID) {
            object[] results = this.Invoke("GetSetupDataLite", new object[] {
                        sShopperID,
                        sOrderID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetSetupDataLiteAsync(string sShopperID, string sOrderID) {
            this.GetSetupDataLiteAsync(sShopperID, sOrderID, null);
        }
        
        /// <remarks/>
        public void GetSetupDataLiteAsync(string sShopperID, string sOrderID, object userState) {
            if ((this.GetSetupDataLiteOperationCompleted == null)) {
                this.GetSetupDataLiteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSetupDataLiteOperationCompleted);
            }
            this.InvokeAsync("GetSetupDataLite", new object[] {
                        sShopperID,
                        sOrderID}, this.GetSetupDataLiteOperationCompleted, userState);
        }
        
        private void OnGetSetupDataLiteOperationCompleted(object arg) {
            if ((this.GetSetupDataLiteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSetupDataLiteCompleted(this, new GetSetupDataLiteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17379")]
    public delegate void GetSetupDataCompletedEventHandler(object sender, GetSetupDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17379")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSetupDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSetupDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17379")]
    public delegate void GetSetupStatusCompletedEventHandler(object sender, GetSetupStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17379")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSetupStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSetupStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17379")]
    public delegate void GetSetupDataLiteCompletedEventHandler(object sender, GetSetupDataLiteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17379")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSetupDataLiteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSetupDataLiteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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