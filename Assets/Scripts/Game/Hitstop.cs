using System.Collections;
using UnityEngine;

public class Hitstop : MonoBehaviour
{
    public static Hitstop Instance { get; private set; }
    bool isWaiting;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    public void Stop(float duration)
    {
        if (isWaiting) { return; }
        Time.timeScale = 0f;
        StartCoroutine(Wait(duration));
    }
    IEnumerator Wait(float duration)
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        isWaiting = false;
    }
}