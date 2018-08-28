using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCol : MonoBehaviour {

    public bool p2_ready;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Floor")
        {
            p2_ready = true;
        }
    }
}
