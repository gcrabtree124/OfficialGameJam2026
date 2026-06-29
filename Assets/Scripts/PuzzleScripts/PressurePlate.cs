using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    //vars



    [SerializeField] private GameObject objToVanish;  //Object to vanish
    [SerializeField] private GameObject plateOn;  //
    [SerializeField] private GameObject plateOff;  //Switch between the on or off plates


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }



    private void OnTriggerStay(Collider other)
    {
        objToVanish.SetActive(false); 
        plateOff.SetActive(true); 
        plateOn.SetActive(false); 
    } 

    private void OnTriggerExit(Collider other)
    {
        objToVanish.SetActive(true); 
        plateOff.SetActive(false); 
        plateOn.SetActive(true); 
    }
}
