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

namespace Atlantis.Framework.NameMatch.Impl.NameMatchService {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="gdDppSearchWSSoap", Namespace="http://auctions.godaddy.com")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Name))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AvailableDomain[]))]
    public partial class gdDppSearchWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getServiceStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback getServiceStatisticsOperationCompleted;
        
        private System.Threading.SendOrPostCallback dppDomainSearchOperationCompleted;
        
        private System.Threading.SendOrPostCallback exactMatchDomainSearchOperationCompleted;
        
        private System.Threading.SendOrPostCallback keywordSpinDomainSearchOperationCompleted;
        
        private System.Threading.SendOrPostCallback dppAvailableDomainsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public gdDppSearchWS() {
            this.Url = global::Atlantis.Framework.NameMatch.Impl.Properties.Settings.Default.Atlantis_Framework_NameMatch_Impl_NameMatchService_gdDppSearchWS;
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
        public event getServiceStatusCompletedEventHandler getServiceStatusCompleted;
        
        /// <remarks/>
        public event getServiceStatisticsCompletedEventHandler getServiceStatisticsCompleted;
        
        /// <remarks/>
        public event dppDomainSearchCompletedEventHandler dppDomainSearchCompleted;
        
        /// <remarks/>
        public event exactMatchDomainSearchCompletedEventHandler exactMatchDomainSearchCompleted;
        
        /// <remarks/>
        public event keywordSpinDomainSearchCompletedEventHandler keywordSpinDomainSearchCompleted;
        
        /// <remarks/>
        public event dppAvailableDomainsCompletedEventHandler dppAvailableDomainsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://auctions.godaddy.com/getServiceStatus", RequestNamespace="http://auctions.godaddy.com", ResponseNamespace="http://auctions.godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string getServiceStatus() {
            object[] results = this.Invoke("getServiceStatus", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetServiceStatus(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getServiceStatus", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string EndgetServiceStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getServiceStatusAsync() {
            this.getServiceStatusAsync(null);
        }
        
        /// <remarks/>
        public void getServiceStatusAsync(object userState) {
            if ((this.getServiceStatusOperationCompleted == null)) {
                this.getServiceStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetServiceStatusOperationCompleted);
            }
            this.InvokeAsync("getServiceStatus", new object[0], this.getServiceStatusOperationCompleted, userState);
        }
        
        private void OngetServiceStatusOperationCompleted(object arg) {
            if ((this.getServiceStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getServiceStatusCompleted(this, new getServiceStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://auctions.godaddy.com/getServiceStatistics", RequestNamespace="http://auctions.godaddy.com", ResponseNamespace="http://auctions.godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string getServiceStatistics(string token) {
            object[] results = this.Invoke("getServiceStatistics", new object[] {
                        token});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetServiceStatistics(string token, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getServiceStatistics", new object[] {
                        token}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndgetServiceStatistics(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getServiceStatisticsAsync(string token) {
            this.getServiceStatisticsAsync(token, null);
        }
        
        /// <remarks/>
        public void getServiceStatisticsAsync(string token, object userState) {
            if ((this.getServiceStatisticsOperationCompleted == null)) {
                this.getServiceStatisticsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetServiceStatisticsOperationCompleted);
            }
            this.InvokeAsync("getServiceStatistics", new object[] {
                        token}, this.getServiceStatisticsOperationCompleted, userState);
        }
        
        private void OngetServiceStatisticsOperationCompleted(object arg) {
            if ((this.getServiceStatisticsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getServiceStatisticsCompleted(this, new getServiceStatisticsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://auctions.godaddy.com/dppDomainSearch", RequestNamespace="http://auctions.godaddy.com", ResponseNamespace="http://auctions.godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string dppDomainSearch(string requestXML) {
            object[] results = this.Invoke("dppDomainSearch", new object[] {
                        requestXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegindppDomainSearch(string requestXML, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("dppDomainSearch", new object[] {
                        requestXML}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EnddppDomainSearch(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void dppDomainSearchAsync(string requestXML) {
            this.dppDomainSearchAsync(requestXML, null);
        }
        
        /// <remarks/>
        public void dppDomainSearchAsync(string requestXML, object userState) {
            if ((this.dppDomainSearchOperationCompleted == null)) {
                this.dppDomainSearchOperationCompleted = new System.Threading.SendOrPostCallback(this.OndppDomainSearchOperationCompleted);
            }
            this.InvokeAsync("dppDomainSearch", new object[] {
                        requestXML}, this.dppDomainSearchOperationCompleted, userState);
        }
        
        private void OndppDomainSearchOperationCompleted(object arg) {
            if ((this.dppDomainSearchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.dppDomainSearchCompleted(this, new dppDomainSearchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://auctions.godaddy.com/exactMatchDomainSearch", RequestNamespace="http://auctions.godaddy.com", ResponseNamespace="http://auctions.godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string exactMatchDomainSearch(string requestXML) {
            object[] results = this.Invoke("exactMatchDomainSearch", new object[] {
                        requestXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginexactMatchDomainSearch(string requestXML, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("exactMatchDomainSearch", new object[] {
                        requestXML}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndexactMatchDomainSearch(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void exactMatchDomainSearchAsync(string requestXML) {
            this.exactMatchDomainSearchAsync(requestXML, null);
        }
        
        /// <remarks/>
        public void exactMatchDomainSearchAsync(string requestXML, object userState) {
            if ((this.exactMatchDomainSearchOperationCompleted == null)) {
                this.exactMatchDomainSearchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnexactMatchDomainSearchOperationCompleted);
            }
            this.InvokeAsync("exactMatchDomainSearch", new object[] {
                        requestXML}, this.exactMatchDomainSearchOperationCompleted, userState);
        }
        
        private void OnexactMatchDomainSearchOperationCompleted(object arg) {
            if ((this.exactMatchDomainSearchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.exactMatchDomainSearchCompleted(this, new exactMatchDomainSearchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://auctions.godaddy.com/keywordSpinDomainSearch", RequestNamespace="http://auctions.godaddy.com", ResponseNamespace="http://auctions.godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string keywordSpinDomainSearch(string requestXML) {
            object[] results = this.Invoke("keywordSpinDomainSearch", new object[] {
                        requestXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginkeywordSpinDomainSearch(string requestXML, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("keywordSpinDomainSearch", new object[] {
                        requestXML}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndkeywordSpinDomainSearch(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void keywordSpinDomainSearchAsync(string requestXML) {
            this.keywordSpinDomainSearchAsync(requestXML, null);
        }
        
        /// <remarks/>
        public void keywordSpinDomainSearchAsync(string requestXML, object userState) {
            if ((this.keywordSpinDomainSearchOperationCompleted == null)) {
                this.keywordSpinDomainSearchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnkeywordSpinDomainSearchOperationCompleted);
            }
            this.InvokeAsync("keywordSpinDomainSearch", new object[] {
                        requestXML}, this.keywordSpinDomainSearchOperationCompleted, userState);
        }
        
        private void OnkeywordSpinDomainSearchOperationCompleted(object arg) {
            if ((this.keywordSpinDomainSearchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.keywordSpinDomainSearchCompleted(this, new keywordSpinDomainSearchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://auctions.godaddy.com/dppAvailableDomains", RequestNamespace="http://auctions.godaddy.com", ResponseNamespace="http://auctions.godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public AvailableDomain[] dppAvailableDomains(string requestXML) {
            object[] results = this.Invoke("dppAvailableDomains", new object[] {
                        requestXML});
            return ((AvailableDomain[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegindppAvailableDomains(string requestXML, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("dppAvailableDomains", new object[] {
                        requestXML}, callback, asyncState);
        }
        
        /// <remarks/>
        public AvailableDomain[] EnddppAvailableDomains(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((AvailableDomain[])(results[0]));
        }
        
        /// <remarks/>
        public void dppAvailableDomainsAsync(string requestXML) {
            this.dppAvailableDomainsAsync(requestXML, null);
        }
        
        /// <remarks/>
        public void dppAvailableDomainsAsync(string requestXML, object userState) {
            if ((this.dppAvailableDomainsOperationCompleted == null)) {
                this.dppAvailableDomainsOperationCompleted = new System.Threading.SendOrPostCallback(this.OndppAvailableDomainsOperationCompleted);
            }
            this.InvokeAsync("dppAvailableDomains", new object[] {
                        requestXML}, this.dppAvailableDomainsOperationCompleted, userState);
        }
        
        private void OndppAvailableDomainsOperationCompleted(object arg) {
            if ((this.dppAvailableDomainsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.dppAvailableDomainsCompleted(this, new dppAvailableDomainsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://auctions.godaddy.com")]
    public partial class AvailableDomain : Domain {
        
        private string anchorWordField;
        
        private string searchMethodField;
        
        private bool availCheckPerformedField;
        
        private string domainAvailableField;
        
        private string fullDomainNameField;
        
        private string tLDField;
        
        private string sLDField;
        
        /// <remarks/>
        public string AnchorWord {
            get {
                return this.anchorWordField;
            }
            set {
                this.anchorWordField = value;
            }
        }
        
        /// <remarks/>
        public string SearchMethod {
            get {
                return this.searchMethodField;
            }
            set {
                this.searchMethodField = value;
            }
        }
        
        /// <remarks/>
        public bool AvailCheckPerformed {
            get {
                return this.availCheckPerformedField;
            }
            set {
                this.availCheckPerformedField = value;
            }
        }
        
        /// <remarks/>
        public string DomainAvailable {
            get {
                return this.domainAvailableField;
            }
            set {
                this.domainAvailableField = value;
            }
        }
        
        /// <remarks/>
        public string FullDomainName {
            get {
                return this.fullDomainNameField;
            }
            set {
                this.fullDomainNameField = value;
            }
        }
        
        /// <remarks/>
        public string TLD {
            get {
                return this.tLDField;
            }
            set {
                this.tLDField = value;
            }
        }
        
        /// <remarks/>
        public string SLD {
            get {
                return this.sLDField;
            }
            set {
                this.sLDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AvailableDomain))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Domainsbot.FirstImpact")]
    public partial class Domain : Name {
        
        private string extensionField;
        
        private string domainNameField;
        
        /// <remarks/>
        public string Extension {
            get {
                return this.extensionField;
            }
            set {
                this.extensionField = value;
            }
        }
        
        /// <remarks/>
        public string DomainName {
            get {
                return this.domainNameField;
            }
            set {
                this.domainNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GridDomain))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Domain))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AvailableDomain))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Domainsbot.FirstImpact")]
    public partial class Name {
        
        private string nameWithoutExtensionField;
        
        private string[] keysField;
        
        private DomainData[][] dataField;
        
        /// <remarks/>
        public string NameWithoutExtension {
            get {
                return this.nameWithoutExtensionField;
            }
            set {
                this.nameWithoutExtensionField = value;
            }
        }
        
        /// <remarks/>
        public string[] Keys {
            get {
                return this.keysField;
            }
            set {
                this.keysField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ArrayOfDomainData")]
        [System.Xml.Serialization.XmlArrayItemAttribute(NestingLevel=1)]
        public DomainData[][] Data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Domainsbot.FirstImpact")]
    public partial class DomainData {
        
        private string nameField;
        
        private object dataField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public object Data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Domainsbot.FirstImpact")]
    public partial class GridExtension {
        
        private DomainStatus statusField;
        
        private DomainData[][] dataField;
        
        private string extensionField;
        
        /// <remarks/>
        public DomainStatus Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ArrayOfDomainData")]
        [System.Xml.Serialization.XmlArrayItemAttribute(NestingLevel=1)]
        public DomainData[][] Data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
            }
        }
        
        /// <remarks/>
        public string Extension {
            get {
                return this.extensionField;
            }
            set {
                this.extensionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Domainsbot.FirstImpact")]
    public enum DomainStatus {
        
        /// <remarks/>
        Available,
        
        /// <remarks/>
        Registered,
        
        /// <remarks/>
        Database,
        
        /// <remarks/>
        Unknown,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Domainsbot.FirstImpact")]
    public partial class GridDomain : Name {
        
        private GridExtension[] extensionsField;
        
        /// <remarks/>
        public GridExtension[] Extensions {
            get {
                return this.extensionsField;
            }
            set {
                this.extensionsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getServiceStatusCompletedEventHandler(object sender, getServiceStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getServiceStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getServiceStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void getServiceStatisticsCompletedEventHandler(object sender, getServiceStatisticsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getServiceStatisticsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getServiceStatisticsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void dppDomainSearchCompletedEventHandler(object sender, dppDomainSearchCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class dppDomainSearchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal dppDomainSearchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void exactMatchDomainSearchCompletedEventHandler(object sender, exactMatchDomainSearchCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class exactMatchDomainSearchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal exactMatchDomainSearchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void keywordSpinDomainSearchCompletedEventHandler(object sender, keywordSpinDomainSearchCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class keywordSpinDomainSearchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal keywordSpinDomainSearchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void dppAvailableDomainsCompletedEventHandler(object sender, dppAvailableDomainsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class dppAvailableDomainsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal dppAvailableDomainsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AvailableDomain[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AvailableDomain[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591