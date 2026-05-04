using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<QuestProgress> activateQuests = new();
    public QuestUI questUI;
    //
    public QuestSO testQuest;
    /// </summary>

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        questUI = FindAnyObjectByType<QuestUI>();
    }
    public void AcceptQuest(QuestSO quest)
    {
        if (IsQuestActive(quest.questID)) { return; }
        activateQuests.Add(new QuestProgress(quest));
        questUI.UpdateQuestUI();
    }

    public void Start()
    {
        AcceptQuest(testQuest);
    }

    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);
}