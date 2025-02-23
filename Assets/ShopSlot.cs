using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    //item Data

    private string itemName;
    private int quantity;
    private Sprite sprite;
    private int cost;
    private bool isFull;

    // item slot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
