using System.Collections.Generic;
using UnityEngine;

public class FramePool : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private List<Frame> _pool = new List<Frame>();
    private Frame _currentLevelFrame;

    public virtual void Init(LevelProperites levelProperites, uint initialCount)
    {
        _currentLevelFrame = levelProperites.Frame;

        for(int i = 0; i < initialCount; i++)
            Create(levelProperites.Frame);
    }

    public Frame GetFrame()
    {
        for(int i = 0; i < _pool.Count; i++)
        {
            if (_pool[i].gameObject.activeSelf == false)
                return _pool[i];
        }

        return Create(_currentLevelFrame);
    }

    public Frame Create(Frame template)
    {
        Frame instance = Instantiate(template, _container);

        instance.gameObject.SetActive(false);
        _pool.Add(instance);
        return instance;
    }

    public void ResetState()
    {
        for (int i = 0; i < _container.childCount; i++)
            Destroy(_container.GetChild(i).gameObject);

        _pool.Clear();
        _currentLevelFrame = null;
    }
}