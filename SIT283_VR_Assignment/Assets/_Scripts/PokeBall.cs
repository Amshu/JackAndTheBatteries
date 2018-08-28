using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the main behaviour of the pokeball
public class PokeBall : MonoBehaviour
{

    [SerializeField] Material colorChange;
    [SerializeField] GameObject colourMesh;

    [SerializeField] AudioClip bounce;
    new AudioSource audio;

    [SerializeField] GameObject parent;

    // Boll to check if its in state of transportation ball
    bool teleportation = false;

    private void Start()
    {
        if (parent == null)
        {
            parent = gameObject.transform.parent.gameObject;
        }

        audio = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        audio.PlayOneShot(bounce, 0.5f);

        // Handle collision with floor
        if (other.transform.tag.Equals("Floor"))
        {
            //Debug.Log("Collided with  Floor");
            StartCoroutine("Kill");

            if (teleportation)
            {
                //Debug.Log("Transporting");
                GameObject.Find("P1_Alien").transform.position = parent.transform.position;
            }

        }
        // Make it a transportation ball when it touches the paint brush
        else if (other.transform.tag == "PaintBrush")
        {
            //Debug.Log("Collided with PaintBrush");
            teleportation = true;

            // Change colour of the pokeball to the material of the paint brush tip
            colourMesh.GetComponent<Renderer>().material = colorChange;

        }

        // Handle collisions for P2
        else if (other.transform.tag == "Player2" && !teleportation)
        {
            // Call Manger script and call GameOver function
            GameObject.Find("SceneManager").GetComponent<Manager>().DidP2Win(false);
        }
    }

    // A delay coroutine to destroy the pokeball
    IEnumerator Kill()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            //Transport();
            Destroy(parent.gameObject);
        }
    }
}
