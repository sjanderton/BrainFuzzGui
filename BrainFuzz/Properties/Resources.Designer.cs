﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BrainFuzz.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BrainFuzz.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to ,[.,].
        /// </summary>
        internal static string cat0 {
            get {
                return ResourceManager.GetString("cat0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,+[-.,+].
        /// </summary>
        internal static string cat1 {
            get {
                return ResourceManager.GetString("cat1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,[.[-],].
        /// </summary>
        internal static string catunchanged {
            get {
                return ResourceManager.GetString("catunchanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ++++++++[&gt;++++[&gt;++&gt;+++&gt;+++&gt;+&lt;&lt;&lt;&lt;-]&gt;+&gt;+&gt;-&gt;&gt;+[&lt;]&lt;-]&gt;&gt;.&gt;---.+++++++..+++.&gt;&gt;.&lt;-.&lt;.+++.------.--------.&gt;&gt;+.&gt;++..
        /// </summary>
        internal static string helloWorld {
            get {
                return ResourceManager.GetString("helloWorld", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to //Using BrainFuzz extensions reads a number and prints it.\r\n// *NOTE* the &apos;;&apos; operation consumes 1 more than the number to allow multiple numbers\r\n//in the form &quot;1 2 3&quot;\r\n;[:&quot; &quot;;].
        /// </summary>
        internal static string numcat {
            get {
                return ResourceManager.GetString("numcat", resourceCulture);
            }
        }
    }
}
