using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] Transform[] questListContent;
    [SerializeField] GameObject questEntryPrefab;
    [SerializeField] GameObject objectiveTextPrefab;

    void Start()
    {
        UpdateQuestUI();
    }
    public void UpdateQuestUI()
    {
        for (int i = 0; i < questListContent.Length; i++)
        {
            foreach (Transform child in questListContent[i])
            {
                Destroy(child.gameObject);
            }
            foreach (var quest in QuestController.Instance.activateQuests)
            {
                GameObject entry = Instantiate(questEntryPrefab, questListContent[i]);
                TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
                Transform objectiveList = entry.transform.Find("ObjectiveList");
                questNameText.text = quest.quest.name;
                foreach (var obj in quest.objectives)
                {
                    GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                    TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                    objText.text = $"{obj.description} ({obj.amount}/{obj.requiredAmount})";
                }
            }
        }
    }
}