using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Orange.SystemBehaviour
{

    /// <summary>
    /// Odpowiada za komunikację sieciową. Wykożystano już istniejące mechanizmy znajdujące się w Unity3D.
    /// Do poprawnego działania wymaga komponentu Network View.
    /// </summary>
    public class Communication : MonoBehaviour
    {
        public void Awake() { if (GameConfig.communication == null) GameConfig.communication = this; else GameObject.Destroy(this); }

        #region UnityFunction

        void Start()
        {
        }

        void Update()
        {
            if (m_doInitHost)
            {
                if (m_doneConnectionTesting)
                {
                    Debug.Log(m_connectionTestMessage);
                    m_doInitHost = false;
                    _InitHost();
                }
            }
        }

        #endregion UnityFunction

        #region ALL

        public delegate void OnHostDisconnected_delegate();

        /// <summary>
        /// Zdarzenie rozłączenia połączenia sieciowego.
        /// </summary>
        public OnHostDisconnected_delegate onDisconnected_delegate = null;

        /// <summary>
        /// Rozłączono lub utrata połączenia.
        /// </summary>
        /// <param name="info"></param>
        protected void OnDisconnectedFromServer(NetworkDisconnection info)
        {
            // Jest serwerem i został rozłączony.
            if (Network.isServer)
            {
                Debug.Log("Local server connection disconnected");
                if (onDisconnected_delegate != null) onDisconnected_delegate();
                for (int i = 0; i < m_listOfPlayersConnectedToHost.Count; i++)
                {
                    Network.RemoveRPCs(m_listOfPlayersConnectedToHost[i]);
                }
                m_listOfPlayersConnectedToHost.Clear();
            }

            // Jest klientem i został rozłączony.
            else
            {
                if (info == NetworkDisconnection.LostConnection)
                    Debug.Log("Lost connection to the server");
                else
                    Debug.Log("Successfully disconnected from the server");
                if (onDisconnected_delegate != null) onDisconnected_delegate();
            }
        }

        /// <summary>
        /// Zwraca adres IP gracza. w postaci tekstu
        /// </summary>
        public string getInternalIP
        {
            get
            {
                return Network.player.ipAddress;
            }
        }

        /// <summary>
        /// Zwraca zewnętrzny adres IP (poza NAT'em) w postaci tekstu.
        /// </summary>
        public string getExternalIP
        {
            get
            {
                return Network.player.externalIP;
            }
        }

        #endregion

        #region TEST_CONNECTION

        #region VARIABLES

        /// <summary>
        /// Czy zakończono testowanie połączenia z serwerem Unity.
        /// </summary>
        protected bool m_doneConnectionTesting = false;

        /// <summary>
        /// Wynik testu połączenia z serwerem Unity.
        /// </summary>
        protected UnityEngine.ConnectionTesterStatus m_connectionTestResult = ConnectionTesterStatus.Undetermined;

        /// <summary>
        /// Informacja o wyniku testu połaczenia z serwerem Unity.
        /// </summary>
        protected string m_connectionTestMessage = "Test in progress";

        /// <summary>
        /// Informacja o stanie testu połączenia z serwerem Unity.
        /// </summary>
        protected string m_connectionTestStatus = "Testing network connection capabilities.";

        /// <summary>
        /// Informacja czy należy używać NAT przy połączeniu z serwerem Unity.
        /// </summary>
        protected bool m_useNat = false;

        /// <summary>
        /// Informacja (tekstowa) czy należe używać NAT przy połączeniu z serwerem Unity.
        /// </summary>
        protected string m_shouldEnableNatMessage = "";

        /// <summary>
        /// Czy używany jest publiczny adres IP.
        /// </summary>
        protected bool m_probingPublicIP = false;

        /// <summary>
        /// Timer Uzywany do testowania połączenia z serwerem Unity.
        /// </summary>
        protected float m_connectionTestTimer;

        /// <summary>
        /// Definiuje czy należy użyć MasterServer Unity.
        /// </summary>
        protected bool m_useMasterServer = false;

        #endregion VARIABLES

        /// <summary>
        /// Wykonuje test połączenia z serwerem.
        /// </summary>
        public void TestConnection()
        {
            m_useMasterServer = false;
            m_doneConnectionTesting = false;
            m_connectionTestResult = Network.TestConnection();
            switch (m_connectionTestResult)
            {
                case ConnectionTesterStatus.Error:
                    m_connectionTestMessage = "Problem determining NAT capabilities";
                    m_doneConnectionTesting = true;
                    break;

                case ConnectionTesterStatus.Undetermined:
                    m_connectionTestMessage = "Undetermined NAT capabilities";
                    m_useNat = false;
                    m_doneConnectionTesting = true;
                    m_useMasterServer = true;
                    break;

                case ConnectionTesterStatus.PublicIPIsConnectable:
                    m_connectionTestMessage = "Directly connectable public IP address.";
                    m_useNat = false;
                    m_doneConnectionTesting = true;
                    m_useMasterServer = true;
                    break;
                // This case is a bit special as we now need to check if we can 
                // circumvent the blocking by using NAT punchthrough
                case ConnectionTesterStatus.PublicIPPortBlocked:
                    m_connectionTestMessage = "Non-connectable public IP address (port " +
                        MasterServer.port + " blocked), running a server is impossible.";
                    m_useNat = false;
                    // If no NAT punchthrough test has been performed on this public 
                    // IP, force a test
                    if (!m_probingPublicIP)
                    {
                        m_connectionTestResult = Network.TestConnectionNAT();
                        m_probingPublicIP = true;
                        m_connectionTestStatus = "Testing if blocked public IP can be circumvented";
                        m_connectionTestTimer = Time.time + 10;
                        TestConnection();
                    }
                    // NAT punchthrough test was performed but we still get blocked
                    else if (Time.time > m_connectionTestTimer)
                    {
                        m_probingPublicIP = false; 		// reset
                        m_useNat = true;
                        m_doneConnectionTesting = true;
                    }
                    break;
                case ConnectionTesterStatus.PublicIPNoServerStarted:
                    m_connectionTestMessage = "Public IP address but server not initialized, " +
                        "it must be started to check server accessibility. Restart " +
                        "connection test when ready.";
                    m_doneConnectionTesting = true;
                    break;
                case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
                    m_connectionTestMessage = "Limited NAT punchthrough capabilities. Cannot " +
                        "connect to all types of NAT servers. Running a server " +
                        "is ill advised as not everyone can connect.";
                    m_useNat = true;
                    m_doneConnectionTesting = true;
                    break;

                case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
                    m_connectionTestMessage = "Limited NAT punchthrough capabilities. Cannot " +
                        "connect to all types of NAT servers. Running a server " +
                        "is ill advised as not everyone can connect.";
                    m_useNat = true;
                    m_doneConnectionTesting = true;
                    break;

                case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
                    m_doneConnectionTesting = true;
                    break;
                case ConnectionTesterStatus.NATpunchthroughFullCone:
                    m_connectionTestMessage = "NAT punchthrough capable. Can connect to all " +
                        "servers and receive connections from all clients. Enabling " +
                        "NAT punchthrough functionality.";
                    m_useNat = true;
                    m_useMasterServer = true;
                    m_doneConnectionTesting = true;
                    break;

                default:
                    m_connectionTestMessage = "Error in test routine, got " + m_connectionTestResult;
                    m_doneConnectionTesting = true;
                    break;
            }
            if (m_doneConnectionTesting)
            {
                if (m_useNat)
                    m_shouldEnableNatMessage = "When starting a server the NAT " +
                        "punchthrough feature should be enabled (useNat parameter)";
                else
                    m_shouldEnableNatMessage = "NAT punchthrough not needed";
                m_connectionTestStatus = "Done testing";
            }
        }
        #endregion

        #region CLIENT

        #region PRIVATE_VARIABLES

        /// <summary>
        /// Informacja czy klient podłączył się do hosta.
        /// </summary>
        protected bool m_connectedToHost = false;

        /// <summary>
        /// ID hosta który został wybrany lub -1.
        /// </summary>
        protected int m_hostListSelected = -1;

        /// <summary>
        /// Lista odnalezionych hostów.
        /// </summary>
        protected List<HostData> m_discoveredHostsList;

        /// <summary>
        /// Informacja o błędzie podłączenia.
        /// </summary>
        protected bool m_connectionFailed;

        /// <summary>
        /// Status połączenia.
        /// </summary>
        protected string m_connectionStatus;

        /// <summary>
        /// Czas próby połączenia.
        /// </summary>
        protected float m_connectionTime;

        /// <summary>
        /// Adre IP hosta do którego należy się podłączyć gdy nie wybrano ID hosta z listy.
        /// </summary>
        protected string m_specifiedHostIP;

        #endregion PRIVATE_VARIABLES

        #region PUBLIC_VARIABLES

        /// <summary>
        /// Informacja czy klient podłączył się do hosta.
        /// </summary>
        public bool connectedToHost { get { return m_connectedToHost; } }

        /// <summary>
        /// Delegat dla zdarzenia podłączenia klienta do serwera wykonywane po stronie klienta.
        /// </summary>
        public OnConnectedToHost_delegate onConnectedToHost_delegate = null;

        #endregion PUBLIC_VARIABLES

        #region DELEGATE

        /// <summary>
        /// Delegat dla zdarzenia podłączenia klienta do serwera wykonywane po stronie klienta.
        /// </summary>
        public delegate void OnConnectedToHost_delegate();

        #endregion DELEGATE

        #region PUBLIC_FUNCTIONS

        /// <summary>
        /// Wybranie ID hosta z listy hostData i połączenie się z nim.
        /// </summary>
        /// <param name="id"></param>
        public void ConnectToHost(int id)
        {
            if (id >= 0 && id < m_discoveredHostsList.Count)
            {
                m_hostListSelected = id;
                _ConnectToHost();
            }
            else m_hostListSelected = -1;
        }

        /// <summary>
        /// Połączenie się z ręcznie zdefiniowanym hostem.
        /// </summary>
        /// <param name="ipAddress">Adres ip hosta</param>
        /// <returns>Zwraca informację czy adres jest poprawny i czy podjęto prubę połączenia.</returns>
        public bool ConnectToHost(string ipAddress)
        {
            bool match = Regex.Match(ipAddress, @"^\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b$").Success;
            if (match)
            {
                m_specifiedHostIP = ipAddress;
                m_hostListSelected = -1;
                _ConnectToHost();
                return true;
            }
            else
            {
                Debug.Log("Wrong server IP address.");
                return false;
            }
        }

        /// <summary>
        /// Odświerza i zwraca listę Hostów znalezionych w sieci.
        /// </summary>
        public List<string> RefreashHostList()
        {
            try
            {
                MasterServer.RequestHostList(GameConfig.GetDefaultGameType());

                HostData[] allHosts = MasterServer.PollHostList();
                m_discoveredHostsList = new List<HostData>();
                List<string> hostList = new List<string>();

                foreach (HostData host in allHosts)
                {
                    if (host.connectedPlayers >= host.playerLimit)
                        continue;

                    m_discoveredHostsList.Add(host);
                    hostList.Add("[" + (host.connectedPlayers - 1) + "/" + (host.playerLimit - 1) + "] " + host.comment);
                }

                return hostList;
            }
            catch
            {
                return new List<string>();
            }
        }

        #endregion PUBLIC_FUNCTIONS

        #region PRIVATE_FUNCTIONS

        /// <summary>
        /// Resetuje klienta.
        /// </summary>
        protected void ResetClient()
        {
            m_discoveredHostsList = null;
            m_hostListSelected = -1;
            m_specifiedHostIP = "";
            MasterServer.ClearHostList();
        }

        /// <summary>
        /// Podłącz do hosta.
        /// </summary>
        protected void _ConnectToHost()
        {
            m_connectedToHost = false;
            m_connectionFailed = false;
            m_connectionStatus = "Connecting: " + (m_hostListSelected != -1 ?
                m_discoveredHostsList[m_hostListSelected].ip[0] :
                m_specifiedHostIP);
            Debug.Log(m_connectionStatus);
            m_connectionTime = Time.time;

            NetworkConnectionError error = m_hostListSelected != -1 ?
                Network.Connect(m_discoveredHostsList[m_hostListSelected]) :
                Network.Connect(m_specifiedHostIP, GameConfig.gamePort);

            OnFailedToConnect(error);
        }

        /// <summary>
        /// Nawiązano połączenie z serwerem.
        /// </summary>
        protected void OnConnectedToServer()
        {
            m_connectedToHost = true;
            Debug.Log("Connected to server");
            m_connectionStatus = "Awaiting server response";
            onConnectedToHost_delegate();
        }

        public delegate void OnFailedToConnectToHost_delegate();

        /// <summary>
        /// Zdarzenie błędu podczas łączenia z hostem - wywoływane po stronie klienta.
        /// </summary>
        public OnFailedToConnectToHost_delegate onFailedToConnectToHost_delegate = null;

        /// <summary>
        /// Błąd połączenia do hosta.
        /// </summary>
        /// <param name="networkError"></param>
        protected void OnFailedToConnect(NetworkConnectionError networkError)
        {
            if (networkError != NetworkConnectionError.NoError)
            {
                m_connectedToHost = false;
                m_hostInitialized = false;
                Debug.Log("Unable to connect to server");
                m_connectionStatus = "Error: " + networkError;
                m_connectionFailed = true;
                if (onFailedToConnectToHost_delegate != null) onFailedToConnectToHost_delegate();
            }
        }

        #endregion PRIVATE_FUNCTIONS

        #endregion CLIENT

        #region HOST

        #region PUBLIC_VARIABLE

        /// <summary>
        /// Definiuje czy pozwolić klientom dołączyć do serwera.
        /// </summary>
        public bool allowJoinToServer { get { return m_allowJoinToServer; } set { m_allowJoinToServer = value; } }

        /// <summary>
        /// Informacja czy host został zainicjowany.
        /// </summary>
        public bool hostInitialized { get { return m_hostInitialized; } }

        /// <summary>
        /// Zwraca listę graczy podpiętych do hosta.
        /// </summary>
        public List<NetworkPlayer> listOfPlayersConnectedToHost { get { return m_listOfPlayersConnectedToHost; } }

        /// <summary>
        /// Lista wywołań delegatu po podłączeniu klienta do hosta - wykonywane po stronie hosta.
        /// </summary>
        public OnPlayerConnected_delegate onPlayerConnected_delegate = null;

        /// <summary>
        /// Zdarzenie rozłączenia klienta od serwera - wykonywane po stronie hosta.
        /// </summary>
        /// <param name="player"></param>
        public OnPlayerDisconnected_delegate onPlayerDisconnected_delegate = null;

        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLE

        /// <summary>
        /// Definiuje czy pozwolić klientom dołączyć do serwera.
        /// </summary>
        protected bool m_allowJoinToServer = true;

        /// <summary>
        /// Lista wszystkich klientów podłączonych do hosta.
        /// </summary>
        protected List<NetworkPlayer> m_listOfPlayersConnectedToHost = new List<NetworkPlayer>();

        /// <summary>
        /// Informacja czy host został zainicjowany.
        /// </summary>
        protected bool m_hostInitialized = false;

        /// <summary>
        /// Informacja czy wystąpił błąd przy inicjacji hosta.
        /// </summary>
        protected bool m_hostFailed;

        /// <summary>
        /// Status hosta.
        /// </summary>
        protected string m_hostStatus;

        /// <summary>
        /// Flaga z rozkazem inicjacji hosta.
        /// </summary>
        protected bool m_doInitHost = false;

        #endregion PRIVATE_VARIABLE

        #region PUBLIC_FUNCTIONS

        /// <summary>
        /// Liczba aktualnie podłączonych klientów
        /// </summary>
        public int getNumberOfConnections
        {
            get
            {
                return Network.connections.Length;
            }
        }

        /// <summary>
        /// Zwraca maksymalną dozwoloną liczbę klientów.
        /// </summary>
        /// <returns>Maksymalna liczb apołączeń.</returns>
        public int getNumberOfMaxConnections
        {
            get
            {
                return Network.maxConnections;
            }
        }

        /// <summary>
        /// Inicjuje hosta.
        /// </summary>
        public void InitHost()
        {
            if (hostInitialized)
            {
                _DestroyHost();
            }
            m_doneConnectionTesting = false;
            TestConnection();
            m_doInitHost = true;
        }

        /// <summary>
        /// Usuwa hosta.
        /// </summary>
        public void DestroyHost()
        {
            if (hostInitialized)
                _DestroyHost();
        }

        #endregion PUBLIC_FUNCTIONS

        #region PRIVATE_FUNCTIONS

        /// <summary>
        /// Rozłączenie serwera.
        /// </summary>
        /// <param name="player"></param>
        protected void OnPlayerDisconnected(NetworkPlayer player)
        {
            Debug.Log("Player " + player + " disconnected");
            if(onPlayerDisconnected_delegate != null) onPlayerDisconnected_delegate(player);
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
            m_listOfPlayersConnectedToHost.Remove(player);
        }

        /// <summary>
        /// Usuwa hosta.
        /// </summary>
        protected void _DestroyHost()
        {
            m_hostInitialized = false; ;
            Debug.Log("Network resources released");
            if (Network.isServer)
            {
                MasterServer.UnregisterHost();
                Network.RemoveRPCsInGroup(0);
            }
            Network.Disconnect();
        }

        /// <summary>
        /// Inicjacja hosta.
        /// </summary>
        protected void _InitHost()
        {
            Debug.Log("Inicializing host...");
            m_hostInitialized = false;
            m_hostFailed = false;
            m_hostStatus = "Initializing";
            NetworkConnectionError error = Network.InitializeServer(GameConfig.maxNumberOfPlayers, GameConfig.gamePort, m_useNat);
            if (error != NetworkConnectionError.NoError)
            {
                m_hostStatus = "Error: " + error;
                Debug.Log("Can not create Host. " + m_hostStatus);
                m_hostFailed = true;
            }
        }

        /// <summary>
        /// Gdy host został zainicjowany.
        /// </summary>
        protected void OnServerInitialized()
        {
            Debug.Log("Host initialized at internal IP:" + Network.player.ipAddress + ", external IP:" + Network.player.externalIP);
            m_hostStatus = "Loading " + GameConfig.playerName.ToString();
            if (m_useMasterServer)
            {
                Debug.Log("Adding host to global server");
                MasterServer.RegisterHost(GameConfig.GetDefaultGameType(), GameConfig.GetFullGameName(), GameConfig.playerName.ToString());
            }
            else
            {
                Debug.Log("Not added host to global server");
            }
            m_hostInitialized = true;
        }

        /// <summary>
        /// Zdarzenie podłączenia nowego klienta.
        /// </summary>
        /// <param name="player"></param>
        protected void OnPlayerConnected(NetworkPlayer player)
        {
            if (
                m_allowJoinToServer &&
                getNumberOfConnections >= getNumberOfMaxConnections
                )
            {
                Network.RemoveRPCs(player);
                Network.DestroyPlayerObjects(player);
            }
            else
            {
                Debug.Log("Player " + player + " connected");
                m_listOfPlayersConnectedToHost.Add(player);
                if (onPlayerConnected_delegate != null) onPlayerConnected_delegate(player);
            }
        }

        #endregion PRIVATE_FUNCTIONS

        #region DELEGATE

        /// <summary>
        /// Delegat wykonywany po podłączeniu gracza do hosta - wykonywany po stronie hosta.
        /// </summary>
        /// <param name="player"></param>
        public delegate void OnPlayerConnected_delegate(NetworkPlayer player);

        /// <summary>
        /// Delegat wykonywany po rozłączeniu klienta od hosta - wykonywany po stronie hosta.
        /// </summary>
        /// <param name="player"></param>
        public delegate void OnPlayerDisconnected_delegate(NetworkPlayer player);

        #endregion DELEGATE

        #endregion
    }
}