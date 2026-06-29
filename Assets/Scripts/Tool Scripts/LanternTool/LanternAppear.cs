using UnityEngine;

public class LanternAppear : MonoBehaviour
{
    ///vars
    /// objects
    [SerializeField] private GameObject objToAppear;  //Object to appear
    [SerializeField]private GameObject playerObj; ///need playerObj to test distance
    [SerializeField]private GameObject redLightObj; //need this to check if the player has an active green lantern

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
    

        if((distanceToPlayer < 15) && (redLightObj.activeInHierarchy == true))
        {
            //set them to active if red lantern out within 10m
            //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/GameObject.SetActive.html
            objToAppear.SetActive(true);        
        }
        else
        {
            //otherwise keep them inactive
            objToAppear.SetActive(false);      
        }
        
    }
}
