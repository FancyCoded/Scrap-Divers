using Lean.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Language : MonoBehaviour
{
    [SerializeField] private LeanLocalization _leanLocalization;

    private Dictionary<string, string> _languages = new()
    {
        { "ru", "Russian" },
        { "en", "English" },
        { "tr", "Turkish" },
    };

    public event Action<string> LanguageChanged;

    public void Set(string language)
    {
        if (_languages.ContainsKey(language))
        {
            _leanLocalization.SetCurrentLanguage(_languages[language]);
            LanguageChanged?.Invoke(_languages[language]);
        }
    }

    [ContextMenu("Ru")]
    public void SetRu()
    {
        _leanLocalization.SetCurrentLanguage(_languages["ru"]);
        LanguageChanged?.Invoke(_languages["ru"]);
    }

    [ContextMenu("En")]
    public void SetEn()
    {
        _leanLocalization.SetCurrentLanguage(_languages["en"]);
        LanguageChanged?.Invoke(_languages["en"]);
    }

    [ContextMenu("Tr")]
    public void SetTr()
    {
        _leanLocalization.SetCurrentLanguage(_languages["tr"]);
        LanguageChanged?.Invoke(_languages["tr"]);
    }
}