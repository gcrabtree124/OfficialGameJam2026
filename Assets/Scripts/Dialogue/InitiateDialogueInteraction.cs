using UnityEngine;
using UnityEngine.InputSystem;

public class InitiateDialogueInteraction : MonoBehaviour
{
    
    ///This stores objects that collide with the InteractionBubble
    /// The colliders on the children of an object also
    /// trigger OnTriggerEnter and OnCollisionEnter 
    /// on the parent GameObject
    /// This is one way to add multiple layers to one game object, but 
    /// we're not doing that here 
    
    private GameObject objInInteractionCollider;
    //We will need to design levels so two interactable objects aren't close together
    //I think that's good policy anyway, really
    //Adjust the size of the InteractionBubble collider to adjust 
    // how far away you can interact with stuff

    
    //Input Action stuff
    //The Action map is located in Project Window>Assets, and is called InputSystem_Actions

    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here
                                                            //in the Inspector

    //input actions, doubleclick InputSystem_Actions to see these
    //or create or edit these
    private InputAction interactInput; ///E key to interact/pick stuff up

    void Start()
    {
        
    }

    private void OnEnable()
    {

        InputActions.FindActionMap("Player").Enable(); //Get the Player actionmap
        //If the PlayerToolSelector is enabled, enable the inputs
       
        interactInput = InputSystem.actions.FindAction("Interact");
    }

    private void OnDisable()
    {

    }



    // Update is called once per frame
    void Update()
    {
        InteractInputChecker(); //Runs if the Interact button is pressed



    }


     

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

            objInInteractionCollider.GetComponent<DialogueTrigger>()?.TriggerDialogue();
        }
    }
}
