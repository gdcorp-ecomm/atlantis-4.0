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

namespace Atlantis.Framework.CMSDomainAvail.Impl.cmsValidateDomain {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="CmsCreditSupportSoap", Namespace="http://services.godaddy.com/")]
    public partial class CmsCreditSupport : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CmsCreditDomainListsOperationCompleted;
        
        private System.Threading.SendOrPostCallback IsDomainValidForInstantPageOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CmsCreditSupport() {
            this.Url = global::Atlantis.Framework.CMSDomainAvail.Impl.Properties.Settings.Default.Atlantis_Framework_CMSDomainAvail_Impl_cmsValidateDomain_CmsCreditSupport;
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
        public event CmsCreditDomainListsCompletedEventHandler CmsCreditDomainListsCompleted;
        
        /// <remarks/>
        public event IsDomainValidForInstantPageCompletedEventHandler IsDomainValidForInstantPageCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://services.godaddy.com/CmsCreditDomainLists", RequestNamespace="http://services.godaddy.com/", ResponseNamespace="http://services.godaddy.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CmsCreditDomainLists(string cmsCreditDomainsRequestXml) {
            object[] results = this.Invoke("CmsCreditDomainLists", new object[] {
                        cmsCreditDomainsRequestXml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CmsCreditDomainListsAsync(string cmsCreditDomainsRequestXml) {
            this.CmsCreditDomainListsAsync(cmsCreditDomainsRequestXml, null);
        }
        
        /// <remarks/>
        public void CmsCreditDomainListsAsync(string cmsCreditDomainsRequestXml, object userState) {
            if ((this.CmsCreditDomainListsOperationCompleted == null)) {
                this.CmsCreditDomainListsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCmsCreditDomainListsOperationCompleted);
            }
            this.InvokeAsync("CmsCreditDomainLists", new object[] {
                        cmsCreditDomainsRequestXml}, this.CmsCreditDomainListsOperationCompleted, userState);
        }
        
        private void OnCmsCreditDomainListsOperationCompleted(object arg) {
            if ((this.CmsCreditDomainListsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CmsCreditDomainListsCompleted(this, new CmsCreditDomainListsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://services.godaddy.com/IsDomainValidForInstantPage", RequestNamespace="http://services.godaddy.com/", ResponseNamespace="http://services.godaddy.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string IsDomainValidForInstantPage(string domainValidityRequestXml) {
            object[] results = this.Invoke("IsDomainValidForInstantPage", new object[] {
                        domainValidityRequestXml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void IsDomainValidForInstantPageAsync(string domainValidityRequestXml) {
            this.IsDomainValidForInstantPageAsync(domainValidityRequestXml, null);
        }
        
        /// <remarks/>
        public void IsDomainValidForInstantPageAsync(string domainValidityRequestXml, object userState) {
            if ((this.IsDomainValidForInstantPageOperationCompleted == null)) {
                this.IsDomainValidForInstantPageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnIsDomainValidForInstantPageOperationCompleted);
            }
            this.InvokeAsync("IsDomainValidForInstantPage", new object[] {
                        domainValidityRequestXml}, this.IsDomainValidForInstantPageOperationCompleted, userState);
        }
        
        private void OnIsDomainValidForInstantPageOperationCompleted(object arg) {
            if ((this.IsDomainValidForInstantPageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.IsDomainValidForInstantPageCompleted(this, new IsDomainValidForInstantPageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void CmsCreditDomainListsCompletedEventHandler(object sender, CmsCreditDomainListsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CmsCreditDomainListsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CmsCreditDomainListsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void IsDomainValidForInstantPageCompletedEventHandler(object sender, IsDomainValidForInstantPageCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IsDomainValidForInstantPageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal IsDomainValidForInstantPageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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