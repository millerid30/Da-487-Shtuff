using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;
    private void Awake()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
    }
    public static void SetPause(bool pause)
    {
        Time.timeScale = 0f;
        IsGamePaused = pause;
        if (!IsGamePaused)
        {
            Time.timeScale = 1f;
        }
    }
}