#define DEBUG_LEVEL_LOG
//#define DEBUG_LEVEL_WARN
//#define DEBUG_LEVEL_ERROR
#define DEBUG_TOFILE
//#define DEBUG_TOCONSOLE
#define DEBUG_TOHTML

using UnityEngine;
using System.Collections;
using System.IO;

namespace Orange
{
    class D
    {
        private static StreamWriter m_writer;
        private static StreamWriter m_html;

        private static DLogger m_logger;

        private static string m_logPath;
        private static string m_logName;

        static D()
        {

            GameObject _logger = GameObject.Find("DLogger");

            if (_logger == null)
            {
                _logger = new GameObject();
                _logger.name = "DLogger";
                _logger.AddComponent("DLogger");
            }

            m_logger = _logger.GetComponent<DLogger>();

            m_logPath = Application.dataPath + "\\";
            m_logName = "dlogger";

            if (m_logger.LoggerPath != "")
            {
                m_logPath = m_logger.LoggerPath;
            }

            if (m_logger.LoggerName != "")
            {
                m_logName = m_logger.LoggerName;
            }

            CreateLogFile();
            CreateHtmlLogFile();

            Application.RegisterLogCallback(logCallback);

            LogTemplates();
        }

        public static void logCallback(string log, string stackTrace, LogType type)
        {
            // error gets a stack trace
            if (type == LogType.Error)
            {
                System.Console.WriteLine(log);
                System.Console.WriteLine(stackTrace);
            }
            else
            {
                System.Console.WriteLine(log);
            }
        }

        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
        public static void log(object format, params object[] paramList)
        {
            if (format is string)
            {
                LogToConsole(string.Format(format as string, paramList));
                LogToFile(string.Format(format as string, paramList));
                LogToHtml(Type.LOG, string.Format(format as string, paramList));
            }
            else
            {
                LogToConsole(format);
                LogToFile(format);
                LogToHtml(Type.LOG, format);
            }
        }

        public enum Type
        {
            AI,
            ASSERT,
            AUDIO,
            CONTENT,
            EXCEPTION,
            GAME_LOGIC,
            GRAPHICS,
            GUI,
            GUI_MESSAGE,
            INPUT,
            NETWORKING,
            NETWORK_CLIENT,
            NETWORK_SERVER,
            PHYSICS,
            REPLAY,
            SYSTEM,
            TERRAIN,
            WARNING,
            ERROR,
            LOG
        }

        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
        public static void log(Type type, object format, params object[] paramList)
        {
            if (format is string)
            {
                LogToConsole(string.Format(format as string, paramList));
                LogToFile(string.Format(format as string, paramList));
                LogToHtml(type, string.Format(format as string, paramList));
            }
            else
            {
                LogToConsole(format);
                LogToFile(format);
                LogToHtml(type, format);
            }
        }


        [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
        public static void warn(object format, params object[] paramList)
        {
            if (format is string)
            {
                LogToConsole(string.Format(format as string, paramList));
                LogToFile(string.Format(format as string, paramList));
                LogToHtml(Type.WARNING, string.Format(format as string, paramList));
            }
            else
            {
                LogToConsole(format);
                LogToFile(format);
                LogToHtml(Type.WARNING, format);
            }
        }


        [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
        public static void error(object format, params object[] paramList)
        {
            if (format is string)
            {
                LogToConsole(string.Format(format as string, paramList));
                LogToFile(string.Format(format as string, paramList));
                LogToHtml(Type.ERROR, string.Format(format as string, paramList));
            }
            else
            {
                LogToConsole(format);
                LogToFile(format);
                LogToHtml(Type.ERROR, format);
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void assert(bool condition)
        {
            assert(condition, string.Empty, true);
        }


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void assert(bool condition, string assertString)
        {
            assert(condition, assertString, false);
        }


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void assert(bool condition, string assertString, bool pauseOnFail)
        {
            if (!condition)
            {
                Debug.LogError("assert failed! " + assertString);

                if (pauseOnFail)
                    Debug.Break();
            }
        }

        [System.Diagnostics.Conditional("DEBUG_TOFILE")]
        private static void CreateLogFile()
        {
            //Debug.Log("CreateLogFile");
            m_writer = new StreamWriter(m_logPath + m_logName + ".log", false);
            m_writer.AutoFlush = true;
            LogToFile("Logger Active...");
        }

        [System.Diagnostics.Conditional("DEBUG_TOFILE")]
        private static void CloseLogFile()
        {
            //Debug.Log("CloseLogFile");
            m_writer.Close();
        }

        [System.Diagnostics.Conditional("DEBUG_TOHTML")]
        private static void CreateHtmlLogFile()
        {
            //Debug.Log("CreateHtmlLogFile");
            m_html = new StreamWriter(m_logPath + m_logName + ".html", false);
            m_html.AutoFlush = true;
            m_html.WriteLine("<head>");
            m_html.WriteLine("<script language=\"javascript\" src=\"dlstyle\\dlogger.javascript\"></script>");
            m_html.WriteLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"dlstyle\\dlogger.css\" />");
            m_html.WriteLine("</head>");
        }

        [System.Diagnostics.Conditional("DEBUG_TOHTML")]
        private static void CloseHtmlLogFile()
        {
            //Debug.Log("CloseHtmlLogFile");
            m_html.Close();
        }

        [System.Diagnostics.Conditional("DEBUG_TOCONSOLE")]
        private static void LogToConsole(object log)
        {
            Debug.Log(log);
        }

        [System.Diagnostics.Conditional("DEBUG_TOFILE")]
        private static void LogToFile(object log)
        {
            string _val = log as string;
            string _log = string.Format("{0} {1} {2}", System.DateTime.Now.ToFileTime(), "LOG", _val);
            m_writer.WriteLine(_log);
        }

        [System.Diagnostics.Conditional("DEBUG_TOHTML")]
        private static void LogToHtml(Type type, object log)
        {
            string name = TypeToString(type);
            m_html.Write("<p class=\"" + name + "\">");
            m_html.Write("<span class=\"Icon\"><img src=\"dlstyle\\{0}.png\" title=\"" + name + "\"></span><span class=\"Time\">{1}</span>", name.ToLower(), System.DateTime.Now.ToString("MM/dd/yyy hh:mm:ss.fff"));
            string _val = log as string;
            string _log = string.Format(_val);
            m_html.Write(_log);
            m_html.Write("</p>");
        }

        private static string TypeToString(Type type)
        {
            switch (type)
            {
                case Type.AI: return "AI";
                case Type.ASSERT: return "Assert";
                case Type.AUDIO: return "Audio";
                case Type.CONTENT: return "Content";
                case Type.EXCEPTION: return "Exception";
                case Type.GAME_LOGIC: return "GameLogic";
                case Type.GRAPHICS: return "Graphics";
                case Type.GUI: return "GUI";
                case Type.GUI_MESSAGE: return "GUIMessage";
                case Type.INPUT: return "Input";
                case Type.NETWORKING: return "Networking";
                case Type.NETWORK_CLIENT: return "NetworkClient";
                case Type.NETWORK_SERVER: return "NetworkServer";
                case Type.PHYSICS: return "Physics";
                case Type.REPLAY: return "Replay";
                case Type.SYSTEM: return "System";
                case Type.TERRAIN: return "Terrain";
                case Type.WARNING: return "Warning";
                case Type.ERROR: return "Error";
                case Type.LOG: return "Log";
                default: return "Log";
            }
        }

        public static void Quit()
        {
            //Debug.Log("DLogger is Shutting Down...");
            CloseHtmlLogFile();
            CloseLogFile();
        }

        public static void LogTemplates()
        {
            D.log("LOG Test");
            D.warn("WARN Test");
            D.error("ERROR Test");

            D.log(Type.AI, "AI Log");
            D.log(Type.ASSERT, "Asser Log");
            D.log(Type.AUDIO, "Audio Log");
            D.log(Type.CONTENT, "Content Log");
            D.log(Type.EXCEPTION, "Exception Log");
            D.log(Type.GAME_LOGIC, "GameLogic Log");
            D.log(Type.GRAPHICS, "Graphics Log");
            D.log(Type.GUI, "GUI Log");
            D.log(Type.GUI_MESSAGE, "GUIMEssage Log");
            D.log(Type.INPUT, "Input Log");
            D.log(Type.NETWORKING, "Networking Log");
            D.log(Type.NETWORK_CLIENT, "NetworkClient Log");
            D.log(Type.NETWORK_SERVER, "NetworkServer Log");
            D.log(Type.PHYSICS, "Physics Log");
            D.log(Type.REPLAY, "Replay Log");
            D.log(Type.SYSTEM, "System Log");
            D.log(Type.TERRAIN, "Terrain Log");

            D.log(Type.SYSTEM, "Dump Stack<br><br>{0}", System.Environment.StackTrace);
        }
    }
}
