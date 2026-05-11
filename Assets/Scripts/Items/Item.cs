using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public ItemSO item;

    [TextArea]
    [SerializeField] private string itemDescription;
    public ItemType type;
    public int quantity = 1;
    private TMP_Text qtyText;
    private void Awake()
    {
        qtyText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityDisplay();
    }
    public void UpdateQuantityDisplay()
    {
        if (qtyText != null)
        {
            qtyText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }
    public void AddToStack(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityDisplay();
    }
    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        UpdateQuantityDisplay();
        return removed;
    }
    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        //clone.transform.localScale = gameObject.transform.localScale;
        //clone.GetComponent<RectTransform>().localScale = Vector3.one;
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.quantity = newQuantity;
        cloneItem.UpdateQuantityDisplay();
        return clone;
    }
}
public enum ItemType
{
    Equipment,
    Weapon,
    Consumable,
    None
};