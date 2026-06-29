using UnityEngine;

public class TestGroundCheck : MonoBehaviour
{

    //vars
    ///other scripts
    [SerializeField]private testPlayerMove testPlayerMove;



    // Update is called once per frame
    void Update()
    {




        
    }
    //In the Inspector, we have the Sphere Collider's layer overrides
    //set to only include the Ground layer,
    //so objects with colliders on that layer are the only 
    //objects this will detect

    private void OnTriggerStay(Collider other)
    {

           testPlayerMove.isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
           testPlayerMove.isGrounded = false;
    }
}
