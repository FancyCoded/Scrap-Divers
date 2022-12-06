using System.Collections.Generic;
using UnityEngine;

public class FrameSpawner : FramePool, IResetable
{
    private const uint FrameScaleZ = 100;

    private List<Frame> _activeFrames = new List<Frame>();
    private Vector3 _frameSpawnStartPosition;

    public IReadOnlyList<Frame> ActiveFrames => _activeFrames;
    public Vector3 LastFramePosition => _activeFrames[_activeFrames.Count - 1].transform.position;
    public float FrameCenter => FrameScaleZ / 2;
    public float LastFrameOriginZ => LastFramePosition.z - FrameCenter;
    public float LastFrameEndZ => LastFramePosition.z + FrameCenter;

    public void Init(LevelProperties levelProperites, uint initialCount, uint spawnStartPositionZ)
    {
        ResetState();

        Init(levelProperites, initialCount);
        _frameSpawnStartPosition = new Vector3(0, 0,  spawnStartPositionZ + FrameCenter);
    }

    public void Spawn()
    {
        Frame frame = GetFrame();
        Vector3 position;

        if(_activeFrames.Count == 0 && _frameSpawnStartPosition != Vector3.zero)
            position = _frameSpawnStartPosition;
        else 
            position = LastFramePosition + Vector3.forward * FrameScaleZ;

        _activeFrames.Add(frame);
        frame.transform.SetPositionAndRotation(position, Quaternion.identity);
        frame.gameObject.SetActive(true);
    }

    public void TryDeleteFirst(uint initialFramesCount)
    {
        if (_activeFrames.Count <= initialFramesCount)
            return;

        Frame first = _activeFrames[0];

        first.gameObject.SetActive(false);
        _activeFrames.Remove(first);
    }

    public new void ResetState()
    {
        _activeFrames.Clear();
        base.ResetState();
    }
}
