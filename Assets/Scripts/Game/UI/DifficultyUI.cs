using TMPro;
using UnityEngine;

public class DifficultyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text diffText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        diffText.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        diffText.text = DifficultyController.Instance.difficulty.ToString("0.###");
    }
}