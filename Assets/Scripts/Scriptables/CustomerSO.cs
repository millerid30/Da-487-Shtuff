using UnityEngine;

[CreateAssetMenu(fileName = "CustomerSO", menuName = "Scriptable Objects/CustomerSO")]
public class CustomerSO : ScriptableObject
{
    public string customerID;
    public string customerName;
    public float maxWaitTime;
    public float rewardMulti;
    public float failMulti;
    public bool isBoss;
    public QuestSO customerQuest;
    public GameObject[] customerTips;
    public GameObject[] customerShits;
}