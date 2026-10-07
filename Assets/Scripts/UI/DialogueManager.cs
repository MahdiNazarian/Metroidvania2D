using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.3f;
    [SerializeField] private GatherInput gatherInput;

    private Dialogue currentConversation;
    private DialogeLine currentDialogueLine;
    private int currentDialogueIndex;
    private bool isTyping;
    private Coroutine dialogueCoroutine;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        gatherInput.nextDialogueActionBlueprint.SetActionPerformedInstructions(ContinueDialogue);
    }
    private void OnDisable()
    {
        gatherInput.nextDialogueActionBlueprint.RemoveActionPerformedInstructions(ContinueDialogue);
    }
    public void StartConversation(Dialogue conversation)
    {
        gatherInput.ChangeToDialogueMap();
        dialogueText.text = "";
        dialogueUI.SetActive(true);
        currentConversation = conversation;
        currentDialogueIndex = 0;
        ShowLine();
    }
    private void NextLine()
    {
        currentDialogueIndex += 1;
        if (currentDialogueIndex < currentConversation.dialogues.Length)
        {
            ShowLine();
        }
        else
        {
            FinishDialogue();
        }
    }
    private void FinishDialogue()
    {
        dialogueUI.SetActive(false);
        gatherInput.ChangeToPlayerMainMap();
        speakerImage.sprite = null;
        speakerName.text = "";
    }
    private void ContinueDialogue()
    {
        if (isTyping)
        {
            dialogueText.text = currentDialogueLine.dialogLine;
            StopCoroutine(dialogueCoroutine);
            isTyping = false;
        }
        else
        {
            NextLine();
        }
    }
    private void ShowLine()
    {
        currentDialogueLine = currentConversation.dialogues[currentDialogueIndex];
        speakerImage.sprite = currentDialogueLine.speakerIcon;
        speakerName.text = currentDialogueLine.speakerName;
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(TypeLine(currentDialogueLine.dialogLine));
    }
    private IEnumerator TypeLine(string line)
    {
        isTyping = true;     
        dialogueText.text = "";
        foreach(char dialogueChar in line)
        {
            dialogueText.text += dialogueChar;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

}
