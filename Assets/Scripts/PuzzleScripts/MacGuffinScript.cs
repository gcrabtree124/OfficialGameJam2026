using UnityEngine;

public class MacGuffinScript : MonoBehaviour
{
    ///vars
    /// 
    /// Game objects
    [SerializeField] private GameObject switch1off;
    [SerializeField] private GameObject switch1on;
    [SerializeField] private GameObject switch2off;
    [SerializeField] private GameObject switch2on;
    [SerializeField] private GameObject macGuffinWindow;
    [SerializeField] private GameObject macGuffinWindowUp;
    [SerializeField] private GameObject floorFalls;
    [SerializeField] private GameObject macGuffin;

    ////switch timers
    [SerializeField] public float s1Timer;   
    [SerializeField] public float s2Timer;   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Switch1Timer(); ///resets the switches after a period of time
        Switch2Timer();  
        MacGuffinWindow();  //raises the MacGuffin window if both switches thrown

        if(macGuffin.activeInHierarchy == false) //if the MacGuffin is turned off
        {

            floorFalls.SetActive(false); //the floor falls out from under the player
        }
 
    }


    private void Switch1Timer()
    {
        if(switch1on.activeInHierarchy == true)
        {
            if(s1Timer < 0)
            {
                switch1on.SetActive(false); 
                switch1off.SetActive(true); 
            }
            else
            {
                s1Timer = s1Timer - Time.deltaTime;
            }
        }
    }

    private void Switch2Timer()
    {
        if(switch2on.activeInHierarchy == true)
        {
            if(s2Timer < 0)
            {
                switch2on.SetActive(false); 
                switch2off.SetActive(true); 
            }
            else
            {
                s2Timer = s2Timer - Time.deltaTime;
            }
        }
    }

    private void MacGuffinWindow()
    {
        if((switch2on.activeInHierarchy == true) && (switch1on.activeInHierarchy == true))
        {
                macGuffinWindow.SetActive(false); 
                macGuffinWindowUp.SetActive(true); 
        }
        else
        {
                macGuffinWindow.SetActive(true); 
                macGuffinWindowUp.SetActive(false); 
        }
    }
}
