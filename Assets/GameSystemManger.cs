using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;


public class GameSystemManger : MonoBehaviour
{

    GameObject submitButton, userNameInput, passwordInput, createToggle, loginToggle;

    GameObject textNameInfo, textPasswordInfo;

    GameObject joinGameRoomButton;

    GameObject networkedClient;

    GameObject ticTacToeSquareButton;

    GameObject quitButton;

    GameObject gameCanvas;

    GameObject menuCanvas;

    //static GameObject instance;
    // Start is called before the first frame update
    void Start()
    {
        //instance = this.gameObject;

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();


        foreach(GameObject go in allObjects)
        {
            if(go.name =="UserNameInput")
                userNameInput = go;
            else if(go.name == "PasswordInput")
                passwordInput = go;
            else if(go.name == "SubmitButton")
                submitButton = go;
            else if(go.name == "LoginToggle")
                loginToggle = go;
            else if(go.name == "CreateToggle")
                createToggle = go;
            else if(go.name == "NetworkedClient")
                networkedClient = go;
            else if(go.name == "JoinGameRoomButton")
                joinGameRoomButton = go;
                else if(go.name == "UserName")
                textNameInfo = go;
                else if(go.name == "Password")
                textPasswordInfo = go;
                else if(go.name == "TicTacToeSquareButton")
                ticTacToeSquareButton = go;
                 else if(go.name == "QuitButton")
                quitButton = go;
                else if(go.name == "GameCanvas")
                gameCanvas = go;
                else if(go.name == "MenuCanvas")
                menuCanvas = go;



                
        }

        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);

        loginToggle.GetComponent<Toggle>().onValueChanged.AddListener(LoginToggleChanged);

        createToggle.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);

        joinGameRoomButton.GetComponent<Button>().onClick.AddListener(JoinGameRoomButtonPressed);

        ticTacToeSquareButton.GetComponent<Button>().onClick.AddListener(TicTacToeSquareButtonPressed);

        quitButton.GetComponent<Button>().onClick.AddListener(QuitButtonPressed);
        

        ChangeState(GameStates.LoginMenu);
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitButtonPressed()
    {

        string p = passwordInput.GetComponent<InputField>().text;

        string n = userNameInput.GetComponent<InputField>().text;

        string msg;

        if(createToggle.GetComponent<Toggle>().isOn)
        msg = ClientToServerSignifiers.CreateAccount + "," + n + "," + p;
        else
        msg = ClientToServerSignifiers.Login + "," + n + "," + p;

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

        Debug.Log(msg);

        
    }

    public void LoginToggleChanged(bool newValue){
        Debug.Log("Login Toggle");

        createToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);

    }

    public void CreateToggleChanged(bool newValue){
        Debug.Log("create Toggle");
        loginToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);


    }

    public void ChangeState(int newState)
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
        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);

        if(newState == GameStates.LoginMenu)
        {
            submitButton.SetActive(true);
            userNameInput.SetActive(true);
            passwordInput.SetActive(true);
            createToggle.SetActive(true);
            loginToggle.SetActive(true);
            menuCanvas.SetActive(true);

            textNameInfo.SetActive(true);
            textPasswordInfo.SetActive(true);
            ticTacToeSquareButton.SetActive(false);
        }
        else if(newState == GameStates.MainMenu)
        {
            quitButton.SetActive(true);
            joinGameRoomButton.SetActive(true);
            menuCanvas.SetActive(true);
        }
        else if(newState == GameStates.WaitingInQueueForOtherPlayer)
        {
             quitButton.SetActive(true);
             ticTacToeSquareButton.SetActive(true);
             menuCanvas.SetActive(true);
            //joinGameRoomButton.SetActive(true);
        }
        else if(newState == GameStates.TicTacToe)
        {
            ticTacToeSquareButton.SetActive(true);
            quitButton.SetActive(true);
            gameCanvas.SetActive(true);

           
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

     public void JoinGameRoomButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinQueueForGameRoom + "");
        ChangeState(GameStates.WaitingInQueueForOtherPlayer);
        
    }

    public void TicTacToeSquareButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinQueueForGameRoom + "");
        ChangeState(GameStates.TicTacToe);
        // networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinQueueForGameRoom + "");
        // ChangeState(GameStates.WaitingInQueueForOtherPlayer);
        
    }

    public void QuitButtonPressed() 
    {
        
        ChangeState(GameStates.LoginMenu);
    }

    static public class GameStates
    {
        public const int LoginMenu = 1;
        public const int MainMenu = 2;
        public const int WaitingInQueueForOtherPlayer = 3;
        public const int TicTacToe = 4;

         public const int OpponentPlay = 5;
    }

}
