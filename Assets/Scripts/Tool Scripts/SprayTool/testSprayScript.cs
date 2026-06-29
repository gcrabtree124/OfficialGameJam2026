using UnityEngine;
using UnityEngine.InputSystem; 

public class testSprayScript : MonoBehaviour
{
    //vars
    //Game Objects
    [SerializeField]private GameObject playerObj; ///need playerObj to test distance
    [SerializeField]private GameObject dashObj; //turn on if player dashes
    [SerializeField]private GameObject doubleJumpObj; //turn on if player dashes
    [SerializeField]private Rigidbody playerRb; //Need to get the player's rigidbody
                                                //to blow the player around

    ///other scripts
    [SerializeField]private testPlayerMove testPlayerMove;

    
    //How much force the wind jump is
    [SerializeField]private float doubleJumpForce;
    [SerializeField]private float dashForce;

    ///input stuff
    /// 
    //The Action map is located in Project Window>Assets, and is called InputSystem_Actions

    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here
                                                            //in the Inspector

    //input actions, doubleclick InputSystem_Actions to see these
    //or create or edit these
    private InputAction useBasicToolInput; ///Activate the basic tool with left mouse button
    private InputAction useAdvancedToolInput; ///Activate the advanced tool with right mouse button

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable(); //Get the Player actionmap
        //If the bucket is enabled, enable the inputs
        useBasicToolInput = InputSystem.actions.FindAction("BasicTool");//connect the input variable
        useAdvancedToolInput = InputSystem.actions.FindAction("AdvTool");//with the action name

    }

    private void OnDisable()
    {

    }



    // Update is called once per frame
    void Update()
    {
        ///if they press LMB and the green lantern isn't out and you have spray mana
        if((useBasicToolInput.WasPressedThisFrame()) && (doubleJumpObj.activeInHierarchy == false) && (testPlayerMove.manaSprayCurrent > 0))
        {
            doubleJumpObj.SetActive(true); 
            testPlayerMove.manaSprayCurrent--;

        }

        ///if they press RMB and the green lantern isn't out and you have spray mana
        if((useAdvancedToolInput.WasPressedThisFrame()) && (dashObj.activeInHierarchy == false) && (testPlayerMove.manaSprayCurrent > 0))
        {
            
            dashObj.SetActive(true); 
            testPlayerMove.manaSprayCurrent--;            
        }   
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        ///if they press LMB and the green lantern isn't out
        if(doubleJumpObj.activeInHierarchy == true)
        {
            playerRb.AddRelativeForce(0, doubleJumpForce, 0, ForceMode.Impulse); ///Apply upward force
            doubleJumpObj.SetActive(false); 
        }

        ///if they press RMB and the green lantern isn't out
        if(dashObj.activeInHierarchy == true)
        {
            playerRb.AddRelativeForce(0, 0, dashForce, ForceMode.Impulse); ///Apply forward force
            dashObj.SetActive(false); 
        }   
    }
}
