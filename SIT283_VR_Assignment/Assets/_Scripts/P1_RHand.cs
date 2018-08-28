using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class P1_RHand : MonoBehaviour {

    // Prefab model of the hand 
    [SerializeField] GameObject handGrip;
    [SerializeField] GameObject handFree;

    [SerializeField] AudioClip pickup;
    [SerializeField] AudioClip release;
    new AudioSource audio;

    GameObject heldObject;
    Controller controller;

    //Position of the object on hand when held.
    public Vector3 holdPos;

    //Pokeball prefab
    public GameObject toy;

    // Rigid body for  throwing objects
    Rigidbody simulator;


    // Function to change the hand modles when trigger is pressed.
    private void setHand(bool grip)
    {
        handGrip.SetActive(grip);
        handFree.SetActive(!grip);         
    }

    private void Start()
    {
        // Set hand model to free
        setHand(false);
        audio = gameObject.AddComponent<AudioSource>();
        // Attach a rigid body component to camera rig.
        simulator = new GameObject().AddComponent<Rigidbody>();
        simulator.name = "simulator";
        simulator.transform.parent = transform.parent;
        controller = GetComponent<Controller>();
    }

    public void handControl(bool trig, bool grip)
    {
        // Set model of hand
        setHand(trig);

        // If grip button is pressed
        if (grip)
        {
            // Spawn Pokeball slightly above the hand
            GameObject spawn = Instantiate(toy);
            spawn.transform.position = transform.position + new Vector3(0.0f, 4, 0.0f);
        }

        if (heldObject)
        {
            // Record the force collected while moving the hand by calculating the distance between the controller origin and cameraRig origin every frame. 
            simulator.velocity = (transform.position - simulator.position) * 100f;

            // If trigger is released when player is holding an object
            if (!trig)
            {
                // Release held object
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                // Add the velocity calculated above to the object
                heldObject.GetComponent<Rigidbody>().velocity = simulator.velocity;
                heldObject.GetComponent<PickupObject>().parent = null;
                heldObject = null;
                audio.PlayOneShot(release);
            }
        }

        else
        {
            // If trigger is pressed
            if (trig)
            {
                // Create a collision shpere to detect any collisions
                Collider[] cols = Physics.OverlapSphere(transform.position, 0.1f);

                // Itereate through the collided objects and select only the pickable objects
                foreach (Collider col in cols)
                {
                    // Object is pickable only if they have a PickUpObject component
                    if (heldObject == null && col.GetComponent<PickupObject>() && col.GetComponent<PickupObject>().parent == null)
                    {
                        // Haptic Feedback
                        controller.controller.TriggerHapticPulse(399);

                        audio.PlayOneShot(pickup);

                        // Attach it to the controller
                        heldObject = col.gameObject;
                        heldObject.transform.parent = transform;      

                        heldObject.transform.localPosition = Vector3.zero;
                        // Offset attach position to position it correctly
                        heldObject.transform.position += new Vector3(-0.1f, 0.3f, 0.1f);
                        // Make the object immune to any other collisions or forces
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                        heldObject.GetComponent<PickupObject>().parent = controller;
                    }
                }
            }
        }
    }
    
}
