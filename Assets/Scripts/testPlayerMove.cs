using UnityEngine;
using UnityEngine.InputSystem;

public class testPlayerMove : MonoBehaviour
{
    ///vars
    /// 
    /// 
    /// 
    ///Input Stuff 
    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here

    //Input Actions
    private InputAction moveInput;
    private InputAction turnInput;
    private InputAction jumpInput;


    //tool objects
    [SerializeField] private GameObject glideObj; //If this is active, player glides
                                                  //with zero downward force
    [SerializeField] private GameObject dashObj; //If this is active, player moves very fast   

    [SerializeField] private GameObject violetLightObj; //If this is active, player moves very fast   
    [SerializeField] private GameObject redLightObj; //If this is active, player moves very fast   

    [SerializeField] private GameObject climbObj; //If this is active, player moves very fast      
    [SerializeField] private GameObject freelookCam; //track this for switching movement modes 

    [SerializeField] private GameObject firstPersonCam; //track this for switching movement modes 
    [SerializeField] private GameObject followCam; //track this for switching movement modes
    [SerializeField] private GameObject groundCheckObj; //track this for switching movement modes

    ///quat4cam, messing around with the followcam
    Quaternion camQuatRotation; ///ignore, hopefully won't use




    /// Player movement force variables
    public float playerWalkSpeed;
    public float playerDashSpeed;
    public float playerSpeed;
    public float playerTurnSpeed;
    public float fallingDownwardForce; //downward force if falling
    public float jumpingDownwardForce; //downward force if jumping
    public float glidingDownwardForce;//downward force if gliding
    public float climbingDownwardForce;    //downward force if climbing
    public float playerDownwardForce;///this is the actual force acting on the player
    public float jumpForce;
    private Vector2 groundMoveValue; ///how much the player moves forward and back
    private Vector2 turnMoveValue; ///how much the player rotates looking
    [SerializeField] private bool isJumping; //tells the physics engine we're jumping this frame
    private Vector3 rayOrigin;



    //Stamina stuff and resource stuff
    [SerializeField] public float staminaCurrent; //current stamina
    [SerializeField] public static float staminaMax; //max stamina, static so it persists between scenes
    [SerializeField] public float manaLanternCurrent; //current stamina
    [SerializeField] public static float manaSILLanternMax; //max stamina
    [SerializeField] public float manaSprayCurrent; //current stamina
    [SerializeField] public static float manaSILSprayMax; //max stamina
    [SerializeField] public float manaBucketCurrent; //current stamina
    [SerializeField] public static float manaSILBucketMax; //max stamina


    //Rigidbody
    [SerializeField] private Rigidbody rb; //drag the PlayerObj rigidbody into this field

    ///Check to see if you're touching the ground
    /// isGrounded is set by the GroundCheckobj, which is a child of the PlayerObj
    /// that has a short script attached to check to see if its collider is touching a collider
    /// in the Grounded layer
    [SerializeField] public bool isGrounded;  //are they touching the ground?


    [SerializeField] private bool ableToClimb; //Can the horizontal climb ray touch a wall?    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ///Initial values for static vars, will only be 0 when the game initially loads
        /// after that they will always be higher, so this should
        /// only happen once at the very beginning of the game, 
        /// not every time the level loads
        if (staminaMax == 0)
        {
            staminaMax = 300;
        }

        if (manaSILLanternMax == 0)
        {
            manaSILLanternMax = 300;
        }

        if (manaSILSprayMax == 0)
        {
            manaSILSprayMax = 30;
        }

        if (manaSILBucketMax == 0)
        {
            manaSILBucketMax = 30;
        }

        ///When scene loads, set current values to their max values
        staminaCurrent = staminaMax;
        manaLanternCurrent = manaSILLanternMax;
        manaSprayCurrent = manaSILSprayMax;
        manaBucketCurrent = manaSILBucketMax;
    }

    private void OnEnable()
    {
        //go into your InputActions asset and enable the
        //"Player" action map
        InputActions.FindActionMap("Player").Enable();
        moveInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
        turnInput = InputSystem.actions.FindAction("Look");


        ///Lock the Cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        //Disable the Dialogue actionmap when you're not in Dialogue
        InputActions.FindActionMap("Player").Disable();   //point the action variables to the correct actions in the actionmap

        ///Unock the Cursor from the center of the screen and make it visible
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;


    }

    // Update is called once per frame
    void Update()
    {
        //Stamina system - regen and keeping track of
        StaminaTracker();
        //Airtracker tracks jumping, falling, climbing, gliding modes of air travel
        AirTracker();
        //tracks how much lantern mana's been used

        //Read the value off the input and assign it to the groundMove vector2
        
        groundMoveValue = moveInput.ReadValue<Vector2>();
        turnMoveValue = turnInput.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {

        if (isJumping == true)
        {
            //add the jump force to the Y axis and don't add any force to x or z
            //because you're already getting those forces 
            rb.AddRelativeForce(0, jumpForce, 0, ForceMode.Impulse);
            isJumping = false;
        }

        rb.AddRelativeForce(groundMoveValue.x * playerSpeed, playerDownwardForce, groundMoveValue.y * playerSpeed);
        rb.AddRelativeTorque(0, turnMoveValue.x * playerTurnSpeed, 0, ForceMode.VelocityChange);


        /*
        if (followCam.activeInHierarchy == true) ///check which camera's on
        {
            ///Add force to the rigidbody in the direction and amount 
            /// of groundMoveValue * the variable playerSpeed
            /// to X and Z, 
            /// while Y is dependent on if grounded, jumping, gliding, or climbing.
            if (groundMoveValue.x != 0 || groundMoveValue.y != 0 || playerDownwardForce != 0)
            {
                rb.AddRelativeForce(groundMoveValue.x * playerSpeed, playerDownwardForce, groundMoveValue.y * playerSpeed);
            }

            ///forcemode documentation below
            /// https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Rigidbody.AddForce.html
            /// There are 4 different types of forces you can apply, what each does is detailed in 
            /// the documentation
            ///for 3rdPersonHardlock cam
            rb.AddRelativeTorque(0, turnMoveValue.x * playerTurnSpeed, 0, ForceMode.VelocityChange);
        }

        //        if(freelookCam.activeInHierarchy == true) ///check which camera's on
        {


            ///for freelook cam, , 
            /// make player move 
            /// in the direction of the camera
            //            rb.AddRelativeForce(groundMoveValue.x * playerSpeed, playerDownwardForce, groundMoveValue.y * playerSpeed);

            ///I hate fucking Quats
            /// https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Quaternion.html
            /// https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Quaternion.LookRotation.html
            /// https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Quaternion.SetFromToRotation.html
            //          Vector3 movementDirection = new Vector3(freelookCam.transform.rotation.x, 0, freelookCam.transform.rotation.z);
            //            Vector3 camRotation = new Vector3(freelookCam.transform.rotation.x, 0, freelookCam.transform.rotation.z);


            //           if(groundMoveValue.x +groundMoveValue.y == 0)
            //           {
            //

            //            Vector3 camRotation = new Vector3(freelookCam.transform.rotation.x * -1, 0, freelookCam.transform.rotation.z * -1);
            //this one looks at an object that's opposite the camera
            //           Vector3 awayFromCamPos = new Vector3(transform.position.x - freelookCam.transform.position.x, transform.position.y, transform.position.z - freelookCam.transform.position.z);
            //           transform.eulerAngles = camRotation;
            //            Quaternion camQuatRotation = Quaternion.LookRotation(camRotation, Vector3.up);
            //            camQuatRotation = new Quaternion();
            //           camQuatRotation = Quaternion.SetFromToRotation(transform.position, awayFromCamPos);           
            //           transform.rotation = camQuatRotation * transform.rotation;
            //            transform.LookAt(awayFromCamPos);

            //           transform.LookAt(freelookCam.transform.position);
            //           }



        }

        if (firstPersonCam.activeInHierarchy == true) ///check which camera's on
        {
            ///Do 1st person movement
            /// make player move 
            /// in the direction the player points
            if (groundMoveValue.x > 0 || groundMoveValue.y > 0 || playerDownwardForce != 0)
            {
                rb.AddRelativeForce(groundMoveValue.x * playerSpeed, playerDownwardForce, groundMoveValue.y * playerSpeed);
            }

            Vector3 movementDirection = new Vector3(groundMoveValue.x, 0, groundMoveValue.y);
            transform.forward = movementDirection;
            rb.AddRelativeTorque(0, turnMoveValue.x * playerTurnSpeed, 0, ForceMode.VelocityChange);



        }*/

        //Jump Stuff

        //first, make a raycast and check if the character is on the ground
        //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Physics.Raycast.html
        //raycast documentation above
        //We shoot the ray from the transform.position of the object the script is attached to, 
        //in the direction of Vector3.down is straight down, 
        //the ray travels 1.2 meters, which should be just below our feet
        //and if it hits something in the groundMask layer, 
        //it returns True, otherwise returns False

        ///////Change this to a collider, we have our toe on the edge of stuff sometimes and count
        /// as in the air
        //        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.3f, groundMask);
        //Debug.DrawRay is just so we can see where the ray is in scene view, it has
        //no impact on anything else
        //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Debug.DrawRay.html
        //       Debug.DrawRay(transform.position, Vector3.down, Color.red, 1.3f);

        //We are casting from a lower point of origin so we can make slanted walls that are not climbable
        rayOrigin = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        ableToClimb = Physics.Raycast(rayOrigin, transform.TransformDirection(Vector3.forward), 1.5f);
        //Debug.DrawRay is just so we can see where the ray is in scene view, it has
        //no impact on anything else
        //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Debug.DrawRay.html

        Debug.DrawRay(rayOrigin, transform.TransformDirection(Vector3.forward), Color.green, 1.5f);
        //groundCheckObj
    }

    //Track what we're doing while we're in the air
    private void AirTracker()
    {
        //check for jump input
        if ((jumpInput.WasPressedThisFrame()) && (isGrounded == true))
        {
            if ((isJumping == false))
            {
                isJumping = true;
            }
        }

        //check for jump input
        if ((jumpInput.WasPressedThisFrame()) && (ableToClimb == true))
        {
            if (climbObj.activeInHierarchy == true)
            {
                //this is done this way so we don't deduct the stamina twice 
                //if they hold the jump button too long
                if ((isJumping == false) && (staminaCurrent > 10))
                {
                    isJumping = true;
                }
            }
        }

        if (isGrounded == false)
        {
            //check to see if gliding or climbing
            if ((glideObj.activeInHierarchy == false) && (climbObj.activeInHierarchy == false))//is the Glide obj active?
            {
                playerDownwardForce = fallingDownwardForce; //If in the air, force the player to the ground quicker
                                                            //rb.AddForce(0, playerDownwardForce, 0, ForceMode.Impulse); ///put this in physics
            }

            //check to see if gliding is true
            if ((glideObj.activeInHierarchy == true) && (climbObj.activeInHierarchy == false))//is the Glide obj active?
            {
                playerDownwardForce = glidingDownwardForce; //Set downwardForce to glide
            }

            //check to see if climbing is true
            if ((glideObj.activeInHierarchy == false) && (climbObj.activeInHierarchy == true))//is the Climb obj active?
            {
                playerDownwardForce = climbingDownwardForce; //Set downwardForce to glide
            }
        }
        else
        {
            playerDownwardForce = 0; //Don't need downward force if we're on the ground
        }
    }




    private void StaminaTracker()
    {
        if (staminaCurrent < staminaMax) //if staminaCurr is less than staminaMax, add stamina
        {
            staminaCurrent += Time.deltaTime * 3;
        }

        if ((climbObj.activeInHierarchy == true) && (isGrounded == false)) //if staminaCurr is less than staminaMax, add stamina
        {
            staminaCurrent -= Time.deltaTime * 6;
        }

        if ((glideObj.activeInHierarchy == true) && (isGrounded == false)) //if staminaCurr is less than staminaMax, add stamina
        {
            staminaCurrent -= Time.deltaTime * 6;
        }
        if (staminaCurrent < 0)
        {
            staminaCurrent = 0;
        }

    }

    ///Functions that can be called to increase max stamina, lantern, spray, and bucket mana
    /// Call this by creating 
    /// 
    /// 
    /// 
    ///             testPlayerMove.IncreaseMaxStamina();
    public void IncreaseMaxStamina()
    {
        staminaMax = staminaMax + 10;
    }



    //Stamina stuff and resource stuff
    //    [SerializeField]public float staminaCurrent; //current stamina
    //   [SerializeField]public static float staminaMax; //max stamina, static so it persists between scenes
    //   [SerializeField]public float manaLanternCurrent; //current stamina
    //   [SerializeField]public static float manaSILLanternMax; //max stamina
    //  [SerializeField]public int manaSprayCurrent; //current stamina
    //   [SerializeField]public static int manaSILSprayMax; //max stamina
    //   [SerializeField]public int manaBucketCurrent; //current stamina
    //   [SerializeField]public static int manaSILBucketMax; //max stamina



}
