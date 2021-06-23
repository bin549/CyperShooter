using UnityEngine;

public class WeatherDB : MonoBehaviour
{
    [SerializeField] private Weather[] weathers;
    [SerializeField] private Weather currentWeather;
    [SerializeField] private int currentWeatherIndex;
    [SerializeField] private int dayCounter = 0;

    private void Awake()
    {
        currentWeather = weathers[currentWeatherIndex];
        SetWeatherEnv(true);
    }

    public void Tomorrow()
    {
        dayCounter++;
        if (dayCounter == currentWeather.duration && currentWeatherIndex != weathers.Length)
        {
            SetWeatherEnv(false);
            currentWeatherIndex++;
            currentWeather = weathers[currentWeatherIndex];
            SetWeatherEnv(true);
            dayCounter = 0;
        }
    }

    private void SetWeatherEnv(bool isVisible)
    {
        foreach (GameObject env in currentWeather.envs)
        {
            env.SetActive(isVisible);
        }
    }
}
