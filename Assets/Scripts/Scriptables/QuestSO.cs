using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSO", menuName = "Quests/QuestSO")]
public class QuestSO : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }
}
[System.Serializable]
public class QuestObjective
{
    public string objectiveID;
    public string description;
    public ObjectiveType type;
    public int requiredAmount;
    public int amount;

    public bool IsCompleted => amount >= requiredAmount;
}
public enum ObjectiveType
{
    Eliminate,
    Survive,
    Other
};

[System.Serializable]
public class QuestProgress
{
    public QuestSO quest;
    public List<QuestObjective> objectives;

    public QuestProgress(QuestSO quest)
    {
        this.quest = quest;
        objectives = new List<QuestObjective>();

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                amount = 0
            });
        }
    }
    public bool IsCompleted => objectives.TrueForAll(x => x.IsCompleted);

    public string QuestID => quest.questID;
}