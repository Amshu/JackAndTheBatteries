using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Script for emulating the autonomous behaviour of AI
public class AI_Chase : MonoBehaviour {

    // Audio Clips for feedback
    [SerializeField] AudioClip hunt;
    new AudioSource audio;

    // State machine to store the different states of the AI
    enum AIState {Patrol, Chase};
    AIState curState; // Enum variable to store state

    // Integer to store the room code
    int goToRoom;

    public NavMeshAgent agent; 

    // Function to change states
    void SetState(AIState newState)
    {
        //if (curState == newState) return;
       
        curState = newState;
        HandleStateChangedEvent(newState);
    }

    // Function to change the AI states.
    void HandleStateChangedEvent(AIState state)
    {
        if (state == AIState.Patrol)
        {
            StartCoroutine(Patrol());
        }
        else
        {
            StartCoroutine(Chase());
        }
    }

    void Start()
    {
        SetState(AIState.Chase);
        audio = gameObject.AddComponent<AudioSource>();
        audio.PlayOneShot(hunt, 0.2f);
    }

    
    void RunAI()
    {
        if(curState == AIState.Chase)
        {
            StartCoroutine(Chase());
        }
        else
        {
            StartCoroutine(Patrol());
        }
    }

    // Coroutine for Chase State
    IEnumerator Chase()
    {
       
        //Set the destination as the players position.
        GameObject temp = GameObject.Find("P2_Astronaut");
        agent.SetDestination(temp.transform.position);
        //transform.LookAt(temp.transform);
        //audio.PlayOneShot(hunt);

        // The rate at which the AI chases the player can be adjusted here - for further balancing
        yield return new WaitForSeconds(0.5f);
        RunAI();
        
    }

    // This function is called from the player script on collision
    public void OnPlayerHit() { SetState(AIState.Patrol); }

    // Patrol Function
    IEnumerator Patrol()
    {
        // Get the next room
        goToRoom = Random.Range(1, 5);
        //Debug.Log("Going to room: " + goToRoom);
        Vector3 pos = GetRandomLoc(goToRoom);
        agent.SetDestination(pos);
       

        // Run Ai function again after sometime
        yield return new WaitForSeconds(2);
        SetState(AIState.Chase);
    }

    // Function which calculates the next random the location the AI needs to go
    Vector3 GetRandomLoc(int room)
    {
        // Variables to store the random calculated postions
        float x, z;

        switch (room)
        {
            // Room 1
            case 1:
                x = Random.Range(-99.5f, -52.4f);
                z = Random.Range(-29.3f, 14.5f);
                return new Vector3(x, -0.5f, z);
                //Debug.Log("x = " + x + " , z = " + z);

            // Hall
            case 2:
                x = Random.Range(-46.3f, 3.9f);
                z = Random.Range(-3.2f, -54.8f);
                return new Vector3(x, -0.5f, z);
                //Debug.Log("x = " + x + " , z = " + z);

           // Kitchen
            case 3:
                x = Random.Range(17.5f, 83f);
                z = Random.Range(26.4f, 78.9f);
                return new Vector3(x, -0.5f, z);
                //Debug.Log("x = " + x + " , z = " + z);

            // Room 2
            case 4:
                x = Random.Range(12.4f, 85.5f);
                z = Random.Range(-54.8f, 3.2f);
                return new Vector3(x, -0.5f, z);
                //Debug.Log("x = " + x + " , z = " + z);


            // Corridor
            case 5:
                x = Random.Range(-98.5f, 15.4f);
                z = Random.Range(48.5f, 80.3f);
                return new Vector3(x, -0.5f, z);
            //Debug.Log("x = " + x + " , z = " + z);

            default:
                return new Vector3(0, -0.5f, 0);
        }
    }
}
