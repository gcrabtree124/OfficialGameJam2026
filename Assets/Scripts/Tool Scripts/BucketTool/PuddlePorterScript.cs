using UnityEngine;

public class PuddlePorterScript : MonoBehaviour
{
    ///vars
    /// Get number of puddles placed from BucketScript, 
    /// so we only look for the teleport target when it's been created
    /// otherwise there's an annoying error
    ///other scripts
    [SerializeField] private BucketScript bucketScript;
    [SerializeField] //serialized for debugging
    private int puddleCount;



    ///teleporter prefab gameobjects


    ///Store the Game Object that hits this object's Collider
    ///Serialized mostly for debugging purposes
    [SerializeField] private GameObject objToTeleport;

    ///Store the Game Object you're teleporting to
    [SerializeField] private GameObject teleportTarget;

    ///Make a vector3 that stores the teleportTarget's
    /// transform.position
    private Vector3 targPos;


    // Update is called once per frame
    void Update()
    {
        ///find the exit portal GameObject 
        //if there is one and store it as teleportTarget
        //Only returns active game objects
        //It's usually a bad idea to use Find in update, because it's resource-intensive,
        //but used in this limited manner in a scene that doesn't have tons and tons of
        //game objects in it shouldn't be too bad
        //still, be careful doing this
        //relevant documentation:
        ///https://docs.unity3d.com/6000.4/Documentation/ScriptReference/GameObject.Find.html


        /// Only check for the exit portal if it's been placed
        //if (bucketScript.rmbPuddlesPlaced == 2)
        //{
        if (bucketScript != null)
        {
            puddleCount = bucketScript.rmbPuddlesPlaced;
        }

        teleportTarget = GameObject.Find("PuddleLMBOut(Clone)");
        if (teleportTarget != null)
        {
            targPos = teleportTarget.transform.position; //store the transform.position of the out location
        }
        else if (objToTeleport != null)
        {
            targPos = objToTeleport.transform.position;
        }

        //}


    }

    private void OnTriggerEnter(Collider other)
    {
        if ((teleportTarget != null) && (other.tag == "Player") && (teleportTarget.name == "PuddleLMBOut(Clone)"))
            //&& (puddleCount == 2))
        {
            //Get the other collider's GameObject and store it as ObjToTeleport
            objToTeleport = other.gameObject;

            //Teleport the object you just collided with to the out location
            objToTeleport.transform.position = targPos;
        }
    }
}
