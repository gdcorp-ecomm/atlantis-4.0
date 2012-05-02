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

namespace Atlantis.Framework.EcommCreditCardReqs.Impl.WSgdCreditCardRequirements {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="CardRequirementsSoap", Namespace="http://WSgdCardRequirements.godaddy.com/")]
    public partial class CardRequirements : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetRequirementsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRequirementsExOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRequirementsByProfileOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRequirementsByProfileExOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CardRequirements() {
            this.Url = global::Atlantis.Framework.EcommCreditCardReqs.Impl.Properties.Settings.Default.Atlantis_Framework_EcommCreditCardReqs_Impl_WSgdCreditCardRequirements_CardRequirements;
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
        public event GetRequirementsCompletedEventHandler GetRequirementsCompleted;
        
        /// <remarks/>
        public event GetRequirementsExCompletedEventHandler GetRequirementsExCompleted;
        
        /// <remarks/>
        public event GetRequirementsByProfileCompletedEventHandler GetRequirementsByProfileCompleted;
        
        /// <remarks/>
        public event GetRequirementsByProfileExCompletedEventHandler GetRequirementsByProfileExCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://WSgdCardRequirements.godaddy.com/GetRequirements", RequestNamespace="http://WSgdCardRequirements.godaddy.com/", ResponseNamespace="http://WSgdCardRequirements.godaddy.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool GetRequirements(string sXCardNo, out string sRequirementXML) {
            object[] results = this.Invoke("GetRequirements", new object[] {
                        sXCardNo});
            sRequirementXML = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void GetRequirementsAsync(string sXCardNo) {
            this.GetRequirementsAsync(sXCardNo, null);
        }
        
        /// <remarks/>
        public void GetRequirementsAsync(string sXCardNo, object userState) {
            if ((this.GetRequirementsOperationCompleted == null)) {
                this.GetRequirementsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequirementsOperationCompleted);
            }
            this.InvokeAsync("GetRequirements", new object[] {
                        sXCardNo}, this.GetRequirementsOperationCompleted, userState);
        }
        
        private void OnGetRequirementsOperationCompleted(object arg) {
            if ((this.GetRequirementsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequirementsCompleted(this, new GetRequirementsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://WSgdCardRequirements.godaddy.com/GetRequirementsEx", RequestNamespace="http://WSgdCardRequirements.godaddy.com/", ResponseNamespace="http://WSgdCardRequirements.godaddy.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool GetRequirementsEx(string sCardXML, out string sRequirementXML) {
            object[] results = this.Invoke("GetRequirementsEx", new object[] {
                        sCardXML});
            sRequirementXML = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void GetRequirementsExAsync(string sCardXML) {
            this.GetRequirementsExAsync(sCardXML, null);
        }
        
        /// <remarks/>
        public void GetRequirementsExAsync(string sCardXML, object userState) {
            if ((this.GetRequirementsExOperationCompleted == null)) {
                this.GetRequirementsExOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequirementsExOperationCompleted);
            }
            this.InvokeAsync("GetRequirementsEx", new object[] {
                        sCardXML}, this.GetRequirementsExOperationCompleted, userState);
        }
        
        private void OnGetRequirementsExOperationCompleted(object arg) {
            if ((this.GetRequirementsExCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequirementsExCompleted(this, new GetRequirementsExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://WSgdCardRequirements.godaddy.com/GetRequirementsByProfile", RequestNamespace="http://WSgdCardRequirements.godaddy.com/", ResponseNamespace="http://WSgdCardRequirements.godaddy.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool GetRequirementsByProfile(string sShopperID, int nProfileID, out string sRequirementXML) {
            object[] results = this.Invoke("GetRequirementsByProfile", new object[] {
                        sShopperID,
                        nProfileID});
            sRequirementXML = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void GetRequirementsByProfileAsync(string sShopperID, int nProfileID) {
            this.GetRequirementsByProfileAsync(sShopperID, nProfileID, null);
        }
        
        /// <remarks/>
        public void GetRequirementsByProfileAsync(string sShopperID, int nProfileID, object userState) {
            if ((this.GetRequirementsByProfileOperationCompleted == null)) {
                this.GetRequirementsByProfileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequirementsByProfileOperationCompleted);
            }
            this.InvokeAsync("GetRequirementsByProfile", new object[] {
                        sShopperID,
                        nProfileID}, this.GetRequirementsByProfileOperationCompleted, userState);
        }
        
        private void OnGetRequirementsByProfileOperationCompleted(object arg) {
            if ((this.GetRequirementsByProfileCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequirementsByProfileCompleted(this, new GetRequirementsByProfileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://WSgdCardRequirements.godaddy.com/GetRequirementsByProfileEx", RequestNamespace="http://WSgdCardRequirements.godaddy.com/", ResponseNamespace="http://WSgdCardRequirements.godaddy.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool GetRequirementsByProfileEx(string sShopperID, int nProfileID, string sCurrency, out string sRequirementXML) {
            object[] results = this.Invoke("GetRequirementsByProfileEx", new object[] {
                        sShopperID,
                        nProfileID,
                        sCurrency});
            sRequirementXML = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void GetRequirementsByProfileExAsync(string sShopperID, int nProfileID, string sCurrency) {
            this.GetRequirementsByProfileExAsync(sShopperID, nProfileID, sCurrency, null);
        }
        
        /// <remarks/>
        public void GetRequirementsByProfileExAsync(string sShopperID, int nProfileID, string sCurrency, object userState) {
            if ((this.GetRequirementsByProfileExOperationCompleted == null)) {
                this.GetRequirementsByProfileExOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequirementsByProfileExOperationCompleted);
            }
            this.InvokeAsync("GetRequirementsByProfileEx", new object[] {
                        sShopperID,
                        nProfileID,
                        sCurrency}, this.GetRequirementsByProfileExOperationCompleted, userState);
        }
        
        private void OnGetRequirementsByProfileExOperationCompleted(object arg) {
            if ((this.GetRequirementsByProfileExCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequirementsByProfileExCompleted(this, new GetRequirementsByProfileExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void GetRequirementsCompletedEventHandler(object sender, GetRequirementsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequirementsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRequirementsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string sRequirementXML {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetRequirementsExCompletedEventHandler(object sender, GetRequirementsExCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequirementsExCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRequirementsExCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string sRequirementXML {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetRequirementsByProfileCompletedEventHandler(object sender, GetRequirementsByProfileCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequirementsByProfileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRequirementsByProfileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string sRequirementXML {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetRequirementsByProfileExCompletedEventHandler(object sender, GetRequirementsByProfileExCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequirementsByProfileExCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRequirementsByProfileExCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string sRequirementXML {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591