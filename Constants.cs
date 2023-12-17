namespace Galaxy_Life_Tool
{
    public class Constants
    {
        public static class Guilds {
            public const ulong GalacticSwamp = 1043524633602826294;
        }
        public static class Channels {
            public const ulong Raw = 1113778766494322688;
        }

        public static string GlProcessUrl
        {
            get
            {
                switch ((StoredData.ApplicationType)StoredData.data.ApplicationType){
                    case StoredData.ApplicationType.Steam: return "steam://rungameid/1927780";
                    case StoredData.ApplicationType.Flash: return @"C:\Program Files (x86)\FlashBrowser\FlashBrowser.exe";
                }
                return string.Empty;
            }
        }
        public static string ProcessName
        {
            get
            {
                switch ((StoredData.ApplicationType)StoredData.data.ApplicationType){
                    case StoredData.ApplicationType.Steam: return "Galaxy Life";
                    case StoredData.ApplicationType.Flash: return "Flash Browser";
                }
                return string.Empty;
            }
        }
        public static int ProcessId
        {
            get
            {
                return StoredData.data.ApplicationType;
            }
            set
            {
                StoredData.data.ApplicationType = value;
            }
        }
    }
}
