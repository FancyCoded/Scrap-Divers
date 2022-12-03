using System.Collections;
using UnityEngine;

public class RagdollScaler : MonoBehaviour
{
    [SerializeField] float _maxDelta = 0.1f;

    private CharacterJoint[] _characterJoints;
    private Vector3[] _connectedAnchor;
    private Vector3[] _anchor;

    private IEnumerator _scale;

    private void Awake()
    {
        _characterJoints = GetComponentsInChildren<CharacterJoint>();
        _connectedAnchor = new Vector3[_characterJoints.Length];
        _anchor = new Vector3[_characterJoints.Length];

        for(int i = 0; i < _characterJoints.Length; i++)
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

    private IEnumerator Scale(float scale, float duration)
    {
        Vector3 defaultScale = transform.localScale;
        Vector3 targetScale = new Vector3(scale, scale, scale);
        WaitForSeconds seconds = new WaitForSeconds(duration);

        while(transform.localScale != targetScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, _maxDelta);
            CorrectCharacterJoints();
            yield return null;
        }


        yield return seconds;

        while (transform.localScale != defaultScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, defaultScale, _maxDelta / 2);
            CorrectCharacterJoints();
            yield return null;
        }
    }

    private void CorrectCharacterJoints()
    {
        for (int i = 0; i < _characterJoints.Length; i++)
        {
            _characterJoints[i].connectedAnchor = _connectedAnchor[i];
            _characterJoints[i].anchor = _anchor[i];
        }
    }
}
