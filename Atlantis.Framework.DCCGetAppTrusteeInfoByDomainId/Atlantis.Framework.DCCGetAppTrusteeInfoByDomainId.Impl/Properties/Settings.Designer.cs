﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Impl.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://devdsweb:80/RegCheckDomainStatusWebSvc/RegCheckDomainStatusWebSvc.dll?Hand" +
            "ler=Default")]
        public string Atlantis_Framework_DCCGetAppTrusteeInfoByDomainId_Impl_AppTrusteeInfoWS_RegCheckDomainStatusWebSvcService {
            get {
                return ((string)(this["Atlantis_Framework_DCCGetAppTrusteeInfoByDomainId_Impl_AppTrusteeInfoWS_RegCheckD" +
                    "omainStatusWebSvcService"]));
            }
        }
    }
}
