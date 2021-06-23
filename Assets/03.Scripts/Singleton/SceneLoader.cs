using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public OVROverlay overlay_Background, overlay_LoadingText;

    private void Awake()
    {
        //  DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(ShowOverlayAndLoad(sceneName));
    }

    private IEnumerator ShowOverlayAndLoad(string sceneName)
    {
        overlay_Background.enabled = true;
        overlay_LoadingText.enabled = true;

        GameObject centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        overlay_LoadingText.gameObject.transform.position = centerEyeAnchor.transform.position + new Vector3(0f, 0f, 3f);

        yield return new WaitForSeconds(5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        overlay_Background.enabled = false;
        overlay_LoadingText.enabled = false;

        yield return null;
    }
}
