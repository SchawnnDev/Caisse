﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CaisseDesktop.Lang {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class French {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal French() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CaisseDesktop.Lang.French", typeof(French).Assembly);
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
        ///   Looks up a localized string similar to Erreur.
        /// </summary>
        internal static string CaisseException_Error {
            get {
                return ResourceManager.GetString("CaisseException_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Une erreur est survenue: {0}.
        /// </summary>
        internal static string CaisseException_ErrorOccured {
            get {
                return ResourceManager.GetString("CaisseException_ErrorOccured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to L&apos;événement a bien été crée..
        /// </summary>
        internal static string Event_Created {
            get {
                return ResourceManager.GetString("Event_Created", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to L&apos;événement a bien été enregistré..
        /// </summary>
        internal static string Event_Saved {
            get {
                return ResourceManager.GetString("Event_Saved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Veuillez remplir toutes les cases obligatoires..
        /// </summary>
        internal static string Exception_ArgsMissing {
            get {
                return ResourceManager.GetString("Exception_ArgsMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Impossible d&apos;enregistrer. Veuillez réessayer..
        /// </summary>
        internal static string Exception_CantSave {
            get {
                return ResourceManager.GetString("Exception_CantSave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to L&apos;image par défaut n&apos;a pas été trouvée..
        /// </summary>
        internal static string Exception_DefaultImageNotFound {
            get {
                return ResourceManager.GetString("Exception_DefaultImageNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to L&apos;heure de fin ne peut pas être avant ou égale à l&apos;heure de début..
        /// </summary>
        internal static string TimeSlotConfigModel_Save_Hour {
            get {
                return ResourceManager.GetString("TimeSlotConfigModel_Save_Hour", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to L&apos;heure de début ne peut pas être après l&apos;heure de fin..
        /// </summary>
        internal static string TimeSlotConfigModel_Save_Hour2 {
            get {
                return ResourceManager.GetString("TimeSlotConfigModel_Save_Hour2", resourceCulture);
            }
        }
    }
}