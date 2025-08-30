using UnityEngine;

public class CharacterContoler : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueJSON;
    private void Start()
    {
        DialogueManager.Instance.EnterDialogueMode(dialogueJSON);
    }

    private void Update()
    {

    }

}


