using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Reflection;
using System.Text;

namespace OctopusLibrary
{
    public class Logger
    {
        private static NLog.Logger logger;

        /// <summary>
        /// 로그의 기본 형태를 지정합니다.
        /// </summary>
        static Logger()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            consoleTarget.Layout = @"${date:format=HH\\:MM\\:ss} ${logger} ${message}";

            LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            FileTarget fileTarget = new FileTarget();
            fileTarget.Encoding = Encoding.UTF8;
            fileTarget.FileName = String.Format("{0}/LogData/{1}/{2}/nLog_{3}.txt", "${basedir}", "${level}", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("yyyyMMdd"));
            fileTarget.Layout = String.Format("{0}[{1}] : {2}", "${logger}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "${message}");
            config.AddTarget("file", fileTarget);

            LoggingRule rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
            logger = LogManager.GetLogger("OctopusLibrary");
        }

        public Logger()
        {
        }

        public static void ErrorFormat(string msg, params object[] parameters)
        {
            Error(msg, parameters);
        }

        public static void InfoFormat(string msg, params object[] parameters)
        {
            Info(msg, parameters);
        }

        public static void WarnFormat(string msg, params object[] parameters)
        {
            Warn(msg, parameters);
        }

        public static void DebugFormat(string msg, params object[] parameters)
        {
            Debug(msg, parameters);
        }

        public void FatalFormat(string msg, params object[] parameters)
        {
            Fatal(msg, parameters);
        }

        /// <summary>
        /// 에러 발생
        /// </summary>
        /// <param name="msg">저장할 메시지</param>
        public static void Error(string msg)
        {
            logger.Error(msg);
        }

        /// <summary>
        /// 에러 발생
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void Error(Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                {
                    logger.Error(ex.InnerException.InnerException);
                }
                else
                {
                    logger.Error(ex.InnerException);
                }
            }
            else
            {
                logger.Error(ex);
            }

        }

        /// <summary>
        /// 정보 수집
        /// </summary>
        /// <param name="msg">수집된 데이터</param>
        public static void Info(string msg)
        {
            logger.Info(msg);
        }

        /// <summary>
        /// 경고 발생
        /// </summary>
        /// <param name="msg">경고 메시지</param>
        public static void Warn(string msg)
        {
            logger.Warn(msg);
        }

        /// <summary>
        /// 오류 발생
        /// </summary>
        /// <param name="msg">메시지</param>
        public static void Fatal(string msg)
        {
            logger.Fatal(msg);
        }

        /// <summary>
        /// 디버깅 메시지
        /// </summary>
        /// <param name="msg">메시지</param>
        public static void Debug(string msg)
        {
            logger.Debug(msg);
        }

        /// <summary>
        /// 에러 발생
        /// </summary>
        /// <param name="msg">메시지</param>
        /// <param name="parameters">String.Format Parameter</param>
        public static void Error(string msg, params object[] parameters)
        {
            logger.Error(String.Format(msg, parameters));
        }

        /// <summary>
        /// 정보 수집
        /// </summary>
        /// <param name="msg">메시지</param>
        /// <param name="parameters">String.Format Parameter</param>
        public static void Info(string msg, params object[] parameters)
        {
            logger.Info(String.Format(msg, parameters));
        }

        /// <summary>
        /// 경고 발생
        /// </summary>
        /// <param name="msg">메시지</param>
        /// <param name="parameters">String.Format Parameter</param>
        public static void Warn(string msg, params object[] parameters)
        {
            logger.Warn(String.Format(msg, parameters));
        }

        /// <summary>
        /// Database 오류 발생
        /// </summary>
        /// <param name="msg">메시지</param>
        /// <param name="parameters">String.Format Parameter</param>
        public static void Fatal(string msg, params object[] parameters)
        {
            logger.Fatal(String.Format(msg, parameters));
        }

        /// <summary>
        /// 디버깅 메시지
        /// </summary>
        /// <param name="msg">메시지</param>
        /// <param name="parameters">String.Format Parameter</param>
        public static void Debug(string msg, params object[] parameters)
        {
            logger.Debug(String.Format(msg, parameters));
        }

        public static void ObjectError<T>(T obj) where T : new()
        {
            if (obj != null)
            {
                Type resultClass = typeof(T);
                PropertyInfo[] resultInfo = resultClass.GetProperties();
                FieldInfo resultRow = null;
                string temp = String.Empty;
                logger.Error("===[{0}]===", Convert.ToString(obj));
                foreach(PropertyInfo info in resultInfo)
                {
                    resultRow = resultClass.GetField(resultClass.Name, BindingFlags.Instance);
                    temp = Convert.ToString(resultRow.GetValue(obj));
                    logger.Error("{0} : {1}", resultClass.Name, temp);
                }
            }
        }

        public static void ObjectDebug<T>(T obj) where T : new()
        {
            if (obj != null)
            {
                Type resultClass = typeof(T);
                PropertyInfo[] resultInfo = resultClass.GetProperties();
                FieldInfo resultRow = null;
                string temp = String.Empty;
                logger.Debug("===[{0}]===", Convert.ToString(obj));
                foreach (PropertyInfo info in resultInfo)
                {
                    resultRow = resultClass.GetField(resultClass.Name, BindingFlags.Instance);
                    temp = Convert.ToString(resultRow.GetValue(obj));
                    logger.Debug("{0} : {1}", resultClass.Name, temp);
                }
            }
        }

        public static void ObjectFatal<T>(T obj) where T : new()
        {
            if (obj != null)
            {
                Type resultClass = typeof(T);
                PropertyInfo[] resultInfo = resultClass.GetProperties();
                FieldInfo resultRow = null;
                string temp = String.Empty;
                logger.Fatal("===[{0}]===", Convert.ToString(obj));
                foreach (PropertyInfo info in resultInfo)
                {
                    resultRow = resultClass.GetField(resultClass.Name, BindingFlags.Instance);
                    temp = Convert.ToString(resultRow.GetValue(obj));
                    logger.Fatal("{0} : {1}", resultClass.Name, temp);
                }
            }
        }

        public static void ObjectWarn<T>(T obj) where T : new()
        {
            if (obj != null)
            {
                Type resultClass = typeof(T);
                PropertyInfo[] resultInfo = resultClass.GetProperties();
                FieldInfo resultRow = null;
                string temp = String.Empty;
                logger.Warn("===[{0}]===", Convert.ToString(obj));
                foreach (PropertyInfo info in resultInfo)
                {
                    resultRow = resultClass.GetField(resultClass.Name, BindingFlags.Instance);
                    temp = Convert.ToString(resultRow.GetValue(obj));
                    logger.Warn("{0} : {1}", resultClass.Name, temp);
                }
            }
        }

        public static void ObjectInfo<T>(T obj) where T : new()
        {
            if (obj != null)
            {
                Type resultClass = typeof(T);
                PropertyInfo[] resultInfo = resultClass.GetProperties();
                FieldInfo resultRow = null;
                string temp = String.Empty;
                logger.Info("===[{0}]===", Convert.ToString(obj));
                foreach (PropertyInfo info in resultInfo)
                {
                    resultRow = resultClass.GetField(resultClass.Name, BindingFlags.Instance);
                    temp = Convert.ToString(resultRow.GetValue(obj));
                    logger.Info("{0} : {1}", resultClass.Name, temp);
                }
            }
        }
    }
}
