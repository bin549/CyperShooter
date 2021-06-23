using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        PlayerPrefs.SetInt("levelReached", 1);
        SceneManager.LoadScene(SceneNames.OFFLINEMODE);
        this.gameObject.SetActive(false);
    }
}
