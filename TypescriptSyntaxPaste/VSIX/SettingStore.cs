using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Shell;

namespace TypescriptSyntaxPaste.VSIX
{
    public class SettingStore
    {
        public static SettingStore Instance = new SettingStore();

        private WritableSettingsStore userSettingsStore;

        private const string IsConvertToInterfaceConst = "IsConvertToInterface";
        private const string CollectionPath = "TypescriptSyntaxPaste";

        protected SettingStore()
        {
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
             userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!userSettingsStore.CollectionExists(CollectionPath))
            {
                userSettingsStore.CreateCollection(CollectionPath);
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
    }
}
