using UnityEngine;
using System.Collections;
using Agava.YandexGames;

public class SDK : MonoBehaviour
{
    [SerializeField] private Language _language;

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
        DontDestroyOnLoad(gameObject);

        yield return YandexGamesSdk.Initialize();
        yield return new WaitForSecondsRealtime(1);

        while (true)
        {
            if (YandexGamesSdk.IsInitialized)
            {
                _language.Set(YandexGamesSdk.Environment.i18n.lang);
                yield break;
            }

            yield return new WaitForSecondsRealtime(1);
        }
    }
}