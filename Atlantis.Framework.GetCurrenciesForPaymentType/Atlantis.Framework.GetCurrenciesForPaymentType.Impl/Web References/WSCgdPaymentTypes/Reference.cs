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

namespace Atlantis.Framework.GetCurrenciesForPaymentType.Impl.WSCgdPaymentTypes
{


  /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSCgdPaymentTypesServiceSoap", Namespace="urn:WSCgdPaymentTypesService")]
    public partial class WSCgdPaymentTypesService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetActivePaymentTypesOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetActivePaymentTypesForCurrencyOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetActivePaymentCurrenciesOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAvailableCurrenciesForPaymentTypeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSCgdPaymentTypesService() {
            this.Url = global::Atlantis.Framework.GetCurrenciesForPaymentType.Impl.Properties.Settings.Default.Atlantis_Framework_GetCurrenciesForPaymentType_Impl_WSCgdPaymentTypes_WSCgdPaymentTypesService;
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
        public event GetActivePaymentTypesCompletedEventHandler GetActivePaymentTypesCompleted;
        
        /// <remarks/>
        public event GetActivePaymentTypesForCurrencyCompletedEventHandler GetActivePaymentTypesForCurrencyCompleted;
        
        /// <remarks/>
        public event GetActivePaymentCurrenciesCompletedEventHandler GetActivePaymentCurrenciesCompleted;
        
        /// <remarks/>
        public event GetAvailableCurrenciesForPaymentTypeCompletedEventHandler GetAvailableCurrenciesForPaymentTypeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#GetActivePaymentTypes", RequestNamespace="urn:WSCgdPaymentTypesService", ResponseNamespace="urn:WSCgdPaymentTypesService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public int GetActivePaymentTypes(string bstrBasketType, out string pbstrOutput) {
            object[] results = this.Invoke("GetActivePaymentTypes", new object[] {
                        bstrBasketType});
            pbstrOutput = ((string)(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetActivePaymentTypesAsync(string bstrBasketType) {
            this.GetActivePaymentTypesAsync(bstrBasketType, null);
        }
        
        /// <remarks/>
        public void GetActivePaymentTypesAsync(string bstrBasketType, object userState) {
            if ((this.GetActivePaymentTypesOperationCompleted == null)) {
                this.GetActivePaymentTypesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetActivePaymentTypesOperationCompleted);
            }
            this.InvokeAsync("GetActivePaymentTypes", new object[] {
                        bstrBasketType}, this.GetActivePaymentTypesOperationCompleted, userState);
        }
        
        private void OnGetActivePaymentTypesOperationCompleted(object arg) {
            if ((this.GetActivePaymentTypesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetActivePaymentTypesCompleted(this, new GetActivePaymentTypesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#GetActivePaymentTypesForCurrency", RequestNamespace="urn:WSCgdPaymentTypesService", ResponseNamespace="urn:WSCgdPaymentTypesService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public int GetActivePaymentTypesForCurrency(string bstrBasketType, string bstrCurrency, out string pbstrOutput) {
            object[] results = this.Invoke("GetActivePaymentTypesForCurrency", new object[] {
                        bstrBasketType,
                        bstrCurrency});
            pbstrOutput = ((string)(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetActivePaymentTypesForCurrencyAsync(string bstrBasketType, string bstrCurrency) {
            this.GetActivePaymentTypesForCurrencyAsync(bstrBasketType, bstrCurrency, null);
        }
        
        /// <remarks/>
        public void GetActivePaymentTypesForCurrencyAsync(string bstrBasketType, string bstrCurrency, object userState) {
            if ((this.GetActivePaymentTypesForCurrencyOperationCompleted == null)) {
                this.GetActivePaymentTypesForCurrencyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetActivePaymentTypesForCurrencyOperationCompleted);
            }
            this.InvokeAsync("GetActivePaymentTypesForCurrency", new object[] {
                        bstrBasketType,
                        bstrCurrency}, this.GetActivePaymentTypesForCurrencyOperationCompleted, userState);
        }
        
        private void OnGetActivePaymentTypesForCurrencyOperationCompleted(object arg) {
            if ((this.GetActivePaymentTypesForCurrencyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetActivePaymentTypesForCurrencyCompleted(this, new GetActivePaymentTypesForCurrencyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#GetActivePaymentCurrencies", RequestNamespace="urn:WSCgdPaymentTypesService", ResponseNamespace="urn:WSCgdPaymentTypesService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public int GetActivePaymentCurrencies(string bstrBasketType, out string pbstrOutput) {
            object[] results = this.Invoke("GetActivePaymentCurrencies", new object[] {
                        bstrBasketType});
            pbstrOutput = ((string)(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetActivePaymentCurrenciesAsync(string bstrBasketType) {
            this.GetActivePaymentCurrenciesAsync(bstrBasketType, null);
        }
        
        /// <remarks/>
        public void GetActivePaymentCurrenciesAsync(string bstrBasketType, object userState) {
            if ((this.GetActivePaymentCurrenciesOperationCompleted == null)) {
                this.GetActivePaymentCurrenciesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetActivePaymentCurrenciesOperationCompleted);
            }
            this.InvokeAsync("GetActivePaymentCurrencies", new object[] {
                        bstrBasketType}, this.GetActivePaymentCurrenciesOperationCompleted, userState);
        }
        
        private void OnGetActivePaymentCurrenciesOperationCompleted(object arg) {
            if ((this.GetActivePaymentCurrenciesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetActivePaymentCurrenciesCompleted(this, new GetActivePaymentCurrenciesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("#GetAvailableCurrenciesForPaymentType", RequestNamespace="urn:WSCgdPaymentTypesService", ResponseNamespace="urn:WSCgdPaymentTypesService")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public int GetAvailableCurrenciesForPaymentType(string bstrBasketType, string bstrPaymentType, string bstrPaymentSubtype, out string pbstrOutput) {
            object[] results = this.Invoke("GetAvailableCurrenciesForPaymentType", new object[] {
                        bstrBasketType,
                        bstrPaymentType,
                        bstrPaymentSubtype});
            pbstrOutput = ((string)(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetAvailableCurrenciesForPaymentTypeAsync(string bstrBasketType, string bstrPaymentType, string bstrPaymentSubtype) {
            this.GetAvailableCurrenciesForPaymentTypeAsync(bstrBasketType, bstrPaymentType, bstrPaymentSubtype, null);
        }
        
        /// <remarks/>
        public void GetAvailableCurrenciesForPaymentTypeAsync(string bstrBasketType, string bstrPaymentType, string bstrPaymentSubtype, object userState) {
            if ((this.GetAvailableCurrenciesForPaymentTypeOperationCompleted == null)) {
                this.GetAvailableCurrenciesForPaymentTypeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAvailableCurrenciesForPaymentTypeOperationCompleted);
            }
            this.InvokeAsync("GetAvailableCurrenciesForPaymentType", new object[] {
                        bstrBasketType,
                        bstrPaymentType,
                        bstrPaymentSubtype}, this.GetAvailableCurrenciesForPaymentTypeOperationCompleted, userState);
        }
        
        private void OnGetAvailableCurrenciesForPaymentTypeOperationCompleted(object arg) {
            if ((this.GetAvailableCurrenciesForPaymentTypeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAvailableCurrenciesForPaymentTypeCompleted(this, new GetAvailableCurrenciesForPaymentTypeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void GetActivePaymentTypesCompletedEventHandler(object sender, GetActivePaymentTypesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetActivePaymentTypesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetActivePaymentTypesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
        public string pbstrOutput {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetActivePaymentTypesForCurrencyCompletedEventHandler(object sender, GetActivePaymentTypesForCurrencyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetActivePaymentTypesForCurrencyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetActivePaymentTypesForCurrencyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
        public string pbstrOutput {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetActivePaymentCurrenciesCompletedEventHandler(object sender, GetActivePaymentCurrenciesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetActivePaymentCurrenciesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetActivePaymentCurrenciesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
        public string pbstrOutput {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetAvailableCurrenciesForPaymentTypeCompletedEventHandler(object sender, GetAvailableCurrenciesForPaymentTypeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAvailableCurrenciesForPaymentTypeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAvailableCurrenciesForPaymentTypeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
        public string pbstrOutput {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591