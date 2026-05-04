using Unity.Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    private CinemachineCamera cam;
    //private CinemachineBasicMultiChannelPerlin perlin;
    private float timer;
    private float timerTotal;
    private float startForce;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        //Instance = this;
        cam = GetComponent<CinemachineCamera>();
        //perlin = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Shake(float force, float duration)
    {
        CinemachineBasicMultiChannelPerlin perlin =
            cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.AmplitudeGain = force;

        startForce = force;
        timerTotal = duration;
        timer = duration;
    }
    public enum ShakeType
    {
        EaseIn,
        EaseOut,
        NoEase
    };
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                CinemachineBasicMultiChannelPerlin perlin =
            cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.AmplitudeGain = Mathf.Lerp(startForce, 0f, 1 - timer / timerTotal);
            }
        }
    }
}