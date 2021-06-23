using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CooperationModeGameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] PlayerPrefabs;
    [SerializeField] private bool gameStart = false;
    [SerializeField] private bool gameOver = false;
    [SerializeField] private bool hasWeapon = false;

    public bool GameStart { get => gameStart; set => gameStart = value; }
    public bool GameIsOver { get => gameOver; set => gameOver = value; }
    public bool HasWeapon { get => hasWeapon; set => hasWeapon = value; }

    private float countdown = 2f;
    public float Countdown { get => countdown; set => countdown = value; }

    public WaveSpawner waveSpawner;

    private void Awake()
    {
        waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerStatus.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                int randomPosition_x = Random.Range(14, 30);
                int randomPosition_z = Random.Range(-30, -22);
                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name, new Vector3(randomPosition_x, -2, randomPosition_z), Quaternion.identity);
            }
        }
    }

    public float WaveIndex()
    {
        return waveSpawner.WaveIndex;
    }

    private void Update()
    {
        if (!HasWeapon)
            return;
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        if (countdown <= 0f && !GameStart)
        {
            GameStart = true;
            waveSpawner.NextWave();
        }

        if (GameIsOver)
            return;

        /*if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }*/
    }

    public void OnQuitMatchButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("00-Choose-2");
    }

    private void EndGame()
    {
        GameIsOver = true;
    }

    public void WinLevel()
    {
        GameIsOver = true;
        Debug.Log("Player Won!");
    }

    public void PlayerLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
