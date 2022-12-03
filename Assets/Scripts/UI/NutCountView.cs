using TMPro;
using UnityEngine;

public class NutCountView : MonoBehaviour
{
    [SerializeField] private TMP_Text _count;
    [SerializeField] private Collector _collector;

    private void OnEnable()
    {
        _collector.NutCountChanged += OnCountChanged;
    }

    private void OnDisable()
    {
        _collector.NutCountChanged -= OnCountChanged;
    }

    private void OnCountChanged(uint count)
    {
        _count.text = count.ToString();
    }
}
