using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Story currentStory;

    private InputAction dialogueAdvanceInput; //E key to advance dialogue

    private InkExternalFunctions inkExternalFunctions; //functions to be called in the ink editor

    [SerializeField] private GameObject dialogueEntryPrefab;
    [SerializeField] private Transform dialogueContent;

    [SerializeField] private Transform choicesPanel;

    [SerializeField] private GameObject choiceButtonPrefab;

    [SerializeField] private GameObject dialogueCanvas;
    
    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here

    
    public bool DialogueActive
    {
        get { return currentStory != null; }
    }

    private void Awake()
    {
        Instance = this;
        dialogueCanvas.SetActive(false);
        inkExternalFunctions = new InkExternalFunctions();
        Debug.Log("inkExternalFunctions instance created: " + inkExternalFunctions);

    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Dialogue").Enable();
        dialogueAdvanceInput = InputSystem.actions.FindAction("DialogueAdvance");
    }
 
    private void Update()
    {
        DialogueInteractInputChecker();
    }
    public void StartStory(TextAsset inkJSON)
    {

        //set time scale to 0 to pause the game
        //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Time-timeScale.html

        //Time.timeScale = 0f; 
        InputActions.FindActionMap("Player").Disable(); 
        InputActions.FindActionMap("Dialogue").Enable(); 
        InputActions.FindActionMap("UI").Enable(); 
        dialogueCanvas.SetActive(true);
        currentStory = new Story(inkJSON.text);
        inkExternalFunctions.bindIncreaseSIL(currentStory, "increaseSIL");
        inkExternalFunctions.bindGetSIL(currentStory);
        

        ContinueStory();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ContinueStory()
    {
        if (currentStory == null){return;}

        ClearChoices();
        
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();
            if (text.Equals("") && !currentStory.canContinue)  //making sure there are no white space at the end of dialogue
            {
                EndDialogue();
                return;
            } 
            else
            {
                AddDialogueLine(text);
                Debug.Log(text);
            }
            
        }
        
        else if (currentStory.currentChoices.Count > 0)
        {
            

            DisplayChoices();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {

        Time.timeScale = 1f; 
        InputActions.FindActionMap("Dialogue").Disable(); 
        InputActions.FindActionMap("Player").Enable(); 
        ClearDialogueHistory();
        Debug.Log("Story Ended");
        inkExternalFunctions.unbindIncreaseSIL(currentStory);
        inkExternalFunctions.unbindGetSIL(currentStory);
        currentStory = null;
        dialogueCanvas.SetActive(false);    
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    public void ChooseChoice(int index)
    {

        ClearChoices();
        currentStory.ChooseChoiceIndex(index);
        ContinueStory();
    }

    private void DisplayChoices()
    {
        // Clear old buttons
        foreach (Transform child in choicesPanel)
        {
            Destroy(child.gameObject);
        }

        // Create new buttons
        for (int i = 0; i < currentStory.currentChoices.Count; i++)
        {
            Choice choice = currentStory.currentChoices[i];
            Debug.Log("Displaying choice: " + choice.text);

            GameObject buttonObj =
                Instantiate(choiceButtonPrefab, choicesPanel);
            
            TMP_Text buttonText =
                buttonObj.GetComponentInChildren<TMP_Text>();

            buttonText.text = choice.text;

            int choiceIndex = i;

            Button button = buttonObj.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                ChooseChoice(choiceIndex);
            });
        }
        // Canvas.ForceUpdateCanvases();
        // LayoutRebuilder.ForceRebuildLayoutImmediate(choicesPanel as RectTransform);
    }

    private void ClearChoices()
    {
        foreach (Transform child in choicesPanel)
        {
            Destroy(child.gameObject);
        }
    }


    private void DialogueInteractInputChecker(){
        if(dialogueAdvanceInput.WasPressedThisFrame()){
            if (DialogueManager.Instance.DialogueActive)
            {
                DialogueManager.Instance.ContinueStory();
                return;
            }
        }
    }

    private void AddDialogueLine(string text)
{
    GameObject entry = Instantiate(dialogueEntryPrefab, dialogueContent);

    TMP_Text textComponent = entry.GetComponentInChildren<TMP_Text>();
    textComponent.text = text;

    textComponent.ForceMeshUpdate();

    LayoutElement layout = entry.GetComponent<LayoutElement>();
    layout.preferredHeight = textComponent.preferredHeight;

    Canvas.ForceUpdateCanvases();

    LayoutRebuilder.ForceRebuildLayoutImmediate(
        dialogueContent.GetComponent<RectTransform>()
    );
}
    private void ClearDialogueHistory(){
        foreach (Transform child in dialogueContent)
        {
            Destroy(child.gameObject);
        }
    }
}