using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public enum CoinType { Touch, Hook }
    public CoinType coinType = CoinType.Touch;

    public int coinValue = 1;

    private bool collected = false;

    // Called when player physically touches the coin
    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (coinType == CoinType.Touch && other.CompareTag("Player"))
        {
            Collect();
        }
    }

    // Called by hook (via script) when it's hit by the hook
    public void CollectByHook()
    {
        if (collected) return;

        if (coinType == CoinType.Hook)
        {
            Collect();
        }
    }

    void Collect()
    {
        collected = true;

        // TODO: Add score logic here (e.g., call GameManager or UIController)
        Debug.Log($"Collected coin worth {coinValue}");

        // You could play a sound, particle effect, etc. here

        Destroy(gameObject);
    }
}
