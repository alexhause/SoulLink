using UnityEngine;

public class StartDialogueOnStart : MonoBehaviour
{
    [SerializeField] private TextAsset inkJson;

    private void Start()
    {
        DialogueManager.Instance.EnterDialogueMode(inkJson);
    }
}


