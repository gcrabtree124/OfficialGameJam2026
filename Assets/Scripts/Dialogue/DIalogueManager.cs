using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Story currentStory;

    private InputAction dialogueAdvanceInput; //E key to advance dialogue

    private InkExternalFunctions inkExternalFunctions; //functions to be called in the ink editor

    [SerializeField] private GameObject dialogueEntryPrefab;
    
    [SerializeField] private Transform dialogueContent;

    [SerializeField] private GameObject choiceEntryPrefab;

    private List<GameObject> activeChoices = new List<GameObject>();

    [SerializeField] private GameObject dialogueCanvas;

    [SerializeField] private ScrollRect dialogueScrollRect;
    
    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image portraitImage;

    private float viewportHeight;
    private float contentHeight;
    private int fullPanels = 0;


    private Dictionary<string, Sprite> backgrounds = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> portraits = new Dictionary<string, Sprite>();
    
    public bool DialogueActive
    {
        get { return currentStory != null; }
    }

    private void Awake()
    {
        Instance = this;
        dialogueCanvas.SetActive(false);
        inkExternalFunctions = new InkExternalFunctions();

        populateDictionariesWithAssets();
        
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

        Time.timeScale = 0f; 
        InputActions.FindActionMap("Player").Disable(); 
        InputActions.FindActionMap("Dialogue").Enable(); 
        dialogueCanvas.SetActive(true);
        currentStory = new Story(inkJSON.text);
        inkExternalFunctions.bindIncreaseSIL(currentStory, "increaseSIL");
        inkExternalFunctions.bindGetSIL(currentStory);
        
        StartCoroutine(ScrollToBottom());
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

            HandleTags();
            if (text.Equals("") && !currentStory.canContinue)  //making sure there are no white space at the end of dialogue
            {
                EndDialogue();
                return;
            } 
            else
            {
                AddDialogueLine(text);
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
        for (int i = 0; i < currentStory.currentChoices.Count; i++)
        {
            Choice choice = currentStory.currentChoices[i];

            GameObject entry = Instantiate(choiceEntryPrefab, dialogueContent);

            activeChoices.Add(entry);

            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            text.text = (i + 1) + ". " + choice.text;

            Button button = entry.GetComponent<Button>();

            int choiceIndex = i;

            button.onClick.AddListener(() =>
            {
                ChooseChoice(choiceIndex);
            });
        }

        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            dialogueContent.GetComponent<RectTransform>()
        );
    }

    private void ClearChoices()
    {
        foreach(GameObject choice in activeChoices)
        {
            Destroy(choice);
        }

        activeChoices.Clear();
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

    if (IsAtBottom()){
        RectTransform entryRect = entry.GetComponent<RectTransform>();
        RectTransform contentRect = dialogueContent.GetComponent<RectTransform>();

        float emptySpace = dialogueScrollRect.viewport.rect.height - entryRect.rect.height;
        VerticalLayoutGroup dialogueLayoutGroup = dialogueContent.GetComponent<VerticalLayoutGroup>();
        dialogueLayoutGroup.padding.bottom = Mathf.Max(0, Mathf.RoundToInt(emptySpace));

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        dialogueScrollRect.verticalNormalizedPosition = 0f;
    }
    
    
    
}
    private void ClearDialogueHistory(){

        foreach (Transform child in dialogueContent)
        {
            Destroy(child.gameObject);
        }
    }


    private IEnumerator ScrollToBottom(){

    yield return null;
    dialogueScrollRect.verticalNormalizedPosition = 0f;

}

    bool IsAtBottom(){

        RectTransform content = dialogueContent.GetComponent<RectTransform>();
        RectTransform viewport = dialogueScrollRect.viewport;

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        int contentRatio = (int)(contentHeight / viewportHeight);

        // no scrolling needed
        if (contentRatio > fullPanels){
            fullPanels = contentRatio;
            return true;
        }
        return false;
        
}

    private void populateDictionariesWithAssets(){

        Sprite[] loadedBackgrounds = Resources.LoadAll<Sprite>("Backgrounds");

        foreach(Sprite sprite in loadedBackgrounds)
        {
            backgrounds.Add(sprite.name, sprite);
        }


        Sprite[] loadedPortraits = Resources.LoadAll<Sprite>("Portraits");

        foreach(Sprite sprite in loadedPortraits)
        {
            portraits.Add(sprite.name, sprite);
        }
    }

    private void HandleTags(){

        foreach(string tag in currentStory.currentTags)
        {
            string[] split = tag.Split(' ');

            if(split.Length < 2)
                continue;


            switch(split[0])
            {
                case "bg":

                    if(backgrounds.ContainsKey(split[1]))
                    {
                        backgroundImage.sprite = backgrounds[split[1]];
                    }

                    break;


                case "portrait":

                    if(portraits.ContainsKey(split[1]))
                    {
                        portraitImage.sprite = portraits[split[1]];
                    }

                    break;
            }
        }
    }

}