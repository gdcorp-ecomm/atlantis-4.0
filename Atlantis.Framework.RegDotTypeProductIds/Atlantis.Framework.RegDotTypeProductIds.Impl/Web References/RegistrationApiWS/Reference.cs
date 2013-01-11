﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17929.
// 
#pragma warning disable 1591

namespace Atlantis.Framework.RegDotTypeProductIds.Impl.RegistrationApiWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="RegistrationApiWebSvcSoap", Namespace="urn:RegistrationApiWebSvc")]
    public partial class RegistrationApiWebSvc : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetDomainAPIOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTLDAPIOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTLDAPIListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetProductIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetProductIdListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetServiceVersionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public RegistrationApiWebSvc() {
            this.Url = global::Atlantis.Framework.RegDotTypeProductIds.Impl.Properties.Settings.Default.Atlantis_Framework_RegDotTypeProductIds_Impl_RegistrationApiWS_RegistrationApiWebSvc;
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
        public event GetDomainAPICompletedEventHandler GetDomainAPICompleted;
        
        /// <remarks/>
        public event GetTLDAPICompletedEventHandler GetTLDAPICompleted;
        
        /// <remarks/>
        public event GetTLDAPIListCompletedEventHandler GetTLDAPIListCompleted;
        
        /// <remarks/>
        public event GetProductIdCompletedEventHandler GetProductIdCompleted;
        
        /// <remarks/>
        public event GetProductIdListCompletedEventHandler GetProductIdListCompleted;
        
        /// <remarks/>
        public event GetServiceVersionCompletedEventHandler GetServiceVersionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RegistrationApiWebSvc/GetDomainAPI", RequestNamespace="urn:RegistrationApiWebSvc", ResponseNamespace="urn:RegistrationApiWebSvc", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetDomainAPI(string xmlRequest) {
            object[] results = this.Invoke("GetDomainAPI", new object[] {
                        xmlRequest});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetDomainAPIAsync(string xmlRequest) {
            this.GetDomainAPIAsync(xmlRequest, null);
        }
        
        /// <remarks/>
        public void GetDomainAPIAsync(string xmlRequest, object userState) {
            if ((this.GetDomainAPIOperationCompleted == null)) {
                this.GetDomainAPIOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetDomainAPIOperationCompleted);
            }
            this.InvokeAsync("GetDomainAPI", new object[] {
                        xmlRequest}, this.GetDomainAPIOperationCompleted, userState);
        }
        
        private void OnGetDomainAPIOperationCompleted(object arg) {
            if ((this.GetDomainAPICompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetDomainAPICompleted(this, new GetDomainAPICompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RegistrationApiWebSvc/GetTLDAPI", RequestNamespace="urn:RegistrationApiWebSvc", ResponseNamespace="urn:RegistrationApiWebSvc", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetTLDAPI(string xmlRequest) {
            object[] results = this.Invoke("GetTLDAPI", new object[] {
                        xmlRequest});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetTLDAPIAsync(string xmlRequest) {
            this.GetTLDAPIAsync(xmlRequest, null);
        }
        
        /// <remarks/>
        public void GetTLDAPIAsync(string xmlRequest, object userState) {
            if ((this.GetTLDAPIOperationCompleted == null)) {
                this.GetTLDAPIOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTLDAPIOperationCompleted);
            }
            this.InvokeAsync("GetTLDAPI", new object[] {
                        xmlRequest}, this.GetTLDAPIOperationCompleted, userState);
        }
        
        private void OnGetTLDAPIOperationCompleted(object arg) {
            if ((this.GetTLDAPICompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTLDAPICompleted(this, new GetTLDAPICompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RegistrationApiWebSvc/GetTLDAPIList", RequestNamespace="urn:RegistrationApiWebSvc", ResponseNamespace="urn:RegistrationApiWebSvc", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetTLDAPIList(string xmlRequest) {
            object[] results = this.Invoke("GetTLDAPIList", new object[] {
                        xmlRequest});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetTLDAPIListAsync(string xmlRequest) {
            this.GetTLDAPIListAsync(xmlRequest, null);
        }
        
        /// <remarks/>
        public void GetTLDAPIListAsync(string xmlRequest, object userState) {
            if ((this.GetTLDAPIListOperationCompleted == null)) {
                this.GetTLDAPIListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTLDAPIListOperationCompleted);
            }
            this.InvokeAsync("GetTLDAPIList", new object[] {
                        xmlRequest}, this.GetTLDAPIListOperationCompleted, userState);
        }
        
        private void OnGetTLDAPIListOperationCompleted(object arg) {
            if ((this.GetTLDAPIListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTLDAPIListCompleted(this, new GetTLDAPIListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RegistrationApiWebSvc/GetProductId", RequestNamespace="urn:RegistrationApiWebSvc", ResponseNamespace="urn:RegistrationApiWebSvc", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetProductId(string xmlRequest) {
            object[] results = this.Invoke("GetProductId", new object[] {
                        xmlRequest});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetProductIdAsync(string xmlRequest) {
            this.GetProductIdAsync(xmlRequest, null);
        }
        
        /// <remarks/>
        public void GetProductIdAsync(string xmlRequest, object userState) {
            if ((this.GetProductIdOperationCompleted == null)) {
                this.GetProductIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetProductIdOperationCompleted);
            }
            this.InvokeAsync("GetProductId", new object[] {
                        xmlRequest}, this.GetProductIdOperationCompleted, userState);
        }
        
        private void OnGetProductIdOperationCompleted(object arg) {
            if ((this.GetProductIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetProductIdCompleted(this, new GetProductIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RegistrationApiWebSvc/GetProductIdList", RequestNamespace="urn:RegistrationApiWebSvc", ResponseNamespace="urn:RegistrationApiWebSvc", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetProductIdList(string xmlRequest) {
            object[] results = this.Invoke("GetProductIdList", new object[] {
                        xmlRequest});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetProductIdListAsync(string xmlRequest) {
            this.GetProductIdListAsync(xmlRequest, null);
        }
        
        /// <remarks/>
        public void GetProductIdListAsync(string xmlRequest, object userState) {
            if ((this.GetProductIdListOperationCompleted == null)) {
                this.GetProductIdListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetProductIdListOperationCompleted);
            }
            this.InvokeAsync("GetProductIdList", new object[] {
                        xmlRequest}, this.GetProductIdListOperationCompleted, userState);
        }
        
        private void OnGetProductIdListOperationCompleted(object arg) {
            if ((this.GetProductIdListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetProductIdListCompleted(this, new GetProductIdListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RegistrationApiWebSvc/GetServiceVersion", RequestNamespace="urn:RegistrationApiWebSvc", ResponseNamespace="urn:RegistrationApiWebSvc", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetServiceVersion() {
            object[] results = this.Invoke("GetServiceVersion", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetServiceVersionAsync() {
            this.GetServiceVersionAsync(null);
        }
        
        /// <remarks/>
        public void GetServiceVersionAsync(object userState) {
            if ((this.GetServiceVersionOperationCompleted == null)) {
                this.GetServiceVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetServiceVersionOperationCompleted);
            }
            this.InvokeAsync("GetServiceVersion", new object[0], this.GetServiceVersionOperationCompleted, userState);
        }
        
        private void OnGetServiceVersionOperationCompleted(object arg) {
            if ((this.GetServiceVersionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetServiceVersionCompleted(this, new GetServiceVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetDomainAPICompletedEventHandler(object sender, GetDomainAPICompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetDomainAPICompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetDomainAPICompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetTLDAPICompletedEventHandler(object sender, GetTLDAPICompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTLDAPICompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTLDAPICompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetTLDAPIListCompletedEventHandler(object sender, GetTLDAPIListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTLDAPIListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTLDAPIListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetProductIdCompletedEventHandler(object sender, GetProductIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetProductIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetProductIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetProductIdListCompletedEventHandler(object sender, GetProductIdListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetProductIdListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetProductIdListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetServiceVersionCompletedEventHandler(object sender, GetServiceVersionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetServiceVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetServiceVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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