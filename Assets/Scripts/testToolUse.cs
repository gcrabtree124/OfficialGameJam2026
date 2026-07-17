using UnityEngine;
using UnityEngine.InputSystem; ///add the input system because we're using it

public class testToolUse : MonoBehaviour
{
    ///var
    /// Tool Game Objects to turn off and on
    [SerializeField] private GameObject noToolActive;
    [SerializeField] private GameObject bucketActive;
    [SerializeField] private GameObject sprayActive;
    [SerializeField] private GameObject lanternActive;
    [SerializeField]private GameObject freelookCam; //track this for switching movement modes 
    [SerializeField]private GameObject firstPersonCam; //track this for switching movement modes 
    [SerializeField]private GameObject followCam; //track this for switching movement modes   
    ///This stores objects that collide with the InteractionBubble
    /// The colliders on the children of an object also
    /// trigger OnTriggerEnter and OnCollisionEnter 
    /// on the parent GameObject
    /// This is one way to add multiple layers to one game object, but 
    /// we're not doing that here 
    [SerializeField] private GameObject objInInteractionCollider;
    //We will need to design levels so two interactable objects aren't close together
    //I think that's good policy anyway, really
    //Adjust the size of the InteractionBubble collider to adjust 
    // how far away you can interact with stuff

    ///MacGuffin Controller stuff
    [SerializeField] private GameObject switch1on;
    [SerializeField] private GameObject switch2on;
    [SerializeField] private GameObject floorFalls;

    [SerializeField] private TextAsset npcStory; //The story that plays when you interact with the npc

    //int for switch to track which tool player selected
     [SerializeField]   public static int whichActiveTool;

    public static int whichCam; //int for keeping track of which camera is active   


    ///bools to see if we have each tool
    [SerializeField]public bool lanternBasic; ///enable the basic mode when player picks up tool
    [SerializeField]public bool bucketBasic; ///vars are static so they keep their value when we load
    [SerializeField]public bool sprayBasic; 
                                    // We need a separate place to store these 3 vars as static vars
                                    //so they don't get destroyed every time we load a scene
                                    //we should be getting from the static location and setting these values
                                    //from the static location on update
                                    //have to think about how that should be done
                                    //these will be replaced with Shadow Integration Level
                                    //As long as SIL is above 1, player can use tool


    ///other scripts
    [SerializeField]private testPlayerMove testPlayerMove;
    [SerializeField]private MacGuffinScript macGuffinScript;    

    //Input Action stuff
    //The Action map is located in Project Window>Assets, and is called InputSystem_Actions

    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here
                                                            //in the Inspector

    //input actions, doubleclick InputSystem_Actions to see these
    //or create or edit these
    private InputAction interactInput; ///E key to interact/pick stuff up

    private InputAction switchToNoToolInput;  //1 key to stow tools
    private InputAction switchToLanternInput;  //2 key to switch to the lantern
    private InputAction switchToSprayInput;  //3 for spray
    private InputAction switchToBucketInput;  //4 for bucket

    private InputAction switchCamModes;  //Switch between camera modes

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {

        InputActions.FindActionMap("Player").Enable(); //Get the Player actionmap
        //If the PlayerToolSelector is enabled, enable the inputs
        switchToNoToolInput = InputSystem.actions.FindAction("NoToolSelect");//connect the input variable
        switchToLanternInput = InputSystem.actions.FindAction("LanternSelect");//with the action name
        switchToSprayInput = InputSystem.actions.FindAction("SpraySelect");//in the Action Map
        switchToBucketInput = InputSystem.actions.FindAction("BucketSelect");
        interactInput = InputSystem.actions.FindAction("Interact");
        switchCamModes = InputSystem.actions.FindAction("CamSelect");
    }

    private void OnDisable()
    {

    }



    // Update is called once per frame
    void Update()
    {
        InputSelectTool(); ///checks player input to see if they switch tools
        ToolSwitcher(); ///turns on the currently selected tool and off the others
        InteractInputChecker(); //Runs if the Interact button is pressed
        CamSwitcher(); ///switch between camera modes       



    }

    void InputSelectTool()
    {
        ///stow your tools so you can dash and glide
        if(switchToNoToolInput.WasPressedThisFrame())
        {
            whichActiveTool = 1;
        }
            //Switch to the lantern if player presses 2 and you have the lantern
        if((switchToLanternInput.WasPressedThisFrame()) && lanternBasic == true)
        {
            whichActiveTool = 2;
        }

        if((switchToSprayInput.WasPressedThisFrame()) && sprayBasic == true)
        {
            whichActiveTool = 3;
        }

        if((switchToBucketInput.WasPressedThisFrame()) && bucketBasic == true)
        {
            whichActiveTool = 4;
        }

    }

    void ToolSwitcher()//The function that keeps track of which tool we have open
    {
        switch(whichActiveTool)
        {
            case 1: ///if 1, set noTool active
            noToolActive.SetActive(true);        
            lanternActive.SetActive(false); //turn on the active tool
            sprayActive.SetActive(false); ///turn off all other tools
            bucketActive.SetActive(false); ///turn off all other conversations
            break;

            case 2: ///if 2 and bool
            if(lanternBasic == true) ///check to see if we have the lantern
            {
            noToolActive.SetActive(false);        
            lanternActive.SetActive(true); //turn on the active tool
            sprayActive.SetActive(false); ///turn off all other tools
            bucketActive.SetActive(false); ///turn off all other conversations
            }
            break;

            case 3: ///if 2 and bool
            if(sprayBasic == true) ///check to see if we have the spray
            {
            noToolActive.SetActive(false);        
            lanternActive.SetActive(false); //turn on the active tool
            sprayActive.SetActive(true); ///turn off all other tools
            bucketActive.SetActive(false); ///turn off all other conversations
            }
            break;

            case 4: ///if 2 and bool
            if(bucketBasic == true) ///check to see if we have the bucket
            {
            noToolActive.SetActive(false);        
            lanternActive.SetActive(false); //turn on the active tool
            sprayActive.SetActive(false); ///turn off all other tools
            bucketActive.SetActive(true); ///turn off all other conversations
            }
            break;
            
            default: ///if other, set noTool active
            noToolActive.SetActive(true);        
            lanternActive.SetActive(false); //turn on the active tool
            sprayActive.SetActive(false); ///turn off all other tools
            bucketActive.SetActive(false); ///turn off all other conversations
            break;
        }
    }

        // For the functions below,
        //Doing it this way works for a limited number of things, but will not scale well
        //if we are interacting with a lot of different things

        //we could just do a layer called "DialogueInteractable" and have all the other
        //weird interactions you can have be dialogue interactions
        //and run those through the dialogue engine


    ///When an object enters the interaction collider, 
    /// check to see if they have an interactable tag,
    /// and if so, store that gameobject as
    /// objInInteractionCollider, which is used in the
    /// InteractInputChecker
    /// Relevant documentation:
    ///  https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Collider.OnTriggerStay.html
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Interactable")
        {
        objInInteractionCollider = other.gameObject; 
        Debug.Log("objInInteractionCollider set to: " + objInInteractionCollider.name);
        }
    }
    
    //If the object leaves the interaction collider, 
    //reset objInInteractionCollider
    //to null.
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Interactable")
        {
        objInInteractionCollider = null;  
        Debug.Log("objInInteractionCollider reset to null");                      
        }
    }
    ///Make an OnTriggerExit to remove the object if the object isn't in range

    //Each update, check to see if InteractInput was pressed, and if so, 
    //compare the name of the game object stored in objInInteractionCollider 
    //to the GameObject names of the different tools
    ///there is probably a better way to do the below, but this does work
    /// without too much trouble as long as you're not interacting with a ton of items
    /// ...which we might be
    private void InteractInputChecker()
    {

        if(interactInput.WasPressedThisFrame())
        {
            Debug.Log( objInInteractionCollider.name);
            if(objInInteractionCollider.name == "Lantern") //check the name of the game object
            {
            objInInteractionCollider.gameObject.SetActive(false); //disable the game object the collider is attached to
            lanternBasic = true;   //turn the boolean on that we check to see if we have the tool
            objInInteractionCollider = null; ///reset the object detected to null                                
            }

            if(objInInteractionCollider.name == "Bucket")
            {
            objInInteractionCollider.gameObject.SetActive(false); //disable the game object the collider is attached to
            bucketBasic = true;               
            objInInteractionCollider = null;       
            }

            if(objInInteractionCollider.name == "Spray")
            {
            objInInteractionCollider.gameObject.SetActive(false); //disable the game object the collider is attached to
            sprayBasic = true;               
            objInInteractionCollider = null;       
            }

            if((objInInteractionCollider.name == "FillStation") || (objInInteractionCollider.name == "FillStation2"))
            {
                
                testPlayerMove.staminaCurrent = testPlayerMove.staminaMax;
                testPlayerMove.manaLanternCurrent = testPlayerMove.manaSILLanternMax;
                testPlayerMove.manaSprayCurrent = testPlayerMove.manaSILSprayMax;
                testPlayerMove.manaBucketCurrent = testPlayerMove.manaSILBucketMax;
            }

            if(objInInteractionCollider.name == "Switch1Off")
            {
            objInInteractionCollider.gameObject.SetActive(false); //disable the game object the collider is attached to
            switch1on.SetActive(true); //turn on the ON game object
            macGuffinScript.s1Timer = 10;
            }

            if(objInInteractionCollider.name == "Switch2Off")
            {
            objInInteractionCollider.gameObject.SetActive(false); //disable the game object the collider is attached to
            switch2on.SetActive(true); //turn on the ON game object
            macGuffinScript.s2Timer = 10;
            }

            if(objInInteractionCollider.name == "MacGuffin")
            {
            objInInteractionCollider.gameObject.SetActive(false); //disable the game object the collider is attached to
            switch2on.SetActive(false); //floor disappears
            }

            // if (!interactInput.WasPressedThisFrame())
            //     return;

            // if (DialogueManager.Instance.DialogueActive)
            // {
            //     DialogueManager.Instance.ContinueStory();
            //     return;
            // }

            // if (objInInteractionCollider == null)
            //     return;

            objInInteractionCollider.GetComponent<DialogueTrigger>()?.TriggerDialogue();
        }
    }

    void CamSwitcher()
    {
        if(switchCamModes.WasPressedThisFrame())
        {
            if(whichCam >= 1)
            {
                whichCam = 0;
            }
            else
            {
                whichCam++;
            }

        }
        switch(whichCam)
        {
            case 0: ///if 1, set noTool active
            freelookCam.SetActive(false); 
            followCam.SetActive(true);                 
            firstPersonCam.SetActive(false); 
            break;

            case 1: ///if 1, set noTool active
            freelookCam.SetActive(false); 
            followCam.SetActive(false);                 
            firstPersonCam.SetActive(true); 
            break;

            case 2: ///if 1, set noTool active
            freelookCam.SetActive(false); 
            followCam.SetActive(false);                 
            firstPersonCam.SetActive(false); 
            break;

            default: ///if 1, set noTool active
            freelookCam.SetActive(false); 
            followCam.SetActive(true);                 
            firstPersonCam.SetActive(false); 
            break;
        }        
    }
}
