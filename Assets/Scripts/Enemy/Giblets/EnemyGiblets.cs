using UnityEngine;

public class EnemyGiblets : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Arena").transform);
    }
}