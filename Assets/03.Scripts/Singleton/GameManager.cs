using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//public class GameManager : Singleton<GameManager>
public class GameManager : MonoBehaviour
{
    public bool gameIsOver;
    public bool gameIsPause;
    public GameCanvas gameCanvas;
    public ActorsManager actorsManager;
    private static GameManager _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameIsOver = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var levelSelector = FindObjectOfType<LevelSelector>();
            levelSelector.WaitPanel.SetActive(true);
            levelSelector.LoadScene("02-Level01");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            WinLevel();
        }
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.Z))
        {
            if (SceneManager.GetActiveScene().name != SceneNames.OFFLINEMODE)
            {
                gameIsPause = !gameIsPause;
                FindObjectOfType<AudioManager>().DoMute(gameIsPause);
                gameCanvas.Pause(gameIsPause);
            }
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(4.0f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("02-Level02");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Load()
    {
        FindObjectOfType<AudioManager>().Load();
        var anim = gameCanvas.LoadAnimator();
        anim.SetTrigger("fade_in");
    }


    public void LoadScene(string sceneName)
    {
        StartCoroutine(AsyncLoad(sceneName));
    }

    private IEnumerator AsyncLoad(string sceneName)
    {
        yield return new WaitForSeconds(5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void WinLevel()
    {
        GameObject player = gameCanvas.playerConstraint.gameObject;
        player.GetComponent<Health>().isInvincible = true;
        FindObjectOfType<AudioManager>().Win();
        FindObjectOfType<LightWaker>().Light();

        for (int i = 0; i < actorsManager.actors.Count; i++)
        {
            Health health = actorsManager.actors[i].gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(99999.9f, null);
            }
        }
        gameIsOver = true;
        gameCanvas.WinLevel();
    }

    public void LoseLevel()
    {
        FindObjectOfType<AudioManager>().Lose();

        gameIsOver = true;
        gameCanvas.LoseLevel();
    }
}
