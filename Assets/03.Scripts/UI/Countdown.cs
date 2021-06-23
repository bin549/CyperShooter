using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public CooperationModeGameManager cooperationModeGameManager;

    public Text countdownText;

    private void Awake()
    {
        cooperationModeGameManager = GameObject.FindObjectOfType<CooperationModeGameManager>();
    }

    private void Update()
    {
        if (!cooperationModeGameManager.GameStart)
        {
            countdownText.text = string.Format("{0:00.00}", cooperationModeGameManager.Countdown);
        }
        else
        {
            countdownText.text = "Wave" + cooperationModeGameManager.WaveIndex();
        }
    }
}
