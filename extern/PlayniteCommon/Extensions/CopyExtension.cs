using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayniteCommon.Extensions
{
    public static class CopyExtension
    {
        public static T Copy<T>(this T original) where T : class
        {
            return Playnite.SDK.Data.Serialization.FromJson<T>(Playnite.SDK.Data.Serialization.ToJson(original));
        }
    }
}
