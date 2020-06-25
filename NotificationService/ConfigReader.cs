using System;
using System.Configuration;

namespace NotificationService
{
    public static class ConfigReader
    {
        public static readonly string ServisName;
        public static readonly string LogAppName;
        public static readonly string LogPath;
        public static readonly string LogConfigPath;
        public static readonly string ConnectionString;
        public static readonly int Timer;
        public static readonly int ThreadCount;

        static ConfigReader()
        {
            ThreadCount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ThreadCount"));
            Timer = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Timer"));
            LogAppName = ConfigurationManager.AppSettings.Get("LogAppName");
            LogPath = ConfigurationManager.AppSettings.Get("LogPath");
            LogConfigPath = ConfigurationManager.AppSettings.Get("LogConfigPath");
            ServisName = ConfigurationManager.AppSettings.Get("ServiceName");
        }      
    }
}
