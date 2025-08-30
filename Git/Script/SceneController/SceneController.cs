using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("More than one SceneController instance!");
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}


