using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectedUI : MonoBehaviour
{
    [SerializeField] private Image[] image;
    private PlayerAttack player;
    private int selectedWeapon = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Image i in image)
        {
            i.gameObject.SetActive(false);
        }
        player = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            selectedWeapon = player.GetSelectedWeapon();
            foreach (Image i in image)
            {
                if (i == image[selectedWeapon])
                {
                    i.gameObject.SetActive(true);
                }
                else
                {
                    i.gameObject.SetActive(false);
                }
            }
        }
    }
}