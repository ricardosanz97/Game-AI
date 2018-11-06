using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {

    public float walkSpeed = 2f;
    public float runSpeed = 8f;
    public float crouchSpeed = 1f;
    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;
    public float gravity = -12f;
    public float jumpHeight = 1f;
    public float timeToJump = 0.7f;
    public GameObject targetObjectRef;
    [Range(0,1)]
    public float airControlPercentage;
    public bool inCorrectWall;
    
    
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float currentSpeed;
    private Animator playerAnimator;
    private float walkPercentage = 0.5f;
    private vThirdPersonCamera cameraRef;
    private CharacterController controller;
    private float velocityY;
    private bool jumping;
    private float currentTime = 2f;
    private bool easterEggDone;

    public Vector3 velocity;

	private AudioSource audio;
	public AudioClip salto;
	public AudioClip muerte;
	public AudioClip walk;
	public AudioClip run;

    // Use this for initialization
    void Start () {

		audio = GetComponent<AudioSource> ();

        playerAnimator = GetComponent<Animator>();
        cameraRef = FindObjectOfType<vThirdPersonCamera>();
        controller = GetComponent<CharacterController>();
        if (targetObjectRef == null)
        {
            Debug.Log("No hay arrastrado ningún target");
        }
	}
	
    void LateUpdate()
    {
        CameraInput();
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (GameManager.I.playerAlive)
        {
            PlayerControl();
            EasterEggEnabled();
        }
        else
        {
            cameraRef.GetComponent<vThirdPersonCamera>().enabled = false;
        }
        //isDead();
    }
	void Update () {

        SoundsController();

	}


    void PlayerControl()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if(TimeToJumpElapsed())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                playerAnimator.SetTrigger("jump");
                currentTime = 0f;
				audio.clip = salto;
				audio.Play ();
            }
        }

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraRef.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        bool crouching = Input.GetKey(KeyCode.C);

        if (!crouching)
        {
            playerAnimator.SetBool("crouch", false);
            float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
            float animationSpeedPercentage = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * walkPercentage);
            playerAnimator.SetFloat("speedPercentage", animationSpeedPercentage, speedSmoothTime, Time.deltaTime);
        }
        else
        {
            playerAnimator.SetBool("crouch", true);
            float targetSpeed = crouchSpeed * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
            float animationCrouchPercentage = currentSpeed / crouchSpeed;
            playerAnimator.SetFloat("crouchPercentage", animationCrouchPercentage, speedSmoothTime, Time.deltaTime);
        }
        velocityY += Time.deltaTime * gravity;

        velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
       
    }

    void CameraInput()
    {
        var y = Input.GetAxis("Mouse Y");
        var x = Input.GetAxis("Mouse X");

        cameraRef.RotateCamera(x,y);
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercentage == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercentage;
    }

    bool TimeToJumpElapsed()
    {
        currentTime += Time.deltaTime;
        return (currentTime >= timeToJump);
    }

    public void SetDeadAnimatorParamenter()
    {
        if (!GameManager.I.playerAlive)
        {
            Debug.Log("muerto");
			audio.clip = muerte;
			audio.Play ();
            playerAnimator.SetBool("death", true);
        }
        else
        {
            Debug.Log("vivo");
            playerAnimator.SetBool("death", false);
        }
    }

    void SoundsController()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 2f && controller.velocity.magnitude < 7f && audio.isPlaying == false)
        {
            audio.Stop();
            audio.volume = Random.Range(0.8f, 1f);
            audio.pitch = Random.Range(0.8f, 1.1f);
            audio.clip = walk;
            audio.Play();
        }
        else if (controller.isGrounded && controller.velocity.magnitude > 6.5f && audio.isPlaying == false)
        {
            audio.Stop();
            audio.volume = Random.Range(0.8f, 1f);
            audio.pitch = Random.Range(0.8f, 1.1f);
            audio.clip = run;
            audio.Play();

        }
    }

    void EasterEggEnabled()
    {
        if (inCorrectWall)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!easterEggDone)
                {
                    easterEggDone = true;
                    playerAnimator.SetTrigger("salute");
                    GetComponent<PlayerHealth>().playerHealth = GetComponent<PlayerHealth>().maxPlayerHealth;
                    GetComponent<PlayerHealth>().playerHealthSlider.value = GetComponent<PlayerHealth>().playerHealth;
                }
            }
        }
    }
}
