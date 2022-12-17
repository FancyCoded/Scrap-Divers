using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _damage = 5;
    [SerializeField] private List<Transform> _itemSpawnPoints;

    private float _itemSpawnChance;

    public float Damage => _damage;

    private void Awake()
    {
        int minSpawnChance = 0;
        int maxSpawnChance = 60;

        _itemSpawnChance = Random.Range(minSpawnChance, maxSpawnChance);
    }

    public List<Transform> ItemSpawnPoints => _itemSpawnPoints;
    public float ItemSpawnChance => _itemSpawnChance;
}