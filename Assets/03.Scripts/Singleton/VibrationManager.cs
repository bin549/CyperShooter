using System.Collections;
using UnityEngine;

public class VibrationManager : Singleton<VibrationManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void VibrateController(float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
    }

    public void VibrateController(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        StartCoroutine(VibrateForSeconds(duration, frequency, amplitude, controller));
    }

    private IEnumerator VibrateForSeconds(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        TurnOffVibrate(controller);
    }

    public void TurnOffVibrate(OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
