using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyDetector : MonoBehaviour
{
    public InputField inputField;
    public GameObject keyBoard;
    private bool iskeyBoardShow = false;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            iskeyBoardShow = !iskeyBoardShow;
            keyBoard.SetActive(iskeyBoardShow);
        }
    }

    public void SetTextSource(InputField inputField)
    {
        this.inputField = inputField;
    }

    private void OnTriggerEnter(Collider other)
    {
        var key = other.GetComponentInChildren<TextMeshPro>();
        if (key != null)
        {
            var keyFeedBack = other.gameObject.GetComponent<KeyFeedback>();
            if (keyFeedBack.keyCanBeHitAgain)
            {
                if (key.text == "SPACE")
                {
                    inputField.text += " ";
                }
                else if (key.text == "<-")
                {
                    inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
                }
                else
                {
                    inputField.text += key.text;
                }
                keyFeedBack.keyHit = true;
            }
        }
    }

    public void SetSourceText(string text)
    {
        if (inputField != null)
        {
            inputField.text = text;
        }
    }
}
