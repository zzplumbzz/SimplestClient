using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class NetworkedClient : MonoBehaviour
{

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;//5496
    byte error;
    bool isConnected = false;
    int ourClientID;

    GameObject gameSystemManger;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("NetworkedClient start/ is connected");
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach(GameObject go in allObjects)
        {
            if(go.GetComponent<GameSystemManger>() != null)
                gameSystemManger = go;
                
        }

        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            SendMessageToHost("Hello from client");

        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }
    
    private void Connect()
    {
        Debug.Log("Network client connect");
        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.1.39", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }
    
    public void Disconnect()
    {Debug.Log("Disconnected()");
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    
    public void SendMessageToHost(string msg)
    {Debug.Log("Send message to host()");
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');

        int signifier = int.Parse(csv[0]);

        if(signifier == ServerToClientSignifiers.AccountCreationComplete)
        {
            Debug.Log("Account creation complete NC");
            gameSystemManger.GetComponent<GameSystemManger>().ChangeState(GameStates.MainMenu);
        }
        else if(signifier == ServerToClientSignifiers.LoginComplete)
        {
            Debug.Log("Log Comp NC");
            gameSystemManger.GetComponent<GameSystemManger>().ChangeState(GameStates.MainMenu);
            
        }
        else if(signifier == ServerToClientSignifiers.GameStart)
        {
            Debug.Log("GAME FINALLY STARTED!!!!!!!!!!!!!!!!!!!!!!!!");
            gameSystemManger.GetComponent<GameSystemManger>().ChangeState(GameStates.TicTacToe);
        }
        else if(signifier == ServerToClientSignifiers.OpponentPlay)
        {
             Debug.Log("Opponent Play!");
        }

       


    }

    public bool IsConnected()
    {Debug.Log("IS Connected NC");
        return isConnected;
    }



}

public static class ClientToServerSignifiers
{
    public const int CreateAccount = 1;

    public const int Login = 2;

    public const int JoinQueueForGameRoom = 3;

    public const int TicTacToePlay = 4;

    //public const int OpponentPlay = 5;


}

public static class ServerToClientSignifiers
{
    public const int LoginComplete = 11;
     public const int LoginFailed = 12;
    

    public const int AccountCreationComplete = 13;
    public const int AccountCreationFailed = 14;

public const int OpponentPlay = 15;

public const int GameStart = 16;

}

// public static class LoginResponses
// {
//     public const int Success = 21;
//      public const int FailureNameInuse = 22;
//     public const int FailureNameNotFound = 23;
//     public const int FailureIncorrectPassword = 24;
// }

