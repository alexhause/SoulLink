using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public event Action OnDialogueModeEnter;
    public event Action OnDialogueModeExit;

    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;

    [Header("Choices UI")]
    [SerializeField] private List<TextMeshProUGUI> choices;
    private bool isTyping = false;
    private Ink.Runtime.Story currentStory;

    public static DialogueManager Instance { get; private set; }
    public bool dialogueIsPlaying { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("More than one DialogueManager instance!");
            return;
        }

        Instance = this;
        dialogueIsPlaying = false;
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (isTyping && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isTyping = false;
            return;
        }

        if (currentStory.currentChoices.Count == 0 && dialogueIsPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ContinueStory();
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        OnDialogueModeEnter?.Invoke();
        currentStory = new Ink.Runtime.Story(inkJSON.text);
        dialogueIsPlaying = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        dialogueText.text = "";
        HideChoicesUI();
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialogueText.text = "";
        HideChoicesUI();
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        OnDialogueModeExit?.Invoke();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
                continue;
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in sentence)
        {
            if (!isTyping)
            {
                dialogueText.text = sentence;
                break;
            }
            dialogueText.text += c;
            yield return null;
        }
        isTyping = false;
        DisplayChoices();
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Count)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choices[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Count; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        HideChoicesUI();
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void HideChoicesUI()
    {
        for (int i = 0; i < choices.Count; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }
}
