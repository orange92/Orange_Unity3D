#define DEBUG_LEVEL_LOG
#define DEBUG_LEVEL_WARN
#define DEBUG_LEVEL_ERROR
#define DEBUG_TOFILE
#define DEBUG_TOCONSOLE
#define DEBUG_TOHTML

using UnityEngine;
using System.Collections;
using System.IO;

namespace Orange
{
    /// <summary>
    /// Klasa generująca logi.
    /// </summary>
    class D
    {
        #region Public_Variables

        /// <summary>
        /// Typy wpisów logu
        /// </summary>
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

        #endregion Public_Variables

        #region Private_Variables

        /// <summary>
        /// Plik textowy z logami.
        /// </summary>
        private static StreamWriter m_writer;

        /// <summary>
        /// Plik HTML z logami.
        /// </summary>
        private static StreamWriter m_html;

        /// <summary>
        /// Funkcja dziedzicząca po MonoBehaviour odpowiedzialan za logi.
        /// </summary>
        private static DLogger m_logger;

        /// <summary>
        /// Katalog z plikami logów.
        /// </summary>
        private static string m_logPath;

        /// <summary>
        /// Nazwa pliku z logami.
        /// </summary>
        private static string m_logName;

        #endregion Private_Variables

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

            CreateLogFile();
            CreateHtmlLogFile();
        }

        #region Public_Functions

        /// <summary>
        /// Dodanie logu.
        /// </summary>
        /// <param name="format">Treść logu.</param>
        /// <param name="paramList">lista parametrów.</param>
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void log(object format, params object[] paramList)
        {
            log(Type.LOG, format, paramList);
        }

        /// <summary>
        /// Dodanie logu
        /// </summary>
        /// <param name="type">Typ wpisu.</param>
        /// <param name="format">Treść logu.</param>
        /// <param name="paramList">lista parametrów.</param>
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void log(Type type, object format, params object[] paramList)
        {
            if (format is string)
            {
                LogToConsole(Type.LOG, string.Format(format as string, paramList));
                LogToFile(string.Format(format as string, paramList));
                LogToHtml(type, string.Format(format as string, paramList));
            }
            else
            {
                LogToConsole(Type.LOG, format);
                LogToFile(format);
                LogToHtml(type, format);
            }
        }

        /// <summary>
        /// Dodanie logu typu warning.
        /// </summary>
        /// <param name="format">Treść logu.</param>
        /// <param name="paramList">lista parametrów.</param>
        [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
        public static void warn(object format, params object[] paramList)
        {
            if (format is string)
            {
                LogToConsole(Type.WARNING, string.Format(format as string, paramList));
                LogToFile(string.Format(format as string, paramList));
                LogToHtml(Type.WARNING, string.Format(format as string, paramList));
            }
            else
            {
                LogToConsole(Type.WARNING, format);
                LogToFile(format);
                LogToHtml(Type.WARNING, format);
            }
        }

        /// <summary>
        /// Dodanie logu typu error.
        /// </summary>
        /// <param name="format">Treść logu.</param>
        /// <param name="paramList">lista parametrów.</param>
        [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
        public static void error(object format, params object[] paramList)
        {
            if (format is string)
            {
                LogToConsole(Type.ERROR, string.Format(format as string, paramList));
                LogToFile(string.Format(format as string, paramList));
                LogToHtml(Type.ERROR, string.Format(format as string, paramList));
            }
            else
            {
                LogToConsole(Type.ERROR, format);
                LogToFile(format);
                LogToHtml(Type.ERROR, format);
            }
        }

        /// <summary>
        /// W trybie UNITY_EDITOR zatrzymuje grę i dodaje pusty wpis w logach unity.
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void assert()
        {
            assert(string.Empty, true);
        }

        /// <summary>
        /// Dodaje wpis w logach unity.
        /// </summary>
        /// <param name="assertString">Treść logu.</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void assert(string assertString)
        {
            assert(assertString, false);
        }

        /// <summary>
        /// Dodaje wpis w logach unity i umożliwia zapauzowanie gry w trybie UNITY_EDITOR.
        /// </summary>
        /// <param name="assertString">Treść logu.</param>
        /// <param name="pauseOnFail">Czy zapauzować scene?</param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
        public static void assert(string assertString, bool pauseOnFail)
        {
            Debug.LogError(assertString);

            if (pauseOnFail)
                Debug.Break();
        }

        /// <summary>
        /// Generuje wszystkie typy logów (tak dla przykładu).
        /// </summary>
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

        #endregion Public_Functions

        #region Private_Functions

        /// <summary>
        /// Tworzy plik tekstowy z logami.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG_TOFILE")]
        private static void CreateLogFile()
        {
            //Debug.Log("CreateLogFile");
            m_writer = new StreamWriter(m_logPath + m_logName + ".log", false);
            m_writer.AutoFlush = true;
            LogToFile("Logger Active...");
        }

        /// <summary>
        /// Zamyka plik tekstowy z logami.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG_TOFILE")]
        private static void CloseLogFile()
        {
            //Debug.Log("CloseLogFile");
            m_writer.Close();
        }

        /// <summary>
        /// Tworzy plik HTML z logami.
        /// </summary>
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

        /// <summary>
        /// Zamyka plik HTML z logami.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG_TOHTML")]
        private static void CloseHtmlLogFile()
        {
            //Debug.Log("CloseHtmlLogFile");
            m_html.Close();
        }

        /// <summary>
        /// Zapisuje logi w konsoli Unity.
        /// </summary>
        /// <param name="type">Typ logu.</param>
        /// <param name="log">Treść logu.</param>
        [System.Diagnostics.Conditional("DEBUG_TOCONSOLE")]
        private static void LogToConsole(Type type, object log)
        {
            switch (type)
            {
                case Type.LOG:
                    Debug.Log(log);
                    break;
                case Type.ERROR:
                    Debug.LogError(log);
                    break;
                case Type.WARNING:
                    Debug.LogWarning(log);
                    break;
                default:
                    Debug.Log(log);
                    break;
            }
        }

        /// <summary>
        /// Zapisuje log do pliku tekstowego.
        /// </summary>
        /// <param name="log">Treść logu.</param>
        [System.Diagnostics.Conditional("DEBUG_TOFILE")]
        private static void LogToFile(object log)
        {
            string _val = log as string;
            string _log = string.Format("{0} {1} {2}", System.DateTime.Now.ToFileTime(), "LOG", _val);
            m_writer.WriteLine(_log);
        }

        /// <summary>
        /// Zapisuje log do HTML.
        /// </summary>
        /// <param name="type">Typ logu.</param>
        /// <param name="log">Treść logu.</param>
        [System.Diagnostics.Conditional("DEBUG_TOHTML")]
        private static void LogToHtml(Type type, object log)
        {
            string name = TypeToString(type);
            m_html.Write("<p class=\"" + name + "\">");
            m_html.Write("<span class=\"Icon\"><img src=\"dlstyle\\{0}.png\" title=\"" + name + "\"></span><span class=\"Time\">{1}</span>", name.ToLower(), System.DateTime.Now.ToString("MM/dd/yyy hh:mm:ss.fff"));
            string _val = log as string;
            string _log = string.Format(_val);
            m_html.Write(_log);
//            m_html.Write("<br><br>" + System.Environment.StackTrace);
            m_html.Write("</p>");
        }

        /// <summary>
        /// Konwertuje typ logu na wartość string.
        /// </summary>
        /// <param name="type">Typ logu.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Koniec logowania zamknięcie pliku logów.
        /// </summary>
        public static void Quit()
        {
            //Debug.Log("DLogger is Shutting Down...");
            CloseHtmlLogFile();
            CloseLogFile();
        }

        #endregion Private_Functions
    }
}
