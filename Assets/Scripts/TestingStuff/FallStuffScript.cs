using UnityEngine;

public class FallStuffScript : MonoBehaviour
{

    ///vars
    [SerializeField]private GameObject playerObj; ///need playerObj to test distance
    [SerializeField]private GameObject thisObj; ///need playerObj to test distance
    [SerializeField]private float distanceToPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        ///calculate the distance to the player by getting the transform.position of this and the player
        //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Vector3.Distance.html
        distanceToPlayer = Vector3.Distance(playerObj.transform.position, transform.position);


        ///Check to see if the player is less than 10m away AND has the red lantern out
        /// https://docs.unity3d.com/6000.4/Documentation/ScriptReference/GameObject-activeInHierarchy.html
    

        if(distanceToPlayer < 5)
        {
                Destroy(thisObj);
  
        }

    }
}
