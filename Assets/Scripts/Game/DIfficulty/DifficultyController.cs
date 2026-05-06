using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public static DifficultyController Instance { get; private set; }
    public float difficulty = 1;
    [SerializeField] private float timeScale = (1f / 60f);

    public float ordersCompleted;
    public float ordersFailed;
    public float enemiesDefeated;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void FixedUpdate()
    {
        difficulty += Time.deltaTime * timeScale;
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
}