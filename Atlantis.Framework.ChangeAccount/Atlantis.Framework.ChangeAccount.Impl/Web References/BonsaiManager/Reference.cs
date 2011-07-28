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

namespace Atlantis.Framework.ChangeAccount.Impl.BonsaiManager {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="#Bonsai")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetAccountXmlOperationCompleted;
        
        private System.Threading.SendOrPostCallback ChangeAccountRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRenewalOptionsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTransitionsAndRankOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::Atlantis.Framework.ChangeAccount.Impl.Properties.Settings.Default.Atlantis_Framework_ChangeAccount_Impl_BonsaiManager_Service;
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
        public event GetAccountXmlCompletedEventHandler GetAccountXmlCompleted;
        
        /// <remarks/>
        public event ChangeAccountRequestCompletedEventHandler ChangeAccountRequestCompleted;
        
        /// <remarks/>
        public event GetRenewalOptionsCompletedEventHandler GetRenewalOptionsCompleted;
        
        /// <remarks/>
        public event GetTransitionsAndRankCompletedEventHandler GetTransitionsAndRankCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("#Bonsai/GetAccountXml", RequestNamespace="#Bonsai", ResponseNamespace="#Bonsai", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultCode")]
        public int GetAccountXml(string ResourceID, string ResourceType, string IDType, int TreeID, int PrivateLabelID, out System.Xml.XmlNode AccountXml) {
            object[] results = this.Invoke("GetAccountXml", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        TreeID,
                        PrivateLabelID});
            AccountXml = ((System.Xml.XmlNode)(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetAccountXmlAsync(string ResourceID, string ResourceType, string IDType, int TreeID, int PrivateLabelID) {
            this.GetAccountXmlAsync(ResourceID, ResourceType, IDType, TreeID, PrivateLabelID, null);
        }
        
        /// <remarks/>
        public void GetAccountXmlAsync(string ResourceID, string ResourceType, string IDType, int TreeID, int PrivateLabelID, object userState) {
            if ((this.GetAccountXmlOperationCompleted == null)) {
                this.GetAccountXmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAccountXmlOperationCompleted);
            }
            this.InvokeAsync("GetAccountXml", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        TreeID,
                        PrivateLabelID}, this.GetAccountXmlOperationCompleted, userState);
        }
        
        private void OnGetAccountXmlOperationCompleted(object arg) {
            if ((this.GetAccountXmlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAccountXmlCompleted(this, new GetAccountXmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("#Bonsai/ChangeAccountRequest", RequestNamespace="#Bonsai", ResponseNamespace="#Bonsai", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int ChangeAccountRequest(string ResourceID, string ResourceType, string IDType, string AccountChangeXml, int RenewalPFID, int RenewalPeriods, string ItemRequestXml, out int ResultCode) {
            object[] results = this.Invoke("ChangeAccountRequest", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        AccountChangeXml,
                        RenewalPFID,
                        RenewalPeriods,
                        ItemRequestXml});
            ResultCode = ((int)(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void ChangeAccountRequestAsync(string ResourceID, string ResourceType, string IDType, string AccountChangeXml, int RenewalPFID, int RenewalPeriods, string ItemRequestXml) {
            this.ChangeAccountRequestAsync(ResourceID, ResourceType, IDType, AccountChangeXml, RenewalPFID, RenewalPeriods, ItemRequestXml, null);
        }
        
        /// <remarks/>
        public void ChangeAccountRequestAsync(string ResourceID, string ResourceType, string IDType, string AccountChangeXml, int RenewalPFID, int RenewalPeriods, string ItemRequestXml, object userState) {
            if ((this.ChangeAccountRequestOperationCompleted == null)) {
                this.ChangeAccountRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnChangeAccountRequestOperationCompleted);
            }
            this.InvokeAsync("ChangeAccountRequest", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        AccountChangeXml,
                        RenewalPFID,
                        RenewalPeriods,
                        ItemRequestXml}, this.ChangeAccountRequestOperationCompleted, userState);
        }
        
        private void OnChangeAccountRequestOperationCompleted(object arg) {
            if ((this.ChangeAccountRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ChangeAccountRequestCompleted(this, new ChangeAccountRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("#Bonsai/GetRenewalOptions", RequestNamespace="#Bonsai", ResponseNamespace="#Bonsai", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public RenewalResponse GetRenewalOptions(string ResourceID, string ResourceType, string IDType, int PrivateLabelID) {
            object[] results = this.Invoke("GetRenewalOptions", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        PrivateLabelID});
            return ((RenewalResponse)(results[0]));
        }
        
        /// <remarks/>
        public void GetRenewalOptionsAsync(string ResourceID, string ResourceType, string IDType, int PrivateLabelID) {
            this.GetRenewalOptionsAsync(ResourceID, ResourceType, IDType, PrivateLabelID, null);
        }
        
        /// <remarks/>
        public void GetRenewalOptionsAsync(string ResourceID, string ResourceType, string IDType, int PrivateLabelID, object userState) {
            if ((this.GetRenewalOptionsOperationCompleted == null)) {
                this.GetRenewalOptionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRenewalOptionsOperationCompleted);
            }
            this.InvokeAsync("GetRenewalOptions", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        PrivateLabelID}, this.GetRenewalOptionsOperationCompleted, userState);
        }
        
        private void OnGetRenewalOptionsOperationCompleted(object arg) {
            if ((this.GetRenewalOptionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRenewalOptionsCompleted(this, new GetRenewalOptionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("#Bonsai/GetTransitionsAndRank", RequestNamespace="#Bonsai", ResponseNamespace="#Bonsai", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitionResponse GetTransitionsAndRank(string ResourceID, string ResourceType, string IDType, int UnifiedProductID) {
            object[] results = this.Invoke("GetTransitionsAndRank", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        UnifiedProductID});
            return ((TransitionResponse)(results[0]));
        }
        
        /// <remarks/>
        public void GetTransitionsAndRankAsync(string ResourceID, string ResourceType, string IDType, int UnifiedProductID) {
            this.GetTransitionsAndRankAsync(ResourceID, ResourceType, IDType, UnifiedProductID, null);
        }
        
        /// <remarks/>
        public void GetTransitionsAndRankAsync(string ResourceID, string ResourceType, string IDType, int UnifiedProductID, object userState) {
            if ((this.GetTransitionsAndRankOperationCompleted == null)) {
                this.GetTransitionsAndRankOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTransitionsAndRankOperationCompleted);
            }
            this.InvokeAsync("GetTransitionsAndRank", new object[] {
                        ResourceID,
                        ResourceType,
                        IDType,
                        UnifiedProductID}, this.GetTransitionsAndRankOperationCompleted, userState);
        }
        
        private void OnGetTransitionsAndRankOperationCompleted(object arg) {
            if ((this.GetTransitionsAndRankCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTransitionsAndRankCompleted(this, new GetTransitionsAndRankCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="#Bonsai")]
    public partial class RenewalResponse {
        
        private RenewalOption[] renewalOptionsField;
        
        private int resultCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public RenewalOption[] RenewalOptions {
            get {
                return this.renewalOptionsField;
            }
            set {
                this.renewalOptionsField = value;
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="#Bonsai")]
    public partial class RenewalOption {
        
        private int unifiedProductIDField;
        
        private int minRenewalPeriodsField;
        
        private int maxRenewalPeriodsField;
        
        private int renewalLengthField;
        
        private string renewalTypeField;
        
        /// <remarks/>
        public int UnifiedProductID {
            get {
                return this.unifiedProductIDField;
            }
            set {
                this.unifiedProductIDField = value;
            }
        }
        
        /// <remarks/>
        public int MinRenewalPeriods {
            get {
                return this.minRenewalPeriodsField;
            }
            set {
                this.minRenewalPeriodsField = value;
            }
        }
        
        /// <remarks/>
        public int MaxRenewalPeriods {
            get {
                return this.maxRenewalPeriodsField;
            }
            set {
                this.maxRenewalPeriodsField = value;
            }
        }
        
        /// <remarks/>
        public int RenewalLength {
            get {
                return this.renewalLengthField;
            }
            set {
                this.renewalLengthField = value;
            }
        }
        
        /// <remarks/>
        public string RenewalType {
            get {
                return this.renewalTypeField;
            }
            set {
                this.renewalTypeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="#Bonsai")]
    public partial class RankTransition {
        
        private string nodeNameField;
        
        private int nodeRankField;
        
        private int unifiedProductIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NodeName {
            get {
                return this.nodeNameField;
            }
            set {
                this.nodeNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int NodeRank {
            get {
                return this.nodeRankField;
            }
            set {
                this.nodeRankField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int UnifiedProductID {
            get {
                return this.unifiedProductIDField;
            }
            set {
                this.unifiedProductIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="#Bonsai")]
    public partial class TransitionResponse {
        
        private int resultCodeField;
        
        private RankTransition[] transitionsField;
        
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
        public RankTransition[] Transitions {
            get {
                return this.transitionsField;
            }
            set {
                this.transitionsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetAccountXmlCompletedEventHandler(object sender, GetAccountXmlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAccountXmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAccountXmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
        public System.Xml.XmlNode AccountXml {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ChangeAccountRequestCompletedEventHandler(object sender, ChangeAccountRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ChangeAccountRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ChangeAccountRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
        public int ResultCode {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetRenewalOptionsCompletedEventHandler(object sender, GetRenewalOptionsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRenewalOptionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRenewalOptionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RenewalResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RenewalResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetTransitionsAndRankCompletedEventHandler(object sender, GetTransitionsAndRankCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTransitionsAndRankCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTransitionsAndRankCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitionResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitionResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591