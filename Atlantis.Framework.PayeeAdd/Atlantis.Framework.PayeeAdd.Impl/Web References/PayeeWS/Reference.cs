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

namespace Atlantis.Framework.PayeeAdd.Impl.PayeeWS
{


  /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSCgdCAPServiceSoap", Namespace="urn:WSCgdCAPService")]
    public partial class WSCgdCAPService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetEnumerationDescriptionsOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateAccountOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddAccountOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAccountsForShopperOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAccountDetailOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSCgdCAPService() {
            this.Url = global::Atlantis.Framework.PayeeAdd.Impl.Properties.Settings.Default.Atlantis_Framework_PayeeAdd_Impl_PayeeWS_WSCgdCAPService;
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
        public event GetEnumerationDescriptionsCompletedEventHandler GetEnumerationDescriptionsCompleted;
        
        /// <remarks/>
        public event UpdateAccountCompletedEventHandler UpdateAccountCompleted;
        
        /// <remarks/>
        public event AddAccountCompletedEventHandler AddAccountCompleted;
        
        /// <remarks/>
        public event GetAccountsForShopperCompletedEventHandler GetAccountsForShopperCompleted;
        
        /// <remarks/>
        public event GetAccountDetailCompletedEventHandler GetAccountDetailCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#GetEnumerationDescriptions", RequestNamespace="urn:WSCgdCAPService", ResponseNamespace="urn:WSCgdCAPService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string GetEnumerationDescriptions(string bstrType) {
            object[] results = this.Invoke("GetEnumerationDescriptions", new object[] {
                        bstrType});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetEnumerationDescriptionsAsync(string bstrType) {
            this.GetEnumerationDescriptionsAsync(bstrType, null);
        }
        
        /// <remarks/>
        public void GetEnumerationDescriptionsAsync(string bstrType, object userState) {
            if ((this.GetEnumerationDescriptionsOperationCompleted == null)) {
                this.GetEnumerationDescriptionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetEnumerationDescriptionsOperationCompleted);
            }
            this.InvokeAsync("GetEnumerationDescriptions", new object[] {
                        bstrType}, this.GetEnumerationDescriptionsOperationCompleted, userState);
        }
        
        private void OnGetEnumerationDescriptionsOperationCompleted(object arg) {
            if ((this.GetEnumerationDescriptionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetEnumerationDescriptionsCompleted(this, new GetEnumerationDescriptionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#UpdateAccount", RequestNamespace="urn:WSCgdCAPService", ResponseNamespace="urn:WSCgdCAPService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string UpdateAccount(string bstrUpdateXML) {
            object[] results = this.Invoke("UpdateAccount", new object[] {
                        bstrUpdateXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateAccountAsync(string bstrUpdateXML) {
            this.UpdateAccountAsync(bstrUpdateXML, null);
        }
        
        /// <remarks/>
        public void UpdateAccountAsync(string bstrUpdateXML, object userState) {
            if ((this.UpdateAccountOperationCompleted == null)) {
                this.UpdateAccountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateAccountOperationCompleted);
            }
            this.InvokeAsync("UpdateAccount", new object[] {
                        bstrUpdateXML}, this.UpdateAccountOperationCompleted, userState);
        }
        
        private void OnUpdateAccountOperationCompleted(object arg) {
            if ((this.UpdateAccountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateAccountCompleted(this, new UpdateAccountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#AddAccount", RequestNamespace="urn:WSCgdCAPService", ResponseNamespace="urn:WSCgdCAPService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string AddAccount(string bstrAddXML) {
            object[] results = this.Invoke("AddAccount", new object[] {
                        bstrAddXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void AddAccountAsync(string bstrAddXML) {
            this.AddAccountAsync(bstrAddXML, null);
        }
        
        /// <remarks/>
        public void AddAccountAsync(string bstrAddXML, object userState) {
            if ((this.AddAccountOperationCompleted == null)) {
                this.AddAccountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddAccountOperationCompleted);
            }
            this.InvokeAsync("AddAccount", new object[] {
                        bstrAddXML}, this.AddAccountOperationCompleted, userState);
        }
        
        private void OnAddAccountOperationCompleted(object arg) {
            if ((this.AddAccountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddAccountCompleted(this, new AddAccountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#GetAccountsForShopper", RequestNamespace="urn:WSCgdCAPService", ResponseNamespace="urn:WSCgdCAPService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string GetAccountsForShopper(string bstrShopperID) {
            object[] results = this.Invoke("GetAccountsForShopper", new object[] {
                        bstrShopperID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAccountsForShopperAsync(string bstrShopperID) {
            this.GetAccountsForShopperAsync(bstrShopperID, null);
        }
        
        /// <remarks/>
        public void GetAccountsForShopperAsync(string bstrShopperID, object userState) {
            if ((this.GetAccountsForShopperOperationCompleted == null)) {
                this.GetAccountsForShopperOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAccountsForShopperOperationCompleted);
            }
            this.InvokeAsync("GetAccountsForShopper", new object[] {
                        bstrShopperID}, this.GetAccountsForShopperOperationCompleted, userState);
        }
        
        private void OnGetAccountsForShopperOperationCompleted(object arg) {
            if ((this.GetAccountsForShopperCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAccountsForShopperCompleted(this, new GetAccountsForShopperCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#GetAccountDetail", RequestNamespace="urn:WSCgdCAPService", ResponseNamespace="urn:WSCgdCAPService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string GetAccountDetail(string bstrShopperID, int lCAPID) {
            object[] results = this.Invoke("GetAccountDetail", new object[] {
                        bstrShopperID,
                        lCAPID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAccountDetailAsync(string bstrShopperID, int lCAPID) {
            this.GetAccountDetailAsync(bstrShopperID, lCAPID, null);
        }
        
        /// <remarks/>
        public void GetAccountDetailAsync(string bstrShopperID, int lCAPID, object userState) {
            if ((this.GetAccountDetailOperationCompleted == null)) {
                this.GetAccountDetailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAccountDetailOperationCompleted);
            }
            this.InvokeAsync("GetAccountDetail", new object[] {
                        bstrShopperID,
                        lCAPID}, this.GetAccountDetailOperationCompleted, userState);
        }
        
        private void OnGetAccountDetailOperationCompleted(object arg) {
            if ((this.GetAccountDetailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAccountDetailCompleted(this, new GetAccountDetailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void GetEnumerationDescriptionsCompletedEventHandler(object sender, GetEnumerationDescriptionsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetEnumerationDescriptionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetEnumerationDescriptionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void UpdateAccountCompletedEventHandler(object sender, UpdateAccountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateAccountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateAccountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void AddAccountCompletedEventHandler(object sender, AddAccountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddAccountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddAccountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void GetAccountsForShopperCompletedEventHandler(object sender, GetAccountsForShopperCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAccountsForShopperCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAccountsForShopperCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void GetAccountDetailCompletedEventHandler(object sender, GetAccountDetailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAccountDetailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAccountDetailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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