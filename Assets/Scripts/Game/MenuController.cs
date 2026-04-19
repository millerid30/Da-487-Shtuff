using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    private InputAction playerControls;

    [SerializeField] private InputActionReference menuToggle;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        menuToggle.action.performed += Menu;
    }
    private void OnDisable()
    {
        menuToggle.action.performed -= Menu;
    }

    void Menu(InputAction.CallbackContext context)
    {
        if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
        {
            return;
        }
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        PauseController.SetPause(menuCanvas.activeSelf);
    }
}