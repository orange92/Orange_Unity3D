using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace Orange
{
    /// <summary>
    /// Statyczna klasa konfiguracyjna przechowująca podstawowe informacje o aplikacji i graczu.
    /// </summary>
    public static class GameConfig
    {
        #region PrivateConfigVariables

        /// <summary>
        /// Nazwa aplikacji.
        /// </summary>
        private static string m_gameName = "NAZWA_APLIKACJI";

        /// <summary>
        /// Wersja aplikacji.
        /// </summary>
        private static string m_version = "WERSJA_APLIKACJI";

        /// <summary>
        /// Lista autorów.
        /// </summary>
        private static string m_author = "AUTORZY";

        /// <summary>
        /// Strona www projektu.
        /// </summary>
        private static string m_site = "ADRES_STRONY_WWW_APLIKACJI";

        /// <summary>
        /// Port wykożystywany do komunikacji sieciowej.
        /// </summary>
        private static int m_gamePort = 25000;

        /// <summary>
        /// Maksymalna ilość graczy.
        /// </summary>
        private static int m_maxNumberOfPlayers = 4;

        /// <summary>
        /// Nazwa gracza / hosta.
        /// </summary>
        private static string _playerName = "NAZWA_GRACZA";

        #endregion PrivateConfigVariables

        #region PrivateVariable

        /// <summary>
        /// Uchwyt do elementu odpowiedzialnego z komunikację sieciową.
        /// </summary>
        public static SystemBehaviour.Communication communication = null;

        #endregion PrivateVariable

        #region PublicConfigVariables

        /// <summary>
        /// Zwraca nazwę gracza
        /// </summary>
        /// <returns>Zwraca aktualną nazwę gracza.</returns>
        public static string playerName { get { return _playerName; } set { _playerName = value; } }

        /// <summary>
        /// Zwraca nazwę aplikacji.
        /// </summary>
        public static string gameName { get { return m_gameName; } }

        /// <summary>
        /// Zwraca wersję aplikacji.
        /// </summary>
        public static string version { get { return m_version; } }

        /// <summary>
        /// Zwraca listę autorów.
        /// </summary>
        public static string author { get { return m_author; } }

        /// <summary>
        /// Zwraca adres strony projektu.
        /// </summary>
        public static string site { get { return site; } }

        /// <summary>
        /// Zwraca port gry.
        /// </summary>
        public static int gamePort { get { return m_gamePort; } set { m_gamePort = value; } }

        /// <summary>
        /// Maksymalna ilość graczy.
        /// </summary>
        public static int maxNumberOfPlayers { get { return m_maxNumberOfPlayers; } set { m_maxNumberOfPlayers = value; } }

        #endregion PublicConfigVariables

        #region PrivateFunction

        /// <summary>
        /// Ładuje ustawienia z pliku konfiguracyjnego.
        /// </summary>
        /// <returns></returns>
        private static bool LoadConfig()
        {
            try
            {
                string[] readText = File.ReadAllLines(Application.dataPath + "/Resources/config.ini", Encoding.UTF8);
                foreach (string line in readText)
                {
                    if (line[0] == '#') continue;
                    int i = line.IndexOf(":");
                    string command_ = line.Substring(0, i);
                    string value_ = line.Substring(i + 1, line.Length - i - 1);
                    SetConfig(command_, value_);
                }
            }
            catch
            {
                Debug.Log("Config file not found. Load default config.");
            }
            return true;
        }

        /// <summary>
        /// Ładuje zmienne konfiguracyjne.
        /// </summary>
        /// <param name="command_"></param>
        /// <param name="value_"></param>
        private static void SetConfig(string command_, string value_)
        {
            switch (command_)
            {
                case "name":
                    playerName = value_;
                    break;
                case "port":
                    int buf;
                    if (int.TryParse(value_, out buf)) gamePort = buf;
                    break;
            }
        }

        #endregion PrivateFunction

        #region PublicFunction

        /// <summary>
        /// Weryfikuje czy gra jest uruchomiona w edytorze czy w wersji produkcyjnej.
        /// </summary>
        /// <returns>Zwraca "E_" jeśli uruchomiono w edytorze, jeśli w wersji produkcyjnej "P_"</returns>
        private static string PrefsGroup()
        {
            return Application.isEditor ? "E_" : "P_";
        }

        /// <summary>
        /// Zwraca pełną nazwę gry, wraz z numerem wersji i listą autorów.
        /// </summary>
        /// <returns>Pełna nazwa gry.</returns>
        public static string GetFullGameName()
        {
            return gameName + " v. " + version + " [" + author + "]";
        }

        /// <summary>
        /// Zwraca domyślny typ gry.
        /// </summary>
        /// <returns>Domyślny typ gry.</returns>
        public static string GetDefaultGameType()
        {
            return GetFullGameName() + " / Default Game";
        }

        #endregion PublicFunction

        /// <summary>
        /// Wymusza załadowanie pliku konfiguracyjnego przy starcie projektu.
        /// </summary>
        private static bool Configloaded = LoadConfig();
    }
}