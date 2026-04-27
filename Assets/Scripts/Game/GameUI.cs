using UnityEngine;

public class GameUI : MonoBehaviour
{

    private CanvasGroup ui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ui = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseController.IsGamePaused)
        {
            ui.alpha = 0.1f;
        }
        else
        {
            ui.alpha = 1f;
        }
    }
}