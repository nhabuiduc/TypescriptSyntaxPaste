using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Shell;
using System.Xml.Serialization;
using System.IO;

namespace TypescriptSyntaxPaste.VSIX
{
    public class SettingStore
    {
        public static SettingStore Instance = new SettingStore();

        private WritableSettingsStore userSettingsStore;

        private const string IsConvertToInterfaceConst = "IsConvertToInterface";
        private const string IsConvertMemberToCamelCaseConst = "IsConvertMemberToCamelCase";
        private const string CollectionPath = "TypescriptSyntaxPaste";
        private const string IsConvertListToArrayConst = "IsConvertListToArray";
        private const string ReplacedTypeNameArrayConst = "ReplacedTypeNameArray";
        private const string AddIPrefixInterfaceDeclarationConst = "AddIPrefixInterfaceDeclaration";

        protected SettingStore()
        {
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
             userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!userSettingsStore.CollectionExists(CollectionPath))
            {
                userSettingsStore.CreateCollection(CollectionPath);
            }
        }

        public bool IsConvertMemberToCamelCase
        {
            get
            {
                if (!userSettingsStore.PropertyExists(CollectionPath, IsConvertMemberToCamelCaseConst))
                {
                    return false;
                }

                return userSettingsStore.GetBoolean(CollectionPath, IsConvertMemberToCamelCaseConst);
            }
            set
            {

                userSettingsStore.SetBoolean(CollectionPath, IsConvertMemberToCamelCaseConst, value);
            }
        }

        public bool IsConvertToInterface
        {
            get
            {
                if (!userSettingsStore.PropertyExists(CollectionPath, IsConvertToInterfaceConst)){
                    return false;
                }

                return userSettingsStore.GetBoolean(CollectionPath,IsConvertToInterfaceConst);
            }
            set
            {
                
                userSettingsStore.SetBoolean(CollectionPath, IsConvertToInterfaceConst, value);
            }
        }

        public bool IsConvertListToArray
        {
            get
            {
                if (!userSettingsStore.PropertyExists(CollectionPath, IsConvertListToArrayConst))
                {
                    return false;
                }

                return userSettingsStore.GetBoolean(CollectionPath, IsConvertListToArrayConst);
            }
            set
            {

                userSettingsStore.SetBoolean(CollectionPath, IsConvertListToArrayConst, value);
            }
        }

        XmlSerializer serializer = new XmlSerializer(typeof(TypeNameReplacementData[]));
        private TypeNameReplacementData[] replacedTypeNameArray;

        public TypeNameReplacementData[] ReplacedTypeNameArray
        {
            get
            {
                if(replacedTypeNameArray != null)
                {
                    return replacedTypeNameArray;
                }

                if (!userSettingsStore.PropertyExists(CollectionPath, ReplacedTypeNameArrayConst))
                {
                    return new TypeNameReplacementData[] {
                        new TypeNameReplacementData
                        {
                            NewTypeName = "Date",
                            OldTypeName = "DateTimeOffset"
                        },
                        new TypeNameReplacementData
                        {
                            NewTypeName = "Date",
                            OldTypeName = "DateTime"
                        }
                    };
                }

                using (StringReader textReader = new StringReader(userSettingsStore.GetString(CollectionPath, ReplacedTypeNameArrayConst)))
                {
                    replacedTypeNameArray = (TypeNameReplacementData[])serializer.Deserialize(textReader);
                }

                return replacedTypeNameArray;
                 
            }
            set
            {
                using (StringWriter textWriter = new StringWriter())
                {
                    serializer.Serialize(textWriter, value);
                    userSettingsStore.SetString(CollectionPath, ReplacedTypeNameArrayConst, textWriter.ToString());
                    replacedTypeNameArray = value;
                }

                
            }
        }

        public bool AddIPrefixInterfaceDeclaration
        {
            get
            {
                if (!userSettingsStore.PropertyExists(CollectionPath, AddIPrefixInterfaceDeclarationConst))
                {
                    return false;
                }

                return userSettingsStore.GetBoolean(CollectionPath, AddIPrefixInterfaceDeclarationConst);
            }
            set
            {

                userSettingsStore.SetBoolean(CollectionPath, AddIPrefixInterfaceDeclarationConst, value);
            }
        }
    }
}
