using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Script for emulating the autonomous behaviour of AI
public class AI_Patrol : MonoBehaviour {

    // Integer to store the room code
    int goToRoom;

    // Nav mesh component
    public NavMeshAgent agent;

    public bool end;

    // Use this for initialization
    void Start()
    {
        // Run the Patrol Function
        StartCoroutine(Patrol());

        end = false;
    }

    // This function is called from the player script on collision
    public void OnPlayerHit() { StartCoroutine(Patrol()); }

    // Patrol Function
    IEnumerator Patrol()
    {
        // If the user has collected all the batteries
        if (end)
        {
            // Patrol the exit door
            agent.SetDestination(new Vector3(-80f, -0.25f, 50f));
            yield return new WaitForSeconds(1f);
            agent.SetDestination(new Vector3(-80f, -0.25f, 80f));
            yield return new WaitForSeconds(1f);
        }
        else
        {
            // Get the next room
            goToRoom = Random.Range(1, 5);
            //Debug.Log("Going to room: " + goToRoom);
            Vector3 pos = GetRandomLoc(goToRoom);
            agent.SetDestination(pos);

            // Run function again after sometime
            yield return new WaitForSeconds(4);
        }

        StartCoroutine(Patrol());
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
