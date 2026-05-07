using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<QuestProgress> activateQuests = new();
    public QuestUI questUI;

    public List<string> completeQuestIDs = new();

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
    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);
    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.IsCompleted);
    }
    public void CompleteQuest(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestID == questID);
        if (quest != null)
        {
            if (!IsQuestHandedIn(questID))
            {
                completeQuestIDs.Add(questID);
            }
            activateQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }
    }
    public bool IsQuestHandedIn(string questID)
    {
        return completeQuestIDs.Contains(questID);
    }
}