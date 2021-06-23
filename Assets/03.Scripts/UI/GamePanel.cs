using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ToggleGroup))]
[RequireComponent(typeof(VerticalLayoutGroup))]
public class GamePanel : MonoBehaviour
{
    private PanelOption[] options;
    private PanelOption currentOption;
    public int optionIndex;
    public ToggleGroup toggleGroup;
    public GameObject gameCanvas;

    private void Awake()
    {
        gameCanvas = transform.parent.gameObject;
        options = GetComponentsInChildren<PanelOption>(true);
        toggleGroup = GetComponent<ToggleGroup>();
    }

    private void Start()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].Init(this);
        }
        currentOption = options[optionIndex];
        currentOption.Selected();
    }

    private void Update()
    {
        ChooseMode();
    }

    private void ChooseMode()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.W))
        {
            PeviewOption();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.S))
        {
            NextOption();
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.J))
        {
            string currentLevelName = SceneManager.GetActiveScene().name;
            int currentMode = int.Parse(currentLevelName.Substring(0, 2));
            //Debug.Log(currentMode);
            if (currentMode == 2)
            {
                FindObjectOfType<GameManager>().gameIsOver = false;
                transform.parent = null;
                FindObjectOfType<GameManager>().Load();
            }

            if (currentOption != null)
            {
                currentOption.Confirm();
                gameObject.SetActive(false);
            }
            else
            {
                currentOption = options[0];
                currentOption.Selected();
                currentOption.Confirm();
            }
            /*
          */
        }
    }

    private void PeviewOption()
    {
        optionIndex--;
        if (optionIndex == -1)
        {
            optionIndex = options.Length - 1;
        }
        currentOption = options[optionIndex];
        currentOption.Selected();
    }

    private void NextOption()
    {
        optionIndex++;
        optionIndex %= options.Length;
        currentOption = options[optionIndex];
        currentOption.Selected();
    }
}
