using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public ItemSO item;
    [TextArea]
    [SerializeField] private string itemDescription;
    public ItemType type;
}
public enum ItemType
{
    Equipment,
    Weapon,
    Consumable,
    None
};