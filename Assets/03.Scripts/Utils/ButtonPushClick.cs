using System.Collections;
using UnityEngine;
using TMPro;
public class ButtonPushClick : MonoBehaviour
{
    public float MinLocalY = 0.25f;
    public float MaxLocalY = 0.55f;
    public bool isBeingTouched = false;
    public bool isClicked = false;
    public string scenename;
    public GameObject timeCountDownCanvas;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeText;
    public float smooth = 0.1f;

    private void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);
        timeCountDownCanvas.SetActive(false);
    }

    private void Update()
    {
        Vector3 buttonDownPosition = new Vector3(transform.localPosition.x, MinLocalY, transform.localPosition.z);
        Vector3 buttonUpPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);
        if (!isClicked)
        {
            if (!isBeingTouched && (transform.localPosition.y > MaxLocalY || transform.localPosition.y < MaxLocalY))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, buttonUpPosition, Time.deltaTime * smooth);
            }

            if (transform.localPosition.y < MinLocalY)
            {
                isClicked = true;
                transform.localPosition = buttonDownPosition;
                OnButtonDown();
            }
        }
    }

    private void OnButtonDown()
    {
        GetComponent<Collider>().isTrigger = true;
        //  AudioManager.Instance.buttonClickSound.gameObject.transform.position = transform.position;
        //  AudioManager.Instance.buttonClickSound.Play();
        titleText.text = scenename.Split('-')[scenename.Split('-').Length - 1];
        StartCoroutine(StartGame(3));
    }

    private IEnumerator StartGame(float countDownValue)
    {
        timeText.text = countDownValue.ToString();
        timeCountDownCanvas.SetActive(true);
        while (countDownValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            countDownValue -= 1;
            timeText.text = countDownValue.ToString();
        }
        SceneLoader.Instance.LoadScene(scenename);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClicked)
        {
            //  AudioManager.Instance.buttonClickSound.gameObject.transform.position = transform.position;
            //AudioManager.Instance.buttonClickSound.Play();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag != "BackButton")
        {
            isBeingTouched = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag != "BackButton")
        {
            isBeingTouched = false;
        }
    }
}
