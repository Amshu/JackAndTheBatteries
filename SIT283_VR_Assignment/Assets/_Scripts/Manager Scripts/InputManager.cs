using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InputManager : MonoBehaviour {

    public bool GameOver;

    // Player 1 Game Object
    [SerializeField] GameObject p1;
    Controller cont;

    // Variables for P1
    // float t;
    // float g;
    bool trig;
    bool grip;

    //Player2 Game Object and Camera
    [SerializeField] GameObject p2;
    [SerializeField] GameObject p2_camera;

    // Variables for Player 2
    float v;
    float h;
    bool j;
    float mouseX;
    float mouseY;

	// Use this for initialization
	void Start () {

        // Check to make sure all the Gameobjects are refered
        if(p1 == null)
        {
            Debug.Log("P1 Controller reference is not attached to manager");
        }

        cont = p1.GetComponent<Controller>();

        if(p2 == null)
        {
            p2 = GameObject.Find("P2_Astronaut");
        }

        if (p2_camera == null)
        {
            p2_camera = GameObject.Find("P2_Camera");
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Home"))
        {
            SceneManager.LoadScene(0);
        }
        if (!GameOver) {

            /* This is to show that I was familiar with this method too however using this created a lot of spwanning balls and 
                made the controllers feel very unresponsive. This this method was not used. 

                If used it was a matter of just changing the function parameter types in the P1_RHand script but it created a 
                problem as stated above since it used axis which threw a lot of values as its called every frame. Just one 
                press passed the value 1 for many screens which made it spawn many objects. 

                I left the code here to show that I was familiar with getting inputs this way too.

                t = Input.GetAxis("Pokeball");
                g = Input.GetAxis("Grip");
            */

            //Player 1 - Alien Inputs
            if (cont.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
            {
                grip = true;
            }
            else grip = false;

            if (cont.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) trig = true;
            else if (cont.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) trig = false;

            //Send inputs to P1_RHand script
            p1.GetComponentInChildren<P1_RHand>().handControl(trig, grip);

            //Player 2 - Astronaut Inputs
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
            j = Input.GetButtonDown("Jump");

            //Player 2 Inputs for Camera
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            //Send inputs to move funciton in Player2 script
            p2.GetComponent<Player2>().MovePlayer(v, h, j);

            //Send mouse iunpuits to camera script
            p2_camera.GetComponent<Player2_Cam>().moveCamera(mouseX, mouseY);
        }   

    }
}
