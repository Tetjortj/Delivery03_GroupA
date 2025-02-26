using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Inventory shopInventoryAsset; // Asigna el asset original desde el editor
    [HideInInspector]
    public Inventory shopInventoryRuntime; // Esta ser� la copia en tiempo de ejecuci�n

    void Awake()
    {
        // Clona el ScriptableObject para usarlo en la partida sin modificar el asset original
        shopInventoryRuntime = Instantiate(shopInventoryAsset);
    }
}
