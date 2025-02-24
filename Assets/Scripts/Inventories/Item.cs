using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public int cost;
    public ItemType type;

    public enum ItemType
    {
        Food,
        Potion,
        Weapon
    }
}
