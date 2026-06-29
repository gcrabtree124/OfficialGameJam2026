using UnityEngine;
using UnityEngine.UI; ///fill amount is in UI

public class TestLanternBarFiller : MonoBehaviour
{
    ///other scripts
    [SerializeField]private testPlayerMove testPlayerMove;
    [SerializeField]Image fill;   


    // Update is called once per frame
    void Update()
    {
        ///Bar is filled based on percentage of resource
        fill.fillAmount = testPlayerMove.manaLanternCurrent / testPlayerMove.manaSILLanternMax;
    }
}
