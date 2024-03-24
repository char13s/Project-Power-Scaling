/**
	Copyright (C) 2019 NyangireWorks. All Rights Reserved.
 */

using UnityEngine;
using ThirdPersonCameraWithLockOn;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class SimpleCharacterController : MonoBehaviour {

	public ThirdPersonCamera camScript;


    private Animator animator;
	private float h;
	private float v;


    private Camera cam = null;
	private float angle;
	private CharacterController cc;
	private float animSpeed = 0;
	private float timeToSprint = 0;

	[Tooltip("Speed of movement")]
	public float moveSpeed = 8f;

	private Vector3 moveDirection;


	[Tooltip("Duration of the air time during a jump")]
	public float jumpDuration = 1f;

	private bool grounded = false;
	//private bool doubleJump = false; 
	[Tooltip("Transform that marks the origin of the ground check sphere, used to deremine if the character is on ground")]
	public Transform groundCheck;
	[Tooltip("radius of the ground check sphere")]
	public float groundRadius = 0.5f;
	[Tooltip("Layers considered walkable")]
	public LayerMask whatIsGround;
	private bool colliderGrounded;
	
	[Tooltip("Is the player currently jumping?")]
	public bool jumping;
	Vector3 verticalPos;
	//public float jumpHeight = 3;
	[Tooltip("Jump Speed")]
	public float jumpSpeed = 20;
	
	private bool falling;

	private float jumpVelocity;
	private float previousY;
	private float currentY;
	//private float vSpeed; 
	private bool moving;

	//private Rigidbody rb;

	private Vector3 moveLock;
	private float gravitySpeed = 0.0f;


#if ENABLE_INPUT_SYSTEM

    Vector2 moveInput;
    float jumpInput;
    CameraInputActions cameraIA;
    Vector2 inputVelocity = Vector2.zero;


    private void ReadRawInputFromInputSystem()
    {

        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        if (kb != null)
        {
            Vector2 newInput = Vector2.zero;

            if (kb.wKey.isPressed)
                newInput.y += 1.0f;
            if (kb.sKey.isPressed)
                newInput.y -= 1.0f;
            if (kb.dKey.isPressed)
                newInput.x += 1.0f;
            if (kb.aKey.isPressed)
                newInput.x -= 1.0f;

            moveInput = Vector2.SmoothDamp(moveInput, newInput, ref inputVelocity, 0.05f);

            if (kb.spaceKey.wasPressedThisFrame)
                jumpInput = 1.0f;
        }




        Gamepad gp = InputSystem.GetDevice<Gamepad>();
        if (gp != null)
        {
            Vector2 gamepadValue = gp.leftStick.ReadValue();
            if (gp.leftStick.IsActuated())
            {
                moveInput = gamepadValue;
            }

            if (gp.buttonSouth.wasPressedThisFrame)
                jumpInput = 1.0f;
        }


    }
#endif

    private bool UseNewInputSystem()
    {
        return camScript.CameraInputType == ThirdPersonCamera.InputType.InputSystem;
    }

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator> ();

		// this gets the camera, the camera is neceesary because movement input is relavite to how its oriented (forward is camera forward etc.)
		if (camScript != null)
		{
			cam = camScript.gameObject.GetComponent<Camera>();
		}
		else
		{
			cam = Camera.main;
		}

		cc = GetComponent<CharacterController> ();

		jumpVelocity = 0f;
		jumping = false;

		currentY = transform.position.y;

    }



    // Update is called once per frame
    void Update () {

        if(UseNewInputSystem())
        {
#if ENABLE_INPUT_SYSTEM

            ReadRawInputFromInputSystem();
            h = moveInput.x;
            v = moveInput.y;
#endif
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
        
        

        animSpeed = Mathf.Max (Mathf.Abs (h), Mathf.Abs (v));

		animator.SetFloat ("speed", animSpeed);

		moveDirection.Set (0, 0, 0);

		previousY = currentY;
		currentY = transform.position.y;

		//vSpeed = !jumping ? 0 : currentY - previousY;
		animator.SetFloat ("vSpeed", cc.velocity.y);

		// set the direction of movement using the joystick/keyboard input and the camera direction
		if (h != 0 || v != 0) {
			moving = true;


			animator.SetFloat ("speed", animSpeed);
            //handAnimator.SetFloat("speed", animSpeed);

			moveDirection.Set (h, 0, v);
			moveDirection.Normalize ();
            moveDirection *= Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));

            if (animSpeed < 0.1)
            {
                moveDirection = Vector3.zero;
            }

			// the true move direction is the cameras y roation euler rotated by the move input euler!
			// this makes the movement input be relative to the camera Y rotation 
			moveDirection = Quaternion.Euler (0, cam.transform.rotation.eulerAngles.y, 0) * moveDirection;

			moveDirection *= Time.deltaTime * moveSpeed;


		} else {
			moving = false;
		}
		animator.SetBool ("Moving", moving);

        cc.center = new Vector3(cc.center.x, cc.center.y, animator.GetFloat("ColliderZ"));


		if (jumping) {


			//cc.height = animator.GetFloat("ColliderShrink");
			//cc.center = new Vector3(cc.center.x, animator.GetFloat("ColliderY"),cc.center.z);

			jumpVelocity = jumpSpeed * Time.deltaTime;


			if ((currentY - previousY) <= 0 ){

				falling = true;


			}


//			if((currentY - previousY) >= 0 && falling){
//				jumpVelocity = 0;
//				jumping = false;
//				falling = false;
//			}
		}

		animator.SetBool ("Jump", false);
        //handAnimator.SetBool("Jump", false);

        if (grounded && !jumping)
        {
            if (!UseNewInputSystem() && Input.GetButtonDown("Jump"))

            {
                animator.SetBool("Jump", true);
                //handAnimator.SetBool("Jump", true);
            }
            else
            {
#if ENABLE_INPUT_SYSTEM

                if(jumpInput > 0.5f)
                {
                    animator.SetBool("Jump", true);
                    jumpInput = 0f;
                }
#endif
            }
        }
            

       

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundMovement") && animSpeed > 0.8){
			timeToSprint += Time.deltaTime;
		}else{
			timeToSprint = 0;
		}

		//where the character is facing
		transform.LookAt (transform.position + moveDirection); 
		

		Vector3 move = Vector3.zero;
		// allows jumping to have some horizonrtal control
		if(jumping)
		{
			move = moveDirection;
		}

		move.y = jumpVelocity;

		//simulate gravity acceleration -30 m/s
		if(cc.isGrounded == true)
		{
			//Debug.Log("cc grounded");
			gravitySpeed = 0.0f;
		}
		else{
			gravitySpeed += 20.0f * Time.deltaTime * Time.deltaTime;
			//Debug.Log(gravitySpeed);
		}


		move.y -= gravitySpeed;



		// falling or jumping doesnt use root motio so we have to move it manually
		cc.Move(move);

    }

	void FixedUpdate(){
		if(groundCheck == null)
		{
			Debug.LogError("ground check null");
			return;
		}
		grounded = Physics.OverlapSphere (groundCheck.position, groundRadius, whatIsGround).Length > 0;//whatIsGround if needed

		animator.SetBool ("Grounded", grounded);

		if (jumping && grounded && falling) {
			jumping = false;
			jumpVelocity = 0;
			falling = false;
		}
		if (camScript != null)
		{
			camScript.UpdateJumpingStatus(jumping);
		}

		//GetComponent<Rigidbody>().AddForce(Vector3.down * 30.0f, ForceMode.Acceleration);
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		if (grounded)
			Gizmos.color = Color.red;
		
		Gizmos.DrawSphere (groundCheck.position, groundRadius);
	}


	void Leap(){
		jumpVelocity = jumpSpeed * Time.deltaTime;
		jumping = true;
		currentY = transform.position.y;

		moveDirection.y = jumpVelocity;


		
		cc.Move(moveDirection);

		//Debug.Log("Leaped");
		if(camScript != null)
		{
			camScript.UpdateJumpingStatus(jumping);
		}

	}

	void OnCollisionEnter(){
		Debug.Log ("collided");

	}

    
}
