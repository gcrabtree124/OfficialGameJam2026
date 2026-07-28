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

    [SerializeField] private Transform choicesPanel;

    [SerializeField] private GameObject choiceEntryPrefab;

    private List<GameObject> activeChoices = new List<GameObject>();

    [SerializeField] private GameObject dialogueCanvas;

    [SerializeField] private ScrollRect dialogueScrollRect;
    
    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image portraitImage;


    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text dialogueText;

    private float viewportHeight;
    private float contentHeight;
    private int fullPanels = 0;


    private Dictionary<string, Sprite> backgrounds = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> portraits = new Dictionary<string, Sprite>();
    

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    [SerializeField]
    private float typingSpeed = 0.01f;
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
        CheckChoiceInput();
    }
    public void StartStory(TextAsset inkJSON)
    {

        //set time scale to 0 to pause the game
        //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Time-timeScale.html

        Time.timeScale = 0f; 
        InputActions.FindActionMap("Player").Disable(); 
        InputActions.FindActionMap("Dialogue").Enable(); 
        currentStory = new Story(inkJSON.text);
        inkExternalFunctions.bindIncreaseSIL(currentStory, "increaseSIL");
        inkExternalFunctions.bindGetSIL(currentStory);
        
        //StartCoroutine(ScrollToBottom());
        // ContinueStory();
        StartCoroutine(StartDialogueAfterFrame());

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private IEnumerator StartDialogueAfterFrame()
    {
        yield return null;

        dialogueCanvas.SetActive(true);


        Canvas.ForceUpdateCanvases();

        ContinueStory();
    }

    public void ContinueStory()
    {
        if (currentStory == null)
            return;

        ClearChoices();

        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();

            HandleTags();

            if (!string.IsNullOrWhiteSpace(text))
            {
                StartCoroutine(TypeText(text));
            }
        }

        if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else if (!currentStory.canContinue)
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
        //dialogueText.text = ""; // Clear the dialogue text when displaying choices
        HandleTags();
        for (int i = 0; i < currentStory.currentChoices.Count; i++)
        {
            Choice choice = currentStory.currentChoices[i];


            GameObject entry = Instantiate(choiceEntryPrefab, choicesPanel);

            activeChoices.Add(entry);

            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            text.text = choice.text; // cut: (i + 1) + ". " + 

            Button button = entry.GetComponent<Button>();

            int choiceIndex = i;

            button.onClick.AddListener(() =>
            {
                ChooseChoice(choiceIndex);
            });
        }

        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            choicesPanel.GetComponent<RectTransform>()
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


    private void DialogueInteractInputChecker()
    {
        if(dialogueAdvanceInput.WasPressedThisFrame())
        {
            if (!DialogueActive)
                return;

            if (isTyping)
            {
                dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;
 
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                isTyping = false;
            }
            else
            {
                ContinueStory();
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

        Debug.Log("Current Tags: " + string.Join(", ", currentStory.currentTags));
        foreach(string tag in currentStory.currentTags)
        {
            string[] split = tag.Split(' ');
            Debug.Log("Tag: " + tag);
            if(split.Length < 2){
                Debug.LogWarning("Tag does not have enough parameters: " + tag);
                continue;

            }


            switch(split[0])
            {
                case "bg":

                    if(backgrounds.ContainsKey(split[1]))
                    {
                        backgroundImage.sprite = backgrounds[split[1]];
                    }

                    break;


                case "sprite":

                    if(portraits.ContainsKey(split[1]))
                    {
                        portraitImage.sprite = portraits[split[1]];
                    }

                    break;
            }
        }
    }


    private IEnumerator TypeText(string text)
    {
        isTyping = true;

        dialogueText.text = text;
        dialogueText.ForceMeshUpdate();

        int totalCharacters = dialogueText.textInfo.characterCount;

        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(typingSpeed);

        for (int i = 0; i <= totalCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return delay;
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void CheckChoiceInput()
    {
        if (currentStory == null  || currentStory.currentChoices.Count == 0 || currentStory.currentChoices == null)
            return;

        int choiceCount = currentStory.currentChoices.Count;

        if (Keyboard.current.digit1Key.wasPressedThisFrame && choiceCount >= 1)
        {
            ChooseChoice(0);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame && choiceCount >= 2)
        {
            ChooseChoice(1);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame && choiceCount >= 3)
        {
            ChooseChoice(2);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame && choiceCount >= 4)
        {
            ChooseChoice(3);
        }
    }
}