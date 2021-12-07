using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;


public class GameSystemManger : MonoBehaviour
{

    public Board boardScript;
    public Box boxScript;

    GameObject submitButton, userNameInput, passwordInput, createToggle, loginToggle;

    GameObject textNameInfo, textPasswordInfo;

    GameObject joinGameRoomButton;

    GameObject networkedClient;

    GameObject ticTacToeSquareButton;

    GameObject quitButton;

    GameObject menuCanvas;

    GameObject board;

    GameObject box;

    GameObject HelloCB;
    GameObject GGCB;

    GameObject ReplayButton;

    public string Hello;
    public string GoodGame;





    // Start is called before the first frame update
    void Start()
    {


        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();


        foreach (GameObject go in allObjects)
        {
            //setting up all game objects in editor
            if (go.name == "UserNameInput")
                userNameInput = go;
            else if (go.name == "PasswordInput")
                passwordInput = go;
            else if (go.name == "SubmitButton")
                submitButton = go;
            else if (go.name == "LoginToggle")
                loginToggle = go;
            else if (go.name == "CreateToggle")
                createToggle = go;
            else if (go.name == "NetworkedClient")
                networkedClient = go;
            else if (go.name == "JoinGameRoomButton")
                joinGameRoomButton = go;
            else if (go.name == "UserName")
                textNameInfo = go;
            else if (go.name == "Password")
                textPasswordInfo = go;
            else if (go.name == "TicTacToeSquareButton")
                ticTacToeSquareButton = go;
            else if (go.name == "QuitButton")
                quitButton = go;
            else if (go.name == "MenuCanvas")
                menuCanvas = go;
            else if (go.name == "Board")
                board = go;
            else if (go.name == "Box")
                box = go;
            else if (go.name == "HelloButton")
                HelloCB = go;
            else if (go.name == "GGButton")
                GGCB = go;
            else if (go.name == "ReplayButton")
                ReplayButton = go;



        }
        //button listeners
        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        loginToggle.GetComponent<Toggle>().onValueChanged.AddListener(LoginToggleChanged);
        createToggle.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);
        joinGameRoomButton.GetComponent<Button>().onClick.AddListener(JoinGameRoomButtonPressed);
        ticTacToeSquareButton.GetComponent<Button>().onClick.AddListener(TicTacToeSquareButtonPressed);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitButtonPressed);
        HelloCB.GetComponent<Button>().onClick.AddListener(HelloCBPressed);
        GGCB.GetComponent<Button>().onClick.AddListener(GGCBPressed);
        ReplayButton.GetComponent<Button>().onClick.AddListener(ReplayButtonPressed);

        ChangeState(GameStates.Login);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SubmitButtonPressed()
    {
        Debug.Log("Submit button pressed");

        string p = passwordInput.GetComponent<InputField>().text;
        string n = userNameInput.GetComponent<InputField>().text;

        string msg;

        if (createToggle.GetComponent<Toggle>().isOn)
            msg = ClientToServerSignifiers.CreateAccount + "," + n + "," + p;
        else
            msg = ClientToServerSignifiers.Login + "," + n + "," + p;

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

        Debug.Log(msg);

    }

    private void LoginToggleChanged(bool newValue)// setting toggle on press and disabling the other toggle
    {
        Debug.Log("Login Toggle");

        createToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);

    }

    private void CreateToggleChanged(bool newValue)
    {
        Debug.Log("create Toggle");
        loginToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);


    }

    private void JoinGameRoomButtonPressed()
    {
        Debug.Log("Joining Queue button pressed");
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinQueueForGameRoom + "");

        ChangeState(GameStates.WaitingForMatch);

    }
    private void TicTacToeSquareButtonPressed()
    {
        Debug.Log("Tic tac toe button pressed");
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "");
        ChangeState(GameStates.TicTacToe);
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ServerToClientSignifiers.UpdateClientsBoard + "");

    }

    public void QuitButtonPressed()// quits game back to login screen
    {

        ChangeState(GameStates.Login);
    }
    public void HelloCBPressed()// when button pressed send to server to send to other client
    {
        Debug.Log("Hello");
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.HelloButtonPressed + "Hello host" + Hello);
        
    }

    public void GGCBPressed()// when button pressed send to server to send to other client
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.GGButtonPressed + "Good Game host" + GoodGame);
        
        Debug.Log("Good Game!");

    }

    public void ReplayButtonPressed()// supposed to replay the game when complete
    {
        
    }

    public void ChangeState(int newState)// changing the different states
    {
        joinGameRoomButton.SetActive(false);
        submitButton.SetActive(false);
        userNameInput.SetActive(false);
        passwordInput.SetActive(false);
        createToggle.SetActive(false);
        loginToggle.SetActive(false);
        textNameInfo.SetActive(false);
        textPasswordInfo.SetActive(false);
        joinGameRoomButton.SetActive(false);
        ticTacToeSquareButton.SetActive(false);
        quitButton.SetActive(false);
        menuCanvas.SetActive(false);
        board.SetActive(false);
        box.SetActive(false);
        HelloCB.SetActive(false);
        GGCB.SetActive(false);
        ReplayButton.SetActive(false);


        if (newState == GameStates.Login)
        {
            Debug.Log("Login menu state");

            submitButton.SetActive(true);
            userNameInput.SetActive(true);
            passwordInput.SetActive(true);
            createToggle.SetActive(true);
            loginToggle.SetActive(true);
            menuCanvas.SetActive(true);
            textNameInfo.SetActive(true);
            textPasswordInfo.SetActive(true);

        }
        else if (newState == GameStates.MainMenu)
        {
            Debug.Log("Main menu state");
            quitButton.SetActive(true);
            joinGameRoomButton.SetActive(true);
            menuCanvas.SetActive(true);
        }
        else if (newState == GameStates.WaitingForMatch)
        {

            Debug.Log("In Queue state");
            quitButton.SetActive(true);
            




        }
        else if (newState == GameStates.TicTacToe)
        {

            Debug.Log("In Game state");
            ticTacToeSquareButton.SetActive(false);
            quitButton.SetActive(true);
            board.SetActive(true);
            box.SetActive(true);
            GGCB.SetActive(true);
            HelloCB.SetActive(true);


            joinGameRoomButton.SetActive(false);
            submitButton.SetActive(false);
            userNameInput.SetActive(false);
            passwordInput.SetActive(false);
            createToggle.SetActive(false);
            loginToggle.SetActive(false);

            textNameInfo.SetActive(false);
            textPasswordInfo.SetActive(false);


        }

        else if (newState == GameStates.OpponentPlay)
        {
            Debug.Log("opponent play state");
            ticTacToeSquareButton.SetActive(false);
            joinGameRoomButton.SetActive(false);
            quitButton.SetActive(true);
            board.SetActive(true);
            box.SetActive(true);
            GGCB.SetActive(true);
            HelloCB.SetActive(true);



        }

        else if (newState == GameStates.Win)
        {

            quitButton.SetActive(true);
            board.SetActive(false);
            box.SetActive(false);
            GGCB.SetActive(true);
            HelloCB.SetActive(true);
            ReplayButton.SetActive(true);



            joinGameRoomButton.SetActive(false);
            submitButton.SetActive(false);
            userNameInput.SetActive(false);
            passwordInput.SetActive(false);
            createToggle.SetActive(false);
            loginToggle.SetActive(false);

            textNameInfo.SetActive(false);
            textPasswordInfo.SetActive(false);

            joinGameRoomButton.SetActive(false);
            ticTacToeSquareButton.SetActive(false);
        }

    }
}










static public class GameStates
{
    public const int Login = 1;
    public const int MainMenu = 2;
    public const int WaitingForMatch = 3;
    public const int TicTacToe = 4;
    public const int OpponentPlay = 5;
    public const int Win = 6;
}
