﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dengueSim.App {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class App : global::System.Configuration.ApplicationSettingsBase {
        
        private static App defaultInstance = ((App)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new App())));
        
        public static App Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int TamanhoMundo {
            get {
                return ((int)(this["TamanhoMundo"]));
            }
            set {
                this["TamanhoMundo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int QtdMosquitos {
            get {
                return ((int)(this["QtdMosquitos"]));
            }
            set {
                this["QtdMosquitos"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int QtdPessoas {
            get {
                return ((int)(this["QtdPessoas"]));
            }
            set {
                this["QtdPessoas"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int QtdAgentesSaude {
            get {
                return ((int)(this["QtdAgentesSaude"]));
            }
            set {
                this["QtdAgentesSaude"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int QtdFocos {
            get {
                return ((int)(this["QtdFocos"]));
            }
            set {
                this["QtdFocos"] = value;
            }
        }
    }
}
