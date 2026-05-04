using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public static DifficultyController Instance { get; private set; }
    public float difficulty;
    [SerializeField] private float timeScale;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void FixedUpdate()
    {

    }
    void IncreaseDifficulty(float amount)
    {

    }
    void DecreaseDifficulty(float amount)
    {

    }
    void SetDifficulty(float difficulty)
    {

    }
}