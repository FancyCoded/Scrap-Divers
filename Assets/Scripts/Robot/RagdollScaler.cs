using System.Collections;
using UnityEngine;

public class RagdollScaler : MonoBehaviour
{
    [SerializeField] private float _maxDeltaOfGrowth = 0.1f;
    [SerializeField] private float _maxDeltaOfDecline = 0.01f;

    private CharacterJoint[] _characterJoints;
    private Vector3[] _connectedAnchor;
    private Vector3[] _anchor;
    private Vector3 _defaultScale;

    private IEnumerator _scale;

    private void Awake()
    {
        _defaultScale = transform.localScale;
        _characterJoints = GetComponentsInChildren<CharacterJoint>();
        _connectedAnchor = new Vector3[_characterJoints.Length];
        _anchor = new Vector3[_characterJoints.Length];

        for (int i = 0; i < _characterJoints.Length; i++)
        {
            _connectedAnchor[i] = _characterJoints[i].connectedAnchor;
            _anchor[i] = _characterJoints[i].anchor;
        }
    }

    public void ChangeScaleFor(float duration, float scale)
    {
        if (_scale != null)
            StopCoroutine(_scale);

        _scale = Scale(scale, duration);
        StartCoroutine(_scale);
    }

    public void CorrectCharacterJoints()
    {
        for (int i = 0; i < _characterJoints.Length; i++)
        {
            _characterJoints[i].connectedAnchor = _connectedAnchor[i];
            _characterJoints[i].anchor = _anchor[i];
        }
    }

    public void ResetState()
    {
        if (_scale != null)
            StopCoroutine(_scale);

        transform.localScale = _defaultScale;
        CorrectCharacterJoints();
    }

    private IEnumerator Scale(float scale, float duration)
    {
        Vector3 targetScale = new Vector3(scale, scale, scale);
        WaitForSeconds seconds = new WaitForSeconds(duration);

        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, _maxDeltaOfGrowth);
            CorrectCharacterJoints();
            yield return null;
        }

        yield return seconds;

        while (transform.localScale != _defaultScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, _defaultScale, _maxDeltaOfDecline);
            CorrectCharacterJoints();
            yield return null;
        }
    }
}
