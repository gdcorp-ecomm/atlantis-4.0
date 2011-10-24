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

namespace Atlantis.Framework.FastballLogOrder.Impl.FastballOrderTracking
{


  /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="OrderSoap", Namespace="http://tempuri.org/")]
    public partial class Order : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback LogOrderOperationCompleted;
        
        private System.Threading.SendOrPostCallback LogOrderWithTypeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Order() {
            this.Url = global::Atlantis.Framework.FastballLogOrder.Impl.Properties.Settings.Default.Atlantis_Framework_FastballLogOrder_Impl_FastballOrderTracking_Order;
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
        public event LogOrderCompletedEventHandler LogOrderCompleted;
        
        /// <remarks/>
        public event LogOrderWithTypeCompletedEventHandler LogOrderWithTypeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LogOrder", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void LogOrder(System.Guid visitGuid, string orderId, int sequenceId) {
            this.Invoke("LogOrder", new object[] {
                        visitGuid,
                        orderId,
                        sequenceId});
        }
        
        /// <remarks/>
        public void LogOrderAsync(System.Guid visitGuid, string orderId, int sequenceId) {
            this.LogOrderAsync(visitGuid, orderId, sequenceId, null);
        }
        
        /// <remarks/>
        public void LogOrderAsync(System.Guid visitGuid, string orderId, int sequenceId, object userState) {
            if ((this.LogOrderOperationCompleted == null)) {
                this.LogOrderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLogOrderOperationCompleted);
            }
            this.InvokeAsync("LogOrder", new object[] {
                        visitGuid,
                        orderId,
                        sequenceId}, this.LogOrderOperationCompleted, userState);
        }
        
        private void OnLogOrderOperationCompleted(object arg) {
            if ((this.LogOrderCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LogOrderCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LogOrderWithType", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void LogOrderWithType(System.Guid visitGuid, string orderId, int sequenceId, string basketType) {
            this.Invoke("LogOrderWithType", new object[] {
                        visitGuid,
                        orderId,
                        sequenceId,
                        basketType});
        }
        
        /// <remarks/>
        public void LogOrderWithTypeAsync(System.Guid visitGuid, string orderId, int sequenceId, string basketType) {
            this.LogOrderWithTypeAsync(visitGuid, orderId, sequenceId, basketType, null);
        }
        
        /// <remarks/>
        public void LogOrderWithTypeAsync(System.Guid visitGuid, string orderId, int sequenceId, string basketType, object userState) {
            if ((this.LogOrderWithTypeOperationCompleted == null)) {
                this.LogOrderWithTypeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLogOrderWithTypeOperationCompleted);
            }
            this.InvokeAsync("LogOrderWithType", new object[] {
                        visitGuid,
                        orderId,
                        sequenceId,
                        basketType}, this.LogOrderWithTypeOperationCompleted, userState);
        }
        
        private void OnLogOrderWithTypeOperationCompleted(object arg) {
            if ((this.LogOrderWithTypeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LogOrderWithTypeCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void LogOrderCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void LogOrderWithTypeCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591