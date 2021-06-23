using UnityEngine;
using UnityEngine.UI;

public class ModeSelect : MonoBehaviour
{
    public Button onlineModeButton;
    public Button offlineModeButton;
    private SceneLoader sceneLoader;

    private void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        onlineModeButton = GameObject.Find("onlineModeButton").GetComponent<Button>();
        offlineModeButton = GameObject.Find("offlineModeButton").GetComponent<Button>();
    }

    private void Start()
    {
        onlineModeButton.onClick.AddListener(() =>
        {
            OnlineMode();
        });
        offlineModeButton.onClick.AddListener(() =>
        {
            OfflineMode();
        });
    }

    public void OnlineMode()
    {
        sceneLoader.LoadScene(SceneNames.ONLINEMODE);
    }

    public void OfflineMode()
    {
        sceneLoader.LoadScene(SceneNames.OFFLINEMODE);
        /*
        if (PlayerPrefs.GetInt("levelReached", 0) == 0)
        {
          sceneLoader.LoadScene(SceneNames.LEVEL0);
        }
        else{
        }*/
    }
}
