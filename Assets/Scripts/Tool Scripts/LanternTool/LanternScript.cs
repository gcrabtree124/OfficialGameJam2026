using UnityEngine;
using UnityEngine.InputSystem; ///add the input system because we're using it

public class LanternScript : MonoBehaviour
{
    //vars
    //Game Objects
    [SerializeField]private GameObject violetLightObj; //The violet light that hides
    [SerializeField]private GameObject redLightObj;   //the red light that reveals


    ///other scripts
    [SerializeField]private testPlayerMove testPlayerMove;

    ///Lantern timers
    [SerializeField]private float violetLightTimer; 
    [SerializeField]private float redLightTimer; 
    [SerializeField]private float lanternMana; 
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
        //tracks lantern mana usage
        LanternTracker();

        ///4 conditions have to be satisfied. 
        /// 1. Press LMB
        /// 2. the RMB Violet Light is not active
        /// 3. The LMB red light is not already active
        /// 4. testPlayerMove.manaLanternCurrent has more than zero mana
        if(useBasicToolInput.IsPressed() && (violetLightObj.activeInHierarchy == false) && (redLightObj.activeInHierarchy == false) && (testPlayerMove.manaLanternCurrent > 0))
        {
            violetLightObj.SetActive(true); 
            redLightObj.SetActive(false);        
        }

        ///Release LMB to exit climb mode
        if(useBasicToolInput.WasReleasedThisFrame())
        {
            violetLightObj.SetActive(false); 
        }  


        ///Hold RMB to activate glide mode
        if((useAdvancedToolInput.IsPressed()) && (violetLightObj.activeInHierarchy == false) && (redLightObj.activeInHierarchy == false) && (testPlayerMove.manaLanternCurrent > 0))
        {
            redLightObj.SetActive(true); 
            violetLightObj.SetActive(false);     
        }   

        ///Release RMB to exit glide mode
        if(useAdvancedToolInput.WasReleasedThisFrame())
        {
            redLightObj.SetActive(false); 
        }

        if(testPlayerMove.manaLanternCurrent < 0)
        {
            redLightObj.SetActive(false); 
            violetLightObj.SetActive(false);                 
        }   
    }

    private void LanternTracker()
    {
        if((violetLightObj.activeInHierarchy == true) || (redLightObj.activeInHierarchy == true) && (testPlayerMove.manaLanternCurrent > 0) ) 
        {
            testPlayerMove.manaLanternCurrent -= Time.deltaTime;
        }
    }

}
