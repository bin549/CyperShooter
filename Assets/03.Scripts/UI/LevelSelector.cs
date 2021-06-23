using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
    public GameObject buttonsContainer;
    public Button[] levelButtons;
    private SceneLoader sceneLoader;
    public int levelNum;
    public int levelReached;
    public GameObject WaitPanel;

    private void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        levelButtons = buttonsContainer.GetComponentsInChildren<Button>(true);
    }

    private void Start()
    {
        levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false;
        }

        PlayerPrefs.SetInt("levelNum", levelButtons.Length);
        levelNum = PlayerPrefs.GetInt("levelNum");
    }

    private void Update()
    {
        Reset();
    }

    private void Reset()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey("levelReached");
        }
    }

    public void Select(string levelName)
    {
        WaitPanel.SetActive(true);
        LoadScene(levelName);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }


    private IEnumerator Load(string sceneName)
    {
        yield return new WaitForSeconds(2.0f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
