using UnityEngine;
using UnityEngine.InputSystem; ///add the input system because we're using it

public class BucketScript : MonoBehaviour
{
    ///vars
    


    ///teleporter prefab gameobjects
    [SerializeField] private GameObject lmbBlock;
    [SerializeField] private GameObject rmbFirstPortal;
    [SerializeField] private GameObject rmbSecondPortal;   
    [SerializeField] private GameObject instantiatePoint;   

    private GameObject lmbPuddle1;   
    private GameObject lmbPuddle2;   
    private GameObject rmbPuddle1;   
    private GameObject rmbPuddle2;   

    ///variables that keep track of the number of teleport puddles placed
    public int rmbPuddlesPlaced;


    ///track where the Out portal is, so we can make sure 
    /// you don't place the puddles too close together
    ///Store the Game Object you're teleporting to
    [SerializeField] private GameObject teleportTarget;
    ///Make a vector3 that stores the teleportTarget's
    /// transform.position
    private Vector3 targPos;
    [SerializeField]private float distanceToOutPuddle;


    ///other scripts
    [SerializeField]private testPlayerMove testPlayerMove;

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

        ///If we have placed the Out puddle, store its location 
        /// in targPos
        if(rmbPuddlesPlaced == 2)
        {
        teleportTarget = GameObject.Find("PuddleLMBOut(Clone)"); 
        targPos = teleportTarget.transform.position; //store the transform.position of the out location

        }




        if((useBasicToolInput.WasPressedThisFrame()) && (testPlayerMove.manaBucketCurrent > 0)) //if LMB pressed, 
        {
            //Create the appropriate block
            //the prefab you dragged into the lmbBlock variable
            //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Object.Instantiate.html
            Instantiate(lmbBlock, transform.position, transform.rotation);

            ///and deduct the bucketmana you spent
            testPlayerMove.manaBucketCurrent--;
        }

        ///then do basically the exact same thing for the Advanced tool
        if((useAdvancedToolInput.WasPressedThisFrame()) && (testPlayerMove.manaBucketCurrent > 0)) //if RMB pressed, 
        {
            ///Switch checks how many puddles have been placed 
            /// and does the appropriate thing

            switch(rmbPuddlesPlaced) 
            {
                case 0: ///0 puddles? Place the In puddle
                //Create the appropriate puddle
                //the prefab you dragged into the lmbInPuddle variable
                //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Object.Instantiate.html
                Instantiate(rmbFirstPortal, instantiatePoint.transform.position, transform.rotation);
                rmbPuddlesPlaced++; //add 1 to the number of puddles placed
                break;

                case 1:
                ///First check to make sure you're not too close to the Out Puddle
                //distanceToOutPuddle = Vector3.Distance(targPos, transform.position);                
                
                //if(distanceToOutPuddle > 2)
                //{
                 ///1 puddle? Place the Out puddle
                //Create the appropriate puddle
                //the prefab you dragged into the lmbInPuddle variable
                //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Object.Instantiate.html
                Instantiate(rmbSecondPortal, instantiatePoint.transform.position, transform.rotation);
                rmbPuddlesPlaced++; //add 1 to the number of puddles placed
                ///and deduct the bucketmana you spent                
                testPlayerMove.manaBucketCurrent--;    
                //}
                //else
                //{
                    ///tell the player they're trying to place
                    /// too close to the Out Puddle
                //}
            
                break;

                case 2: ///2 puddles already? Find the two existing puddles, 
                //destroy them, and place the new In puddle

                ///Find the two existing puddles
                /// https://docs.unity3d.com/6000.4/Documentation/ScriptReference/GameObject.Find.html
                rmbPuddle1 = GameObject.Find("PuddleLMBOut(Clone)");
                rmbPuddle2 = GameObject.Find("PuddleLMBIn(Clone)");
                /// Destroy those existing puddles
                /// https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Object.Destroy.html
                Destroy(rmbPuddle1);
                Destroy(rmbPuddle2);

                //Create the appropriate puddle
                //the prefab you dragged into the lmbInPuddle variable
                //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Object.Instantiate.html
                Instantiate(rmbFirstPortal, transform.position, transform.rotation);
                rmbPuddlesPlaced = 1; //set the number of existing puddles to 1 
                break;
            }
        }
    }
}
