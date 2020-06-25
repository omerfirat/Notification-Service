using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace NotificationService
{
    public class Logger : IDisposable
    {
        private static NLog.Logger _logger;
        private static bool _firstRun;
        public static void EnterLog(LogType logType, string message, Exception exception = null, string loggerName = "Default",
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int line = -1,
            [CallerFilePath] string path = "")
        {
            try
            {
                if (!_firstRun)
                {
                    string logConfigPath = ConfigReader.LogConfigPath;
                    string logPath = ConfigReader.LogPath;
                    string logAppName = ConfigReader.LogAppName;
                    if (!string.IsNullOrEmpty(logAppName)) logAppName = logAppName + "/";

                    if (!File.Exists(logConfigPath))
                        return;

                    NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(logConfigPath);

                    // file config
                    Target targetFile = LogManager.Configuration.FindTargetByName("file");
                    if (targetFile == null)
                        return;

                    WrapperTargetBase wrapperTargetFile = targetFile as WrapperTargetBase;
                    FileTarget fileTarget;
                    if (targetFile ==null)
                    {
                        fileTarget = targetFile as FileTarget;
                    }
                    else
                    {
                        fileTarget = wrapperTargetFile.WrappedTarget as FileTarget;
                    }

                    if (fileTarget == null)
                        return;

                    fileTarget.FileName = logPath.Replace("\\", "/").TrimEnd('/') + "/" + logAppName +"${shortdate}/" + "${logger}_${level}.log";
                    fileTarget.ArchiveFileName = logPath.Replace("\\", "/").TrimEnd('/') + "/" + "${shortdate}/" + logAppName + "${logger}_archive{###}.${level}.log";
                    LogManager.ReconfigExistingLoggers();

                    _firstRun = true;
                }

                _logger = NLog.LogManager.GetLogger(loggerName);
                message = message + "MethodName:" + methodName + " Line:"+line+ " Path:"+path;
                switch (logType)
                {
                    case LogType.Trace:
                        _logger.Log(NLog.LogLevel.Trace, exception, message, new object[] { methodName, line, path });
                        break;
                    case LogType.Debug:
                        _logger.Log(NLog.LogLevel.Debug, exception, message, new object[] { methodName, line, path });
                        break;
                    case LogType.Info:
                        _logger.Log(NLog.LogLevel.Info, exception, message, new object[] { methodName, line, path });
                        break;
                    case LogType.Warn:
                        _logger.Log(NLog.LogLevel.Warn, exception, message, new object[] { methodName, line, path });
                        break;
                    case LogType.Error:
                        _logger.Log(NLog.LogLevel.Error, exception, message, new object[] { methodName, line, path });
                        break;
                    case LogType.Fatal:
                        _logger.Log(NLog.LogLevel.Fatal, exception, message, new object[] { methodName, line, path });
                        break;
                }
            }
            catch (Exception ex)
            {
                string aa = ex.Message;
                // ignored
            }
        }

        public static void Flush()
        {
            NLog.LogManager.Flush();
        }

        public void Dispose()
        {
            Flush();
        }
    }

    public enum LogType
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
    }
}


