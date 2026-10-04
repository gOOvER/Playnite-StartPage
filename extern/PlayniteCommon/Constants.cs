using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayniteCommon
{
    public static class Constants
    {
        public struct ExtensionId
        {
            public ExtensionId(Guid pluginId, string addonId)
            {
                PluginId = pluginId;
                AddonId = addonId;
            }
            public readonly Guid PluginId;
            public readonly string AddonId;
        }

        public const string AddonIdPrefix = "felixkmh_";

        public static readonly ExtensionId ThemeExtrasId    = new ExtensionId(Guid.Parse("d2039edd-78f5-47c5-b190-72afef560fbe"), $"{AddonIdPrefix}Extras_Plugin");
        public static readonly ExtensionId DuplicateHiderId = new ExtensionId(Guid.Parse("382f8003-8ed0-4e47-ae93-05b43c9c6c32"), $"{AddonIdPrefix}DuplicateHider_Plugin");
        public static readonly ExtensionId QuickSearchId    = new ExtensionId(Guid.Parse("6a604592-7001-4b4e-a3be-91073b459e2b"), $"{AddonIdPrefix}QuickSearch_Plugin");
        public static readonly ExtensionId ShortcutSyncId   = new ExtensionId(Guid.Parse("8e48a544-3c67-41f8-9aa0-465627380ec8"), $"{AddonIdPrefix}ShortcutSync_Plugin");
        public static readonly ExtensionId AutoUpdateId     = new ExtensionId(Guid.Parse("e998a914-7644-4d1b-b6ff-57e3d8c39a6b"), $"{AddonIdPrefix}AutoUpdate_Plugin");
    }
}
