using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {

    // Variables for audio clips
    [SerializeField] AudioClip hit_cat;
    [SerializeField] AudioClip hit_brush;
    [SerializeField] AudioClip jump;
    //[SerializeField] AudioClip land;

    new AudioSource audio;

    // Bool to see if the battery was collected
    bool batteryCollected;

    // Bool to see if the player is stunned
    bool isStunned;

    // Float to chek for a double jump
    private float dJump = 0;
   
    // Variables to control the animations of the character
    private Animator anim; 
    private enum AnimState { zero, idle, run, jump, djump, air};
    private AnimState state;

    // Character control to handel collisions without using rigid body physics
	private CharacterController controller;

    // Movement varialbles
	public float speed = 600.0f;
	public float turnSpeed = 400.0f;
    public float jumpSpeed = 15.0f;
	private Vector3 moveDirection = Vector3.zero;
	[SerializeField] private float gravity = 400.0f;

    // Function to change states
    void SetState(AnimState newState)
    {
        if (state == newState)
        {
            return;
        }
        state = newState;

        HandleStateChangedEvent(newState);
    }

    // Function to set up animations on state changes
    void HandleStateChangedEvent(AnimState state)
    {
        if (state == AnimState.idle)
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else if (state == AnimState.run)
        {
            anim.SetInteger("AnimationPar", 0);
        }
        else if (state == AnimState.jump)
        {
            anim.SetInteger("AnimationPar", 2);
        }
        else if (state == AnimState.djump)
        {
            anim.SetInteger("AnimationPar", 3);
        }
        else if (state == AnimState.air)
        {
            anim.SetInteger("AnimationPar", 3);
        }
    }


    void Start () {
        audio = gameObject.AddComponent<AudioSource>();
        state = AnimState.zero;
		controller = GetComponent <CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();
    }

    
    // Function which hanldes the movement and animation
    public void MovePlayer(float vertical, float horizontal, bool jump)
    {
        // Dont move if stunned
        if (!isStunned)
        {
            // Changing the animations according to the inputs
            if (vertical != 0 && !jump)
            {
                SetState(AnimState.idle);
            }
            else
            {
                SetState(AnimState.run);
            }

            // Check if player is on ground
            if (controller.isGrounded)
            {
                moveDirection = transform.forward * vertical * speed;

                // Reset Double Jump Checker
                dJump = 0;
            }
            else
            {
                if (jump && dJump == 0)
                {
                    SetState(AnimState.jump);
                }
                else if (jump && dJump == 1)
                {
                    SetState(AnimState.djump);
                }
            }

            // Call the jump function when Jump button is pressed
            if (jump)
            {
                JumpFunc();
            }

            // Set the rotation of the character according to the inputs
            transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);
            // Move using the character controller
            controller.Move(moveDirection * Time.deltaTime);
            // Add gravity to the character
            moveDirection.y -= gravity * Time.deltaTime;
        }
       
    }

    // Function which hanles the jump;
    void JumpFunc()
    {
        if(dJump == 0 || dJump == 1)
        {
            audio.PlayOneShot(jump);
            jumpSpeed = 15f;
            dJump++;
            moveDirection.y += jumpSpeed;
            jumpSpeed = 0f;
        }
    }

    //Handle Collisions
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // When Collided with battery call the function inside battery  
        //This is to prevent a bug where the player tends to pick up a batteries 2 times instead of one
        if (hit.transform.tag == "Battery")
        {
            hit.transform.SendMessageUpwards("Collect");
        }

        // Get stunned if hit by cats or the paintbrush
        if (hit.transform.tag == "PaintBrush")
        {
            audio.PlayOneShot(hit_brush);
            isStunned = true;
            StartCoroutine(DeStun());
        }

        // Get stunned if hit by paintbrush
        if (hit.transform.tag == "AI")
        {
            hit.transform.SendMessageUpwards("OnPlayerHit");
            audio.PlayOneShot(hit_cat);
            isStunned = true;
            StartCoroutine(DeStun());
        }

    }
 
    // Coroutine which acts as the stun timer
    IEnumerator DeStun()
    {
        yield return new WaitForSeconds(2);
        isStunned = false;
    }
}
