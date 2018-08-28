using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Script for emulating the autonomous behaviour of AI
public class AI_Random : MonoBehaviour {

    // Audio Clips for feedback
    [SerializeField] AudioClip hit_brush;
    [SerializeField] AudioClip hunt;
    new AudioSource audio;

    // State machine to store the different states of the AI
    enum AIState {Patrol, Chase, Lazy};
    AIState curState; // Enum variable to store state

    // Integer to store the room code
    int goToRoom;

    bool isLazy;

    public NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        // Run the AI Function
        RunAI();
        if(audio == null) { audio = gameObject.AddComponent<AudioSource>(); }
        
    }

    // Function to change states
    void SetState(AIState newState)
    {
        //if (curState == newState) return;
       
        curState = newState;
        HandleStateChangedEvent(newState);
    }

    // Function to set up animations on state changes
    void HandleStateChangedEvent(AIState state)
    {
        if (state == AIState.Patrol)
        {
            StartCoroutine(Patrol());
            isLazy = false;
        }
        else if (state == AIState.Chase)
        {
            StartCoroutine(Chase());
            isLazy = false;
        }
        else
        {
            isLazy = true; // Make it do nothing
            StartCoroutine(Lazy());
        }
    }

    // AI function which randomly chooses its states.
    void RunAI()
    {
        // If the Ai is lazy dont do anything
        if (!isLazy)
        {
            // If not lazy then pick a number from 1 to 20
            int i = Random.Range(1, 4);
            //Debug.Log("/////////////////////// - " + i);

            // If number divisible by 3 - Hunt
            if (i == 2)
            {
                SetState(AIState.Chase);
            }
            // If number divisible by 4 - Lazy(lesser probabalility)
            if (i == 1)
            {
                //isLazy = true;
                SetState(AIState.Lazy);
            }
            // Or keep patrolling
            else
            {
                SetState(AIState.Patrol);
            }
        }
        else
        {
            StartCoroutine(Lazy());
        }
        Debug.Log("AI state is now: " + curState);
    }


    // Patrol Function
    IEnumerator Patrol()
    {
        // Get the next room
        goToRoom = Random.Range(1, 5);
        //Debug.Log("Going to room: " + goToRoom);
        Vector3 pos = GetRandomLoc(goToRoom);
        agent.SetDestination(pos);

        // Run Ai function again after sometime
        yield return new WaitForSeconds(Random.Range(1, 6));
        RunAI();
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

    // Coroutine for Hunt - Chase player 2
    IEnumerator Chase()
    {
        while(curState == AIState.Chase)
        {
            agent.SetDestination(GameObject.Find("P2_Astronaut").transform.position);
            audio.PlayOneShot(hunt, 0.2f);
            // Chase the Player for 1-6 seconds.
            yield return new WaitForSeconds(Random.Range(1, 6));
            RunAI();
        }
    }

    IEnumerator Lazy()
    {
        // Be lazy for 10-16 seconds
        yield return new WaitForSeconds(Random.Range(8, 15));
        isLazy = false;
        RunAI();
    }

    // Function to hanlde collisions
    // This function is called from the player script on collision
    public void OnPlayerHit() { SetState(AIState.Patrol); }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PaintBrush")
        {
            audio.PlayOneShot(hit_brush);
            isLazy = false;
            RunAI();
        }
    }
}
