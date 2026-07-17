using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartStory(inkJSON);

    }
}