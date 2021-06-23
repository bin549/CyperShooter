using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool Turn { get; set; } = false;
    public bool isTurn;
    [SerializeField] private GameObject flashlight;

    private void Update()
    {
        if (Turn)
        {
            flashlight.SetActive(!flashlight.activeSelf);
            Turn = false;
        }
    }
}
