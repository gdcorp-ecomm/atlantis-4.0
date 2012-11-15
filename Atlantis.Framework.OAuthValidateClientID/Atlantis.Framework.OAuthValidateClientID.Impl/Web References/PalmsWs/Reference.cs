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

namespace Atlantis.Framework.OAuthValidateClientId.Impl.PalmsWs {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="Palms")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetLoginOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetMultipleLoginsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLoginByUserNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetPasswordOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetUserNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteLoginOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::Atlantis.Framework.OAuthValidateClientId.Impl.Properties.Settings.Default.Atlantis_Framework_OAuthValidateClientId_Impl_PalmsWs_Service;
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
        public event GetLoginCompletedEventHandler GetLoginCompleted;
        
        /// <remarks/>
        public event GetMultipleLoginsCompletedEventHandler GetMultipleLoginsCompleted;
        
        /// <remarks/>
        public event GetLoginByUserNameCompletedEventHandler GetLoginByUserNameCompleted;
        
        /// <remarks/>
        public event SetPasswordCompletedEventHandler SetPasswordCompleted;
        
        /// <remarks/>
        public event SetUserNameCompletedEventHandler SetUserNameCompleted;
        
        /// <remarks/>
        public event DeleteLoginCompletedEventHandler DeleteLoginCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("Palms/GetLogin", RequestNamespace="Palms", ResponseNamespace="Palms", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int GetLogin(string ApplicationId, string PalmsId, string HostName, string ClientIp, out string UserName, out string Password, out int FailedAuthCount, out string LastAuthSuccess, out string LastAuthAttempt) {
            object[] results = this.Invoke("GetLogin", new object[] {
                        ApplicationId,
                        PalmsId,
                        HostName,
                        ClientIp});
            UserName = ((string)(results[1]));
            Password = ((string)(results[2]));
            FailedAuthCount = ((int)(results[3]));
            LastAuthSuccess = ((string)(results[4]));
            LastAuthAttempt = ((string)(results[5]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetLoginAsync(string ApplicationId, string PalmsId, string HostName, string ClientIp) {
            this.GetLoginAsync(ApplicationId, PalmsId, HostName, ClientIp, null);
        }
        
        /// <remarks/>
        public void GetLoginAsync(string ApplicationId, string PalmsId, string HostName, string ClientIp, object userState) {
            if ((this.GetLoginOperationCompleted == null)) {
                this.GetLoginOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLoginOperationCompleted);
            }
            this.InvokeAsync("GetLogin", new object[] {
                        ApplicationId,
                        PalmsId,
                        HostName,
                        ClientIp}, this.GetLoginOperationCompleted, userState);
        }
        
        private void OnGetLoginOperationCompleted(object arg) {
            if ((this.GetLoginCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLoginCompleted(this, new GetLoginCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("Palms/GetMultipleLogins", RequestNamespace="Palms", ResponseNamespace="Palms", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int GetMultipleLogins(string ApplicationId, [System.Xml.Serialization.XmlElementAttribute("PalmsIds")] string[] PalmsIds, string HostName, string ClientIp, [System.Xml.Serialization.XmlArrayItemAttribute("Login")] out LoginData[] Logins) {
            object[] results = this.Invoke("GetMultipleLogins", new object[] {
                        ApplicationId,
                        PalmsIds,
                        HostName,
                        ClientIp});
            Logins = ((LoginData[])(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetMultipleLoginsAsync(string ApplicationId, string[] PalmsIds, string HostName, string ClientIp) {
            this.GetMultipleLoginsAsync(ApplicationId, PalmsIds, HostName, ClientIp, null);
        }
        
        /// <remarks/>
        public void GetMultipleLoginsAsync(string ApplicationId, string[] PalmsIds, string HostName, string ClientIp, object userState) {
            if ((this.GetMultipleLoginsOperationCompleted == null)) {
                this.GetMultipleLoginsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetMultipleLoginsOperationCompleted);
            }
            this.InvokeAsync("GetMultipleLogins", new object[] {
                        ApplicationId,
                        PalmsIds,
                        HostName,
                        ClientIp}, this.GetMultipleLoginsOperationCompleted, userState);
        }
        
        private void OnGetMultipleLoginsOperationCompleted(object arg) {
            if ((this.GetMultipleLoginsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetMultipleLoginsCompleted(this, new GetMultipleLoginsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("Palms/GetLoginByUserName", RequestNamespace="Palms", ResponseNamespace="Palms", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int GetLoginByUserName(string ApplicationId, string UserName, string HostName, string ClientIp, out string PalmsId, out string Password, out int FailedAuthCount, out string LastAuthSuccess, out string LastAuthAttempt) {
            object[] results = this.Invoke("GetLoginByUserName", new object[] {
                        ApplicationId,
                        UserName,
                        HostName,
                        ClientIp});
            PalmsId = ((string)(results[1]));
            Password = ((string)(results[2]));
            FailedAuthCount = ((int)(results[3]));
            LastAuthSuccess = ((string)(results[4]));
            LastAuthAttempt = ((string)(results[5]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetLoginByUserNameAsync(string ApplicationId, string UserName, string HostName, string ClientIp) {
            this.GetLoginByUserNameAsync(ApplicationId, UserName, HostName, ClientIp, null);
        }
        
        /// <remarks/>
        public void GetLoginByUserNameAsync(string ApplicationId, string UserName, string HostName, string ClientIp, object userState) {
            if ((this.GetLoginByUserNameOperationCompleted == null)) {
                this.GetLoginByUserNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLoginByUserNameOperationCompleted);
            }
            this.InvokeAsync("GetLoginByUserName", new object[] {
                        ApplicationId,
                        UserName,
                        HostName,
                        ClientIp}, this.GetLoginByUserNameOperationCompleted, userState);
        }
        
        private void OnGetLoginByUserNameOperationCompleted(object arg) {
            if ((this.GetLoginByUserNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLoginByUserNameCompleted(this, new GetLoginByUserNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("Palms/SetPassword", RequestNamespace="Palms", ResponseNamespace="Palms", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int SetPassword(string ApplicationId, string PalmsId, string Password, string HostName, string ClientIp) {
            object[] results = this.Invoke("SetPassword", new object[] {
                        ApplicationId,
                        PalmsId,
                        Password,
                        HostName,
                        ClientIp});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void SetPasswordAsync(string ApplicationId, string PalmsId, string Password, string HostName, string ClientIp) {
            this.SetPasswordAsync(ApplicationId, PalmsId, Password, HostName, ClientIp, null);
        }
        
        /// <remarks/>
        public void SetPasswordAsync(string ApplicationId, string PalmsId, string Password, string HostName, string ClientIp, object userState) {
            if ((this.SetPasswordOperationCompleted == null)) {
                this.SetPasswordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetPasswordOperationCompleted);
            }
            this.InvokeAsync("SetPassword", new object[] {
                        ApplicationId,
                        PalmsId,
                        Password,
                        HostName,
                        ClientIp}, this.SetPasswordOperationCompleted, userState);
        }
        
        private void OnSetPasswordOperationCompleted(object arg) {
            if ((this.SetPasswordCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetPasswordCompleted(this, new SetPasswordCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("Palms/SetUserName", RequestNamespace="Palms", ResponseNamespace="Palms", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int SetUserName(string ApplicationId, string PalmsId, string UserName, string HostName, string ClientIp) {
            object[] results = this.Invoke("SetUserName", new object[] {
                        ApplicationId,
                        PalmsId,
                        UserName,
                        HostName,
                        ClientIp});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void SetUserNameAsync(string ApplicationId, string PalmsId, string UserName, string HostName, string ClientIp) {
            this.SetUserNameAsync(ApplicationId, PalmsId, UserName, HostName, ClientIp, null);
        }
        
        /// <remarks/>
        public void SetUserNameAsync(string ApplicationId, string PalmsId, string UserName, string HostName, string ClientIp, object userState) {
            if ((this.SetUserNameOperationCompleted == null)) {
                this.SetUserNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetUserNameOperationCompleted);
            }
            this.InvokeAsync("SetUserName", new object[] {
                        ApplicationId,
                        PalmsId,
                        UserName,
                        HostName,
                        ClientIp}, this.SetUserNameOperationCompleted, userState);
        }
        
        private void OnSetUserNameOperationCompleted(object arg) {
            if ((this.SetUserNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetUserNameCompleted(this, new SetUserNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("Palms/DeleteLogin", RequestNamespace="Palms", ResponseNamespace="Palms", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int DeleteLogin(string ApplicationId, string PalmsId, string HostName, string ClientIp) {
            object[] results = this.Invoke("DeleteLogin", new object[] {
                        ApplicationId,
                        PalmsId,
                        HostName,
                        ClientIp});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void DeleteLoginAsync(string ApplicationId, string PalmsId, string HostName, string ClientIp) {
            this.DeleteLoginAsync(ApplicationId, PalmsId, HostName, ClientIp, null);
        }
        
        /// <remarks/>
        public void DeleteLoginAsync(string ApplicationId, string PalmsId, string HostName, string ClientIp, object userState) {
            if ((this.DeleteLoginOperationCompleted == null)) {
                this.DeleteLoginOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteLoginOperationCompleted);
            }
            this.InvokeAsync("DeleteLogin", new object[] {
                        ApplicationId,
                        PalmsId,
                        HostName,
                        ClientIp}, this.DeleteLoginOperationCompleted, userState);
        }
        
        private void OnDeleteLoginOperationCompleted(object arg) {
            if ((this.DeleteLoginCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteLoginCompleted(this, new DeleteLoginCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="Palms")]
    public partial class LoginData {
        
        private string palmsIdField;
        
        private int resultCodeField;
        
        private string userNameField;
        
        private string passwordField;
        
        private int failedAuthCountField;
        
        private string lastAuthSuccessField;
        
        private string lastAuthAttemptField;
        
        /// <remarks/>
        public string PalmsId {
            get {
                return this.palmsIdField;
            }
            set {
                this.palmsIdField = value;
            }
        }
        
        /// <remarks/>
        public int ResultCode {
            get {
                return this.resultCodeField;
            }
            set {
                this.resultCodeField = value;
            }
        }
        
        /// <remarks/>
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public int FailedAuthCount {
            get {
                return this.failedAuthCountField;
            }
            set {
                this.failedAuthCountField = value;
            }
        }
        
        /// <remarks/>
        public string LastAuthSuccess {
            get {
                return this.lastAuthSuccessField;
            }
            set {
                this.lastAuthSuccessField = value;
            }
        }
        
        /// <remarks/>
        public string LastAuthAttempt {
            get {
                return this.lastAuthAttemptField;
            }
            set {
                this.lastAuthAttemptField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetLoginCompletedEventHandler(object sender, GetLoginCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLoginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLoginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string UserName {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[2]));
            }
        }
        
        /// <remarks/>
        public int FailedAuthCount {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[3]));
            }
        }
        
        /// <remarks/>
        public string LastAuthSuccess {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[4]));
            }
        }
        
        /// <remarks/>
        public string LastAuthAttempt {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[5]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetMultipleLoginsCompletedEventHandler(object sender, GetMultipleLoginsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetMultipleLoginsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetMultipleLoginsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public LoginData[] Logins {
            get {
                this.RaiseExceptionIfNecessary();
                return ((LoginData[])(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetLoginByUserNameCompletedEventHandler(object sender, GetLoginByUserNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLoginByUserNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLoginByUserNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string PalmsId {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[2]));
            }
        }
        
        /// <remarks/>
        public int FailedAuthCount {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[3]));
            }
        }
        
        /// <remarks/>
        public string LastAuthSuccess {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[4]));
            }
        }
        
        /// <remarks/>
        public string LastAuthAttempt {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[5]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SetPasswordCompletedEventHandler(object sender, SetPasswordCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetPasswordCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetPasswordCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SetUserNameCompletedEventHandler(object sender, SetUserNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetUserNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetUserNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void DeleteLoginCompletedEventHandler(object sender, DeleteLoginCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteLoginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteLoginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591