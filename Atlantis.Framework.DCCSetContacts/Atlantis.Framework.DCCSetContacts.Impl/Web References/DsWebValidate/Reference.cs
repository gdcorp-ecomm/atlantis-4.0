﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.235.
// 
#pragma warning disable 1591

namespace Atlantis.Framework.DCCSetContacts.Impl.DsWebValidate
{


  /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="RegDCCValidateWebSvcSoap", Namespace="http://tempuri.org/")]
    public partial class RegDCCValidateWebSvc : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ValidateDomainSetLockOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateDomainSetAutoRenewOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateDomainForwardingUpdateOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateDomainForwardingDeleteOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateContactUpdateOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateNameserverUpdateOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateDomainRenewalOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public RegDCCValidateWebSvc() {
            this.Url = global::Atlantis.Framework.DCCSetContacts.Impl.Properties.Settings.Default.Atlantis_Framework_DCCSetContacts_Impl_DsWebValidate_RegDCCValidateWebSvc;
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
        public event ValidateDomainSetLockCompletedEventHandler ValidateDomainSetLockCompleted;
        
        /// <remarks/>
        public event ValidateDomainSetAutoRenewCompletedEventHandler ValidateDomainSetAutoRenewCompleted;
        
        /// <remarks/>
        public event ValidateDomainForwardingUpdateCompletedEventHandler ValidateDomainForwardingUpdateCompleted;
        
        /// <remarks/>
        public event ValidateDomainForwardingDeleteCompletedEventHandler ValidateDomainForwardingDeleteCompleted;
        
        /// <remarks/>
        public event ValidateContactUpdateCompletedEventHandler ValidateContactUpdateCompleted;
        
        /// <remarks/>
        public event ValidateNameserverUpdateCompletedEventHandler ValidateNameserverUpdateCompleted;
        
        /// <remarks/>
        public event ValidateDomainRenewalCompletedEventHandler ValidateDomainRenewalCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateDomainSetLock", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateDomainSetLock(string action, string domains) {
            object[] results = this.Invoke("ValidateDomainSetLock", new object[] {
                        action,
                        domains});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateDomainSetLockAsync(string action, string domains) {
            this.ValidateDomainSetLockAsync(action, domains, null);
        }
        
        /// <remarks/>
        public void ValidateDomainSetLockAsync(string action, string domains, object userState) {
            if ((this.ValidateDomainSetLockOperationCompleted == null)) {
                this.ValidateDomainSetLockOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateDomainSetLockOperationCompleted);
            }
            this.InvokeAsync("ValidateDomainSetLock", new object[] {
                        action,
                        domains}, this.ValidateDomainSetLockOperationCompleted, userState);
        }
        
        private void OnValidateDomainSetLockOperationCompleted(object arg) {
            if ((this.ValidateDomainSetLockCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateDomainSetLockCompleted(this, new ValidateDomainSetLockCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateDomainSetAutoRenew", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateDomainSetAutoRenew(string action, string domains) {
            object[] results = this.Invoke("ValidateDomainSetAutoRenew", new object[] {
                        action,
                        domains});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateDomainSetAutoRenewAsync(string action, string domains) {
            this.ValidateDomainSetAutoRenewAsync(action, domains, null);
        }
        
        /// <remarks/>
        public void ValidateDomainSetAutoRenewAsync(string action, string domains, object userState) {
            if ((this.ValidateDomainSetAutoRenewOperationCompleted == null)) {
                this.ValidateDomainSetAutoRenewOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateDomainSetAutoRenewOperationCompleted);
            }
            this.InvokeAsync("ValidateDomainSetAutoRenew", new object[] {
                        action,
                        domains}, this.ValidateDomainSetAutoRenewOperationCompleted, userState);
        }
        
        private void OnValidateDomainSetAutoRenewOperationCompleted(object arg) {
            if ((this.ValidateDomainSetAutoRenewCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateDomainSetAutoRenewCompleted(this, new ValidateDomainSetAutoRenewCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateDomainForwardingUpdate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateDomainForwardingUpdate(string action, string domains) {
            object[] results = this.Invoke("ValidateDomainForwardingUpdate", new object[] {
                        action,
                        domains});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateDomainForwardingUpdateAsync(string action, string domains) {
            this.ValidateDomainForwardingUpdateAsync(action, domains, null);
        }
        
        /// <remarks/>
        public void ValidateDomainForwardingUpdateAsync(string action, string domains, object userState) {
            if ((this.ValidateDomainForwardingUpdateOperationCompleted == null)) {
                this.ValidateDomainForwardingUpdateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateDomainForwardingUpdateOperationCompleted);
            }
            this.InvokeAsync("ValidateDomainForwardingUpdate", new object[] {
                        action,
                        domains}, this.ValidateDomainForwardingUpdateOperationCompleted, userState);
        }
        
        private void OnValidateDomainForwardingUpdateOperationCompleted(object arg) {
            if ((this.ValidateDomainForwardingUpdateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateDomainForwardingUpdateCompleted(this, new ValidateDomainForwardingUpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateDomainForwardingDelete", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateDomainForwardingDelete(string action, string domains) {
            object[] results = this.Invoke("ValidateDomainForwardingDelete", new object[] {
                        action,
                        domains});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateDomainForwardingDeleteAsync(string action, string domains) {
            this.ValidateDomainForwardingDeleteAsync(action, domains, null);
        }
        
        /// <remarks/>
        public void ValidateDomainForwardingDeleteAsync(string action, string domains, object userState) {
            if ((this.ValidateDomainForwardingDeleteOperationCompleted == null)) {
                this.ValidateDomainForwardingDeleteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateDomainForwardingDeleteOperationCompleted);
            }
            this.InvokeAsync("ValidateDomainForwardingDelete", new object[] {
                        action,
                        domains}, this.ValidateDomainForwardingDeleteOperationCompleted, userState);
        }
        
        private void OnValidateDomainForwardingDeleteOperationCompleted(object arg) {
            if ((this.ValidateDomainForwardingDeleteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateDomainForwardingDeleteCompleted(this, new ValidateDomainForwardingDeleteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateContactUpdate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateContactUpdate(string action, string domains) {
            object[] results = this.Invoke("ValidateContactUpdate", new object[] {
                        action,
                        domains});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateContactUpdateAsync(string action, string domains) {
            this.ValidateContactUpdateAsync(action, domains, null);
        }
        
        /// <remarks/>
        public void ValidateContactUpdateAsync(string action, string domains, object userState) {
            if ((this.ValidateContactUpdateOperationCompleted == null)) {
                this.ValidateContactUpdateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateContactUpdateOperationCompleted);
            }
            this.InvokeAsync("ValidateContactUpdate", new object[] {
                        action,
                        domains}, this.ValidateContactUpdateOperationCompleted, userState);
        }
        
        private void OnValidateContactUpdateOperationCompleted(object arg) {
            if ((this.ValidateContactUpdateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateContactUpdateCompleted(this, new ValidateContactUpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateNameserverUpdate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateNameserverUpdate(string action, string domains) {
            object[] results = this.Invoke("ValidateNameserverUpdate", new object[] {
                        action,
                        domains});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateNameserverUpdateAsync(string action, string domains) {
            this.ValidateNameserverUpdateAsync(action, domains, null);
        }
        
        /// <remarks/>
        public void ValidateNameserverUpdateAsync(string action, string domains, object userState) {
            if ((this.ValidateNameserverUpdateOperationCompleted == null)) {
                this.ValidateNameserverUpdateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateNameserverUpdateOperationCompleted);
            }
            this.InvokeAsync("ValidateNameserverUpdate", new object[] {
                        action,
                        domains}, this.ValidateNameserverUpdateOperationCompleted, userState);
        }
        
        private void OnValidateNameserverUpdateOperationCompleted(object arg) {
            if ((this.ValidateNameserverUpdateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateNameserverUpdateCompleted(this, new ValidateNameserverUpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateDomainRenewal", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateDomainRenewal(string action, string domains) {
            object[] results = this.Invoke("ValidateDomainRenewal", new object[] {
                        action,
                        domains});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateDomainRenewalAsync(string action, string domains) {
            this.ValidateDomainRenewalAsync(action, domains, null);
        }
        
        /// <remarks/>
        public void ValidateDomainRenewalAsync(string action, string domains, object userState) {
            if ((this.ValidateDomainRenewalOperationCompleted == null)) {
                this.ValidateDomainRenewalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateDomainRenewalOperationCompleted);
            }
            this.InvokeAsync("ValidateDomainRenewal", new object[] {
                        action,
                        domains}, this.ValidateDomainRenewalOperationCompleted, userState);
        }
        
        private void OnValidateDomainRenewalOperationCompleted(object arg) {
            if ((this.ValidateDomainRenewalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateDomainRenewalCompleted(this, new ValidateDomainRenewalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void ValidateDomainSetLockCompletedEventHandler(object sender, ValidateDomainSetLockCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateDomainSetLockCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateDomainSetLockCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void ValidateDomainSetAutoRenewCompletedEventHandler(object sender, ValidateDomainSetAutoRenewCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateDomainSetAutoRenewCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateDomainSetAutoRenewCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void ValidateDomainForwardingUpdateCompletedEventHandler(object sender, ValidateDomainForwardingUpdateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateDomainForwardingUpdateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateDomainForwardingUpdateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void ValidateDomainForwardingDeleteCompletedEventHandler(object sender, ValidateDomainForwardingDeleteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateDomainForwardingDeleteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateDomainForwardingDeleteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void ValidateContactUpdateCompletedEventHandler(object sender, ValidateContactUpdateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateContactUpdateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateContactUpdateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void ValidateNameserverUpdateCompletedEventHandler(object sender, ValidateNameserverUpdateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateNameserverUpdateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateNameserverUpdateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void ValidateDomainRenewalCompletedEventHandler(object sender, ValidateDomainRenewalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateDomainRenewalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateDomainRenewalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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