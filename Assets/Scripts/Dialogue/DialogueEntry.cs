using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(string dialogue)
    {
        text.text = "TEST DIALOGUE";

        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            GetComponent<RectTransform>()
        );
    }
}