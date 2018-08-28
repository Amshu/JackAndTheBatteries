using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class Manager : MonoBehaviour {

    //Background Music
    [SerializeField] AudioClip music;
    [SerializeField] AudioClip bat_Col;
    new AudioSource audio;

    // Get the number of batteries to be collected
    private int batteries = 3;

    // Prefab array for the Ai cats
    public GameObject[] aiBots;

    // References to UI objects
    [SerializeField] GameObject p1_ui;
    [SerializeField] Text p2_ui;
    [SerializeField] GameObject p1_battery;

    //Reference to the spawner
    [SerializeField] BatterySpawner spawnner;

    // All the states of the game
    private enum GameState { Blank, Begin, Battery1, Battery2, Battery3, P1GameWin, P2GameWin }
    private GameState curState = GameState.Blank;


    // Getter for the amount of batteries
    public int GetBatteries() { return batteries; }


    // Use this for initialization
    private void Start()
    {
        if (spawnner == null) spawnner = gameObject.GetComponentInChildren<BatterySpawner>();

        //Debug.Log("Is this start running");
        audio = gameObject.AddComponent<AudioSource>();
        SetState(GameState.Begin); 
    }


    void SetState(GameState newState)
    {
        if (curState == newState){
            return;
        }

        curState = newState;
        //Debug.Log(curState);
        HandleStateChangedEvent(curState);
    }


    // Function to set up animations on state changes
    void HandleStateChangedEvent(GameState state)
    {
        // Starting state of the game
        if (state == GameState.Begin)
        {
            //Debug.Log("If this is displaying why is there no audio???");
            audio.PlayOneShot(music, 0.3f);

            // Activate AI
            Instantiate(aiBots[0].gameObject);

            spawnner.Spawn(batteries); //Randomize battery spawns

           // DisplayGoal(); // Display Objective
        }        

        // When Player 2 picks up the first battery
        if (state == GameState.Battery1)
        { 
            DisplayGoal();
            Instantiate(aiBots[1].gameObject);
        }

        // When Player 2 picks up the second battery
        if (state == GameState.Battery2)
        {
            DisplayGoal();
            Instantiate(aiBots[2].gameObject);
        }

        // When Player 2 picks up the last battery
        if (state == GameState.Battery3)
        {
            DisplayGoal();
            Instantiate(aiBots[3].gameObject);
        }

        // State for when player 1 wins the game
        if (state == GameState.P1GameWin)
        {
            OnGameOver(1);
            gameObject.GetComponent<InputManager>().GameOver = true;
        }

        // State for when player 2 wins the game
        if (state == GameState.P2GameWin)
        {
            OnGameOver(2);
            gameObject.GetComponent<InputManager>().GameOver = true;
        }
    }


    // Display the number of batteries left to both Player1 and Player2
    public void DisplayGoal()
    {
        string obj = "/3 batteries need to be collected";

        // Display number of batteries on Player 1 paintbrush
        p1_battery.GetComponent<TextMesh>().text = batteries.ToString();
        if (batteries == 1) p1_battery.GetComponent<TextMesh>().color = Color.red; // If only one battery is remaining then show it in red

        // Display number of batteries on the canvas for Player 2
        p2_ui.gameObject.SetActive(true);
        if (batteries == 0)
        {
            p2_ui.text = "You found all the batteries, get back to your spaceship!";
        }
        else p2_ui.text = batteries + obj;

        // Start coroutine to disable UI after a few seconds
        StartCoroutine(Disable());
    }


    //Coroutine to disable the UI
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(3f);
        p1_ui.SetActive(false);
        p2_ui.gameObject.SetActive(false);
    }


    // Function to reduce the number of batteries once they are collected
    public void BatteryCollected()
    {  
        if(batteries == 3 && curState == GameState.Begin)
        {
            batteries--;
            audio.PlayOneShot(bat_Col);  // Play the audio for collecting the battery
            SetState(GameState.Battery1);
            return; // So that it does not go to the next if statement
        }

        if (batteries == 2 && curState == GameState.Battery1)
        {
            batteries--;
            audio.PlayOneShot(bat_Col);
            SetState(GameState.Battery2);
            return;
        }

        if (batteries == 1 && curState == GameState.Battery2)
        {
            batteries--;
            audio.PlayOneShot(bat_Col);
            SetState(GameState.Battery3);
            return;
        }

        if (batteries == 0 && curState == GameState.Battery3)
        { 
            batteries--;
            audio.PlayOneShot(bat_Col);
            return;
        }
        //Debug.Log(batteries);
    }


    // Public function which is called from the Pokeball & Spaceship script
    public void DidP2Win(bool win)
    {
        //If player 2 reaches the spaceship after collecting 3 batteries
        if(batteries == 0 && win)
        {
            SetState(GameState.P2GameWin);
        }

        // If player 1 catches player 2
        if(!win)
        {
            SetState(GameState.P1GameWin);
        }
    }


    // Call function when game is over
    void OnGameOver(int p)
    {
        //Initialise the UI text
        string p1_win = "You caught Player2! You Win. Press home to restart.";
        string p2_win = "You did it! You got all the batteries. You Win!";
        string p1_lose = "He got away with all your batteries. You Lose! Press home to restart.";
        string p2_lose = "You got caught. You Lose!";

        //Activate UI
        p1_ui.SetActive(true);
        p2_ui.gameObject.SetActive(true);

        // If player 1 Won
        if (p == 1)
        {
            p1_ui.GetComponent<TextMesh>().text = p1_win;
            p2_ui.text = p2_lose;
        }
        // If player 2 Won
        else
        {
            p1_ui.GetComponent<TextMesh>().text = p1_lose;
            p2_ui.text = p2_win;
        }
    }


    // Collision box to destroy any objects which fall off from the level
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name + " destroyed");
        Destroy(collision.gameObject);
    }
}
