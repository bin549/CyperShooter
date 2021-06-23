using UnityEngine;
using Photon.Pun;

public class WeaponStats : MonoBehaviour
{
    public int currentLevel = 0;
    public int maxLevel = 5;
    public int currentEXP = 0;
    public int[] expToNextLevel;
    public int baseEXP = 0;
    [SerializeField] private GameObject[] levelUpEffects;
    public PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[0] = baseEXP;
        for (int i = 1; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.25f);
        }
    }

    public bool AddEXP(int expToAdd)
    {
        currentEXP += expToAdd;
        if (currentLevel < maxLevel)
        {
            if (currentEXP >= expToNextLevel[currentLevel])
            {
                currentEXP -= expToNextLevel[currentLevel];
                photonView.RPC("SpawnLevelUpEffect", RpcTarget.All);
                currentLevel++;
                return true;
            }
        }
        if (currentLevel >= maxLevel)
        {
            currentEXP = 0;
            return false;
        }
        return false;
    }

    [PunRPC]
    private void SpawnLevelUpEffect()
    {
        GameObject levelUpEffect = GameObject.Instantiate(levelUpEffects[currentLevel], transform.position, transform.rotation);
        Destroy(levelUpEffect, 1.5f);
    }
}
