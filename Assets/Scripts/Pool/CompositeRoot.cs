using UnityEngine;
using UnityEngine.UI;

public class CompositeRoot : MonoBehaviour
{
    [SerializeField] private Button _spawnButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private int _spawnOffset;
    [SerializeField] private ObstaclePool2 _cubePool;
    [SerializeField] private ObstacleSpawner2 _cubeSpawner; 

    public void Awake()
    {
        _spawnButton.onClick.AddListener(OnSpawnButtonClicked);
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        _cubePool.Init();
        //_cubeSpawner.Init(_cubePool);
    }

    private void OnDeleteButtonClicked()
    {
        _cubeSpawner.ReleaseFirst(1);
    }

    private void OnSpawnButtonClicked()
    {
        //_cubeSpawner.Spawn(new Vector3(0, 0, _spawnOffset));
    }
}