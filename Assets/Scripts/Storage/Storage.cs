using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    protected const string NutCountKey = "Nuts";
    protected const string CheckPointsKey = "CheckPoints";
    protected const string AchievementsKey = "Achievements";
    protected const string BestDistanceKey = "BestDistance";
    protected const string BestCollectedNutsKey = "BestCollectedNuts";
    protected const string GeneralAudioActivityKey = "AudioActivity";
    protected const string FirstLoad = "FirstLoad";

    private List<CheckPointProperty> _checkPointPropertiesDefault;
    private List<AchievementProperties> _achievementPropertiesDefault;
    private Wallet _wallet;
    private CheckPointMap _checkPointMap;
    private Score _score;
    private GeneralAudioActivityToggler _generalAudio;
    private AchievementMap _achievementMap;
    private AchievementFactory _achievementFactory;
    private bool _isInited = false;

    public CheckPointMap CheckPointMap => _checkPointMap;
    public Wallet Wallet => _wallet;
    public Score Score => _score;
    public AchievementMap AchievementMap => _achievementMap;

    public int BestDistance => PlayerPrefs.GetInt(BestDistanceKey, 0);
    public int BestCollectedNuts => PlayerPrefs.GetInt(BestCollectedNutsKey, 0);
    public uint Nuts => (uint)PlayerPrefs.GetInt(NutCountKey, 0);
    public string CheckPoints => PlayerPrefs.GetString(CheckPointsKey);
    public string Achievements => PlayerPrefs.GetString(AchievementsKey);
    public bool GenrealAudioAcitivity => Convert.ToBoolean(PlayerPrefs.GetInt(GeneralAudioActivityKey, 0));
    public bool IsFirstLoad => Convert.ToBoolean(PlayerPrefs.GetInt(FirstLoad, 1));

    public void Init(Wallet wallet, CheckPointMap checkPointMap, Score score, AchievementMap achievementMap,
        List<CheckPointProperty> checkPointPropertiesDefault, List<AchievementProperties> achievementPropertiesDefault,
        GeneralAudioActivityToggler generalAudio, AchievementFactory achievementFactory)
    {
        _score = score;
        _wallet = wallet;
        _checkPointMap = checkPointMap;
        _checkPointPropertiesDefault = checkPointPropertiesDefault;
        _achievementPropertiesDefault = achievementPropertiesDefault;
        _generalAudio = generalAudio;
        _achievementFactory = achievementFactory;
        _achievementMap = achievementMap;

        _isInited = true;
    }

    public void Save()
    {
        if (_isInited == false)
            return;

        PlayerPrefs.SetInt(NutCountKey, (int)_wallet.NutCount);
        PlayerPrefs.SetInt(GeneralAudioActivityKey, Convert.ToInt32(_generalAudio.IsMuted));

        string output = JsonConvert.SerializeObject(_checkPointMap.CheckPointProperties);
        PlayerPrefs.SetString(CheckPointsKey, output);

        output = JsonConvert.SerializeObject(_achievementMap.AchievementProperties);
        PlayerPrefs.SetString(AchievementsKey, output);
    }

    public void Load()
    {
        _wallet.Init(Nuts);
        _generalAudio.Init(GenrealAudioAcitivity);

        if (IsFirstLoad)
        {
            _checkPointMap.Init(_checkPointPropertiesDefault, _wallet, this);
            _achievementMap.Init(_achievementPropertiesDefault, _wallet, _achievementFactory);

            PlayerPrefs.SetInt(FirstLoad, 0);
            PlayerPrefs.Save();
            return;
        }

        List<CheckPointProperty> checkPointProperties = JsonConvert.DeserializeObject<List<CheckPointProperty>>(CheckPoints);
        _checkPointMap.Init(checkPointProperties, _wallet, this);

        List<AchievementProperties> achievementProperties = JsonConvert.DeserializeObject<List<AchievementProperties>>(Achievements);
        _achievementMap.Init(achievementProperties, _wallet, _achievementFactory);
    }

    public void ResetState()
    {
        string output = JsonConvert.SerializeObject(_checkPointPropertiesDefault);
        PlayerPrefs.SetString(CheckPointsKey, output);

        output = JsonConvert.SerializeObject(_achievementPropertiesDefault);
        PlayerPrefs.SetString(Achievements, output);

        PlayerPrefs.SetInt(NutCountKey, 0);
        PlayerPrefs.SetInt(BestDistanceKey, 0);
        PlayerPrefs.SetInt(BestCollectedNutsKey, 0);
        PlayerPrefs.SetInt(GeneralAudioActivityKey, 0);
        PlayerPrefs.SetInt(FirstLoad, 1);
    }

    public void TestMode()
    {
        string output = JsonConvert.SerializeObject(_checkPointPropertiesDefault);
        PlayerPrefs.SetString(CheckPointsKey, output);

        output = JsonConvert.SerializeObject(_achievementPropertiesDefault);
        PlayerPrefs.SetString(Achievements, output);

        PlayerPrefs.SetInt(NutCountKey, 10000);
        PlayerPrefs.SetInt(BestDistanceKey, 10000);
        PlayerPrefs.SetInt(FirstLoad, 1);
    }

    public void SaveBestDistance()
    {
        PlayerPrefs.SetInt(BestDistanceKey, (int)_score.Distance);
    }

    public void SaveBestCollectedNuts()
    {
        PlayerPrefs.SetInt(BestCollectedNutsKey, (int)_score.NutCount);
    }
}