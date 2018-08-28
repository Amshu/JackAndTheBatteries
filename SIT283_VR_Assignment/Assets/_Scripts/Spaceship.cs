using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "P2_Astronaut")
        {
            GameObject.Find("SceneManager").GetComponent<Manager>().DidP2Win(true);
        }
    }
}
