using UnityEngine;
using UnityEngine.InputSystem;

public class DifficultyController : MonoBehaviour
{
    public static DifficultyController Instance { get; private set; }
    public float difficulty = 1;
    [SerializeField] private float timeScale = (1f / 120f);
    [SerializeField] private bool isPaused;
    [SerializeField] private float timer;
    private float maxTimer = 30;

    public float ordersCompleted;
    public float ordersFailed;
    public float enemiesDefeated;
    public float wavesCompleted;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        timer = maxTimer;
        isPaused = false;
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            difficulty += Time.deltaTime * timeScale;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                SetTimeScale();
                timer = maxTimer;
            }
        }
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            SetTimeScale();
            timer = maxTimer;
        }
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseDifficultyClock(!isPaused);
        }
    }
    public void IncreaseDifficulty(float amount)
    {
        difficulty += amount * timeScale;
    }
    public void DecreaseDifficulty(float amount)
    {
        difficulty -= amount * timeScale;
    }
    public void SetDifficulty(float difficulty)
    {
        this.difficulty = difficulty;
    }
    public void SetTimeScale()
    {
        CinemachineShake.Instance.Shake(10, 1);
        timeScale *= 1 + (((1 + ordersFailed) * Mathf.Log(1 + enemiesDefeated + wavesCompleted, 10)) / (1 + ordersCompleted));
    }
    public void PauseDifficultyClock(bool pause)
    {
        isPaused = pause;
    }
}