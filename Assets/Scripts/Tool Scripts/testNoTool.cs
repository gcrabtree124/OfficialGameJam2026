using UnityEngine;
using UnityEngine.InputSystem; ///add the input system because we're using it

public class testNoTool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //vars
    //Game Objects
    [SerializeField]private GameObject glideObj; //need this to check if the player has an active green lantern
    [SerializeField]private GameObject climbObj; //need this to check if the player has an active green lantern





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
        ///Hold LMB to activate climb mode
        if(useBasicToolInput.IsPressed())
        {
            climbObj.SetActive(true); 
        }

        ///Release LMB to exit climb mode
        if(useBasicToolInput.WasReleasedThisFrame())
        {
            climbObj.SetActive(false); 
        }   

        ///Hold RMB to activate glide mode
        if(useAdvancedToolInput.IsPressed())
        {
            glideObj.SetActive(true); 
        }   

        ///Release RMB to exit glide mode
        if(useAdvancedToolInput.WasReleasedThisFrame())
        {
            glideObj.SetActive(false); 
        }   


    }


}
