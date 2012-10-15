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

namespace Atlantis.Framework.DCCICannConfirm.Impl.ICannDsweb {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="httpBinding_IRegICANNConfirmWebSvc", Namespace="http://godaddy.com")]
    public partial class httpBinding_IRegICANNConfirmWebSvc : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetComplianceEmailOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetIcannDomainDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback TestOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetVersionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public httpBinding_IRegICANNConfirmWebSvc() {
            this.Url = global::Atlantis.Framework.DCCICannConfirm.Impl.Properties.Settings.Default.Atlantis_Framework_DCCICannConfirm_Impl_ICannDsweb_RegICANNConfirmWebSvcService;
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
        public event GetComplianceEmailCompletedEventHandler GetComplianceEmailCompleted;
        
        /// <remarks/>
        public event GetIcannDomainDataCompletedEventHandler GetIcannDomainDataCompleted;
        
        /// <remarks/>
        public event TestCompletedEventHandler TestCompleted;
        
        /// <remarks/>
        public event GetVersionCompletedEventHandler GetVersionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/GetComplianceEmail", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetComplianceEmail([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string RequestXml) {
            object[] results = this.Invoke("GetComplianceEmail", new object[] {
                        RequestXml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetComplianceEmailAsync(string RequestXml) {
            this.GetComplianceEmailAsync(RequestXml, null);
        }
        
        /// <remarks/>
        public void GetComplianceEmailAsync(string RequestXml, object userState) {
            if ((this.GetComplianceEmailOperationCompleted == null)) {
                this.GetComplianceEmailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetComplianceEmailOperationCompleted);
            }
            this.InvokeAsync("GetComplianceEmail", new object[] {
                        RequestXml}, this.GetComplianceEmailOperationCompleted, userState);
        }
        
        private void OnGetComplianceEmailOperationCompleted(object arg) {
            if ((this.GetComplianceEmailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetComplianceEmailCompleted(this, new GetComplianceEmailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/GetIcannDomainData", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetIcannDomainData([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string RequestXml) {
            object[] results = this.Invoke("GetIcannDomainData", new object[] {
                        RequestXml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetIcannDomainDataAsync(string RequestXml) {
            this.GetIcannDomainDataAsync(RequestXml, null);
        }
        
        /// <remarks/>
        public void GetIcannDomainDataAsync(string RequestXml, object userState) {
            if ((this.GetIcannDomainDataOperationCompleted == null)) {
                this.GetIcannDomainDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetIcannDomainDataOperationCompleted);
            }
            this.InvokeAsync("GetIcannDomainData", new object[] {
                        RequestXml}, this.GetIcannDomainDataOperationCompleted, userState);
        }
        
        private void OnGetIcannDomainDataOperationCompleted(object arg) {
            if ((this.GetIcannDomainDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetIcannDomainDataCompleted(this, new GetIcannDomainDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/Test", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Test() {
            object[] results = this.Invoke("Test", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void TestAsync() {
            this.TestAsync(null);
        }
        
        /// <remarks/>
        public void TestAsync(object userState) {
            if ((this.TestOperationCompleted == null)) {
                this.TestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestOperationCompleted);
            }
            this.InvokeAsync("Test", new object[0], this.TestOperationCompleted, userState);
        }
        
        private void OnTestOperationCompleted(object arg) {
            if ((this.TestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TestCompleted(this, new TestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/GetVersion", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetVersion() {
            object[] results = this.Invoke("GetVersion", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetVersionAsync() {
            this.GetVersionAsync(null);
        }
        
        /// <remarks/>
        public void GetVersionAsync(object userState) {
            if ((this.GetVersionOperationCompleted == null)) {
                this.GetVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetVersionOperationCompleted);
            }
            this.InvokeAsync("GetVersion", new object[0], this.GetVersionOperationCompleted, userState);
        }
        
        private void OnGetVersionOperationCompleted(object arg) {
            if ((this.GetVersionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetVersionCompleted(this, new GetVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    // CODEGEN: The optional WSDL extension element 'PolicyReference' from namespace 'http://schemas.xmlsoap.org/ws/2004/09/policy' was not handled.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="httpsBinding_IRegICANNConfirmWebSvc", Namespace="http://godaddy.com")]
    public partial class httpsBinding_IRegICANNConfirmWebSvc : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetComplianceEmailOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetIcannDomainDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback TestOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetVersionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public httpsBinding_IRegICANNConfirmWebSvc() {
            this.Url = "https://g1twdsweb01.dc1.corp.gd/RegICANNConfirmWebSvc/RegICANNConfirmWebSvcServic" +
                "e.svc";
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
        public event GetComplianceEmailCompletedEventHandler GetComplianceEmailCompleted;
        
        /// <remarks/>
        public event GetIcannDomainDataCompletedEventHandler GetIcannDomainDataCompleted;
        
        /// <remarks/>
        public event TestCompletedEventHandler TestCompleted;
        
        /// <remarks/>
        public event GetVersionCompletedEventHandler GetVersionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/GetComplianceEmail", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetComplianceEmail([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string RequestXml) {
            object[] results = this.Invoke("GetComplianceEmail", new object[] {
                        RequestXml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetComplianceEmailAsync(string RequestXml) {
            this.GetComplianceEmailAsync(RequestXml, null);
        }
        
        /// <remarks/>
        public void GetComplianceEmailAsync(string RequestXml, object userState) {
            if ((this.GetComplianceEmailOperationCompleted == null)) {
                this.GetComplianceEmailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetComplianceEmailOperationCompleted);
            }
            this.InvokeAsync("GetComplianceEmail", new object[] {
                        RequestXml}, this.GetComplianceEmailOperationCompleted, userState);
        }
        
        private void OnGetComplianceEmailOperationCompleted(object arg) {
            if ((this.GetComplianceEmailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetComplianceEmailCompleted(this, new GetComplianceEmailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/GetIcannDomainData", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetIcannDomainData([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string RequestXml) {
            object[] results = this.Invoke("GetIcannDomainData", new object[] {
                        RequestXml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetIcannDomainDataAsync(string RequestXml) {
            this.GetIcannDomainDataAsync(RequestXml, null);
        }
        
        /// <remarks/>
        public void GetIcannDomainDataAsync(string RequestXml, object userState) {
            if ((this.GetIcannDomainDataOperationCompleted == null)) {
                this.GetIcannDomainDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetIcannDomainDataOperationCompleted);
            }
            this.InvokeAsync("GetIcannDomainData", new object[] {
                        RequestXml}, this.GetIcannDomainDataOperationCompleted, userState);
        }
        
        private void OnGetIcannDomainDataOperationCompleted(object arg) {
            if ((this.GetIcannDomainDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetIcannDomainDataCompleted(this, new GetIcannDomainDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/Test", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Test() {
            object[] results = this.Invoke("Test", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void TestAsync() {
            this.TestAsync(null);
        }
        
        /// <remarks/>
        public void TestAsync(object userState) {
            if ((this.TestOperationCompleted == null)) {
                this.TestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestOperationCompleted);
            }
            this.InvokeAsync("Test", new object[0], this.TestOperationCompleted, userState);
        }
        
        private void OnTestOperationCompleted(object arg) {
            if ((this.TestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TestCompleted(this, new TestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://godaddy.com/IRegICANNConfirmWebSvc/GetVersion", RequestNamespace="http://godaddy.com", ResponseNamespace="http://godaddy.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetVersion() {
            object[] results = this.Invoke("GetVersion", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetVersionAsync() {
            this.GetVersionAsync(null);
        }
        
        /// <remarks/>
        public void GetVersionAsync(object userState) {
            if ((this.GetVersionOperationCompleted == null)) {
                this.GetVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetVersionOperationCompleted);
            }
            this.InvokeAsync("GetVersion", new object[0], this.GetVersionOperationCompleted, userState);
        }
        
        private void OnGetVersionOperationCompleted(object arg) {
            if ((this.GetVersionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetVersionCompleted(this, new GetVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void GetComplianceEmailCompletedEventHandler(object sender, GetComplianceEmailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetComplianceEmailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetComplianceEmailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void GetIcannDomainDataCompletedEventHandler(object sender, GetIcannDomainDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetIcannDomainDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetIcannDomainDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void TestCompletedEventHandler(object sender, TestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void GetVersionCompletedEventHandler(object sender, GetVersionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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