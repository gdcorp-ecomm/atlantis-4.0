﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Atlantis.Framework.Basket.Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Atlantis.Framework.Basket.Tests.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;RESPONSE&gt;&lt;ERRORS&gt;&lt;ERROR&gt;&lt;NUMBER&gt;-1073468912&lt;/NUMBER&gt;&lt;DESC&gt; The specified shopper is not valid.&lt;/DESC&gt;&lt;SOURCE&gt;WscgdBasketService::CWscgdBasketService::AddItem&lt;/SOURCE&gt;&lt;LEVEL&gt;ERROR&lt;/LEVEL&gt;&lt;/ERROR&gt;&lt;/ERRORS&gt;&lt;/RESPONSE&gt;.
        /// </summary>
        internal static string InvalidShopperResponse {
            get {
                return ResourceManager.GetString("InvalidShopperResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        ///&lt;!-- edited with XMLSpy v2009 sp1 (http://www.altova.com) by GoDaddy.com, Inc. (GoDaddy.com, Inc.) --&gt;
        ///&lt;!--W3C Schema generated by XMLSpy v2006 sp2 U (http://www.altova.com)--&gt;
        ///&lt;xs:schema xmlns:xs=&quot;http://www.w3.org/2001/XMLSchema&quot; elementFormDefault=&quot;qualified&quot;&gt;
        ///	&lt;xs:simpleType name=&quot;contactTypeType&quot;&gt;
        ///		&lt;xs:restriction base=&quot;xs:byte&quot;&gt;
        ///			&lt;xs:minInclusive value=&quot;0&quot;/&gt;
        ///			&lt;xs:maxInclusive value=&quot;7&quot;/&gt;
        ///		&lt;/xs:restriction&gt;
        ///	&lt;/xs:simpleType&gt;
        ///	&lt;xs:s [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ItemRequest {
            get {
                return ResourceManager.GetString("ItemRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;RESPONSE&gt;&lt;MESSAGE&gt;Success&lt;/MESSAGE&gt;&lt;/RESPONSE&gt;.
        /// </summary>
        internal static string SuccessResponse {
            get {
                return ResourceManager.GetString("SuccessResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;RESPONSE/&gt;.
        /// </summary>
        internal static string UnknownResponse {
            get {
                return ResourceManager.GetString("UnknownResponse", resourceCulture);
            }
        }
    }
}
