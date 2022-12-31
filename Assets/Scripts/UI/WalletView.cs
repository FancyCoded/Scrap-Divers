using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nutCount;

    public void OnNutCountChanged(uint nutCount)
    {
        _nutCount.text = nutCount.ToString();
    }
}
