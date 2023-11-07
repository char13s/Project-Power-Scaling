using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private bool pause;
    public static event UnityAction<bool> pauseScreen;
    public static event UnityAction<int> switchMap;
    public static event UnityAction gameOver;
    //private Game tempSave;
    private int orbAmt;
    private int lastLevel;
    public enum GameState { Paused, PlayMode,Dialogue }
    private GameState currentState;
    public GameState CurrentState { get => currentState; set { currentState = value; StateMappings(); } }
    public int OrbAmt { get => orbAmt; set => orbAmt = value; }
    public int LastLevel { get => lastLevel; set => lastLevel = value; }

    public static GameManager GetManager() => instance;
    // Start is called before the first frame update
    private void Awake() {
        DontDestroyOnLoad(this);
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        CurrentState = GameState.Paused;
        
    }

    void Start() {
        //DialogueTrigger.gameMode += DialogueTriggered;
        //PlayerInputs.pause += PauseGame;
        //ExperimentalInputs.pause += PauseGame;
        //LevelManager.gameMode += GameStateControl;
        //Stats.onOrbGain += Collect;
        //PlayerInputs.playerEnabled += StateMappings;
        //NewZend.onPlayerDeath += HandlePlayerDeath;
        //CheckPoint.onCheckPoint += SetCheck;
        //LoadingCanvas.loadPlayer += RespawnPlayer;
        //NewZend.playerEnabled += AssignPlayer;
        //PauseCanvas.pause += PauseGame;
    }
    private void AssignPlayer() {

       // tempSave = new Game(Zend);
    }
    public void PauseGame() {
        if (pause) {
            pause = false;
            Time.timeScale = 1;
            CurrentState = GameState.PlayMode;
        }
        else {
            pause = true;
            Time.timeScale = 0;
            CurrentState = GameState.Paused;
        }
        pauseScreen.Invoke(pause);
    }
    private void HandlePlayerDeath() {
        //kill player controls
        //tell level manager to send back to main menu
        if (switchMap != null)
            switchMap(99);
        //gameOver.Invoke();
    }
    private void DialogueTriggered() {
        CurrentState = GameState.Dialogue;
        Time.timeScale = 0;
    }
    private void GameStateControl(bool val) {
        if (val) {
            CurrentState = GameState.PlayMode;
        }
        else {
            CurrentState = GameState.Paused;
        }
    }
    void StateMappings() {
        switch (currentState) {
            case GameState.PlayMode:
                if(switchMap!=null)
                    switchMap(0);
                break;
            case GameState.Paused:
                if (switchMap != null)
                    switchMap(1);
                break;
            case GameState.Dialogue:
                if (switchMap != null)
                    switchMap(4);
                break;
        }
    }
    private void SetCheck(GameObject val) {
        //checkPoint = val;
       // tempSave.Stats = zend.stats;
    }
    private void RespawnPlayer() {
        //NewZend tempZ = Instantiate(zend, checkPoint.transform.position, Quaternion.identity);
        //tempZ.stats=tempSave.Stats;
    }
    private void Collect(int amt) {
        orbAmt += amt;
    }
    public void QuitGame() {
        Application.Quit();
    }
}
