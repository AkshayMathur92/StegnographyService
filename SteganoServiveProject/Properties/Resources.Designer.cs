﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SteganoServiveProject.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SteganoServiveProject.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;HTML&gt;
        ///&lt;BODY&gt;
        ///&lt;FORM name=&quot;steganoapidecode&quot; method=&quot;post&quot; enctype=&quot;multipart/form-data&quot;&gt;
        ///    &lt;INPUT type=&quot;text&quot; name=&quot;method&quot; value=&quot;decode&quot; hidden/&gt;
        ///    &lt;INPUT type=&quot;text&quot; name=&quot;msg&quot; value=&quot;&quot; hidden/&gt;
        ///    &lt;INPUT type = FILE name=&quot;image&quot; action = &quot;http://localhost:61189/api/Decode&quot;  REQUIRED/&gt; 
        ///    &lt;INPUT type = SUBMIT /&gt;
        ///&lt;/FORM&gt;
        ///&lt;/BODY&gt;
        ///&lt;/HTML&gt;.
        /// </summary>
        internal static string DecodeHTML {
            get {
                return ResourceManager.GetString("DecodeHTML", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;HTML&gt;
        ///&lt;BODY&gt;
        ///&lt;FORM name=&quot;steganoapiencode&quot; method=&quot;post&quot; enctype=&quot;multipart/form-data&quot;&gt;
        ///    &lt;INPUT type=&quot;text&quot; name=&quot;msg&quot; REQUIRED/&gt;
        ///    &lt;INPUT type = FILE name=&quot;image&quot; action = &quot;http://localhost:61189/api/Encode&quot;  REQUIRED/&gt; 
        ///    &lt;INPUT type = SUBMIT /&gt;
        ///&lt;/FORM&gt;
        ///&lt;/BODY&gt;
        ///&lt;/HTML&gt;.
        /// </summary>
        internal static string EncodeHTML {
            get {
                return ResourceManager.GetString("EncodeHTML", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;HTML&gt;
        ///&lt;BODY&gt;
        ///&lt;FORM name=&quot;steganoapiencode&quot; method=&quot;post&quot; enctype=&quot;multipart/form-data&quot;&gt;
        ///    &lt;INPUT type=&quot;text&quot; name=&quot;method&quot; value=&quot;encode&quot; hidden/&gt;
        ///    &lt;INPUT type=&quot;text&quot; name=&quot;msg&quot; REQUIRED/&gt;
        ///    &lt;INPUT type = FILE name=&quot;image&quot; action = &quot;http://localhost:61189/api/CryptUtility&quot;  REQUIRED/&gt; 
        ///    &lt;INPUT type = SUBMIT /&gt;
        ///&lt;/FORM&gt;
        ///&lt;HR&gt;
        ///&lt;FORM name=&quot;steganoapidecode&quot; method=&quot;post&quot; enctype=&quot;multipart/form-data&quot;&gt;
        ///    &lt;INPUT type=&quot;text&quot; name=&quot;method&quot; value=&quot;decode&quot; hidden/&gt;
        ///    &lt;INPUT type=&quot;text&quot; name=&quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TestHTML {
            get {
                return ResourceManager.GetString("TestHTML", resourceCulture);
            }
        }
    }
}
