using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    public bool pressed;

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "Wall")
        {
            pressed = true;
        }
    }

}
