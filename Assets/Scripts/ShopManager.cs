using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Inventory shopInventoryAsset;
    [HideInInspector]
    public Inventory shopInventoryRuntime; 

    void Awake()
    {
        shopInventoryRuntime = Instantiate(shopInventoryAsset);
    }
}
