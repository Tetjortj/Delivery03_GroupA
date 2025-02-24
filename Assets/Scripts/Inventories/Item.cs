using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int cost;
    public ItemType type;
    public GameObject prefab;

    public enum ItemType
    {
        Food,
        Potion,
        Weapon
    }
}