using UnityEngine;
using System.Collections;
using Agava.YandexGames;

public class SDK : MonoBehaviour
{
    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        DontDestroyOnLoad(gameObject);

        yield return YandexGamesSdk.Initialize();
    }
}