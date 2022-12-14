using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    protected const string NutCountKey = "Nuts";
    protected const string CheckPointsKey = "CheckPoints";
    protected const string BestDistanceKey = "BestDistance";
    protected const string BestCollectedNutsKey = "BestCollectedNuts";
    protected const string GeneralAudioActivityKey = "AudioActivity";

    private List<CheckPointProperty> _checkPointPropertiesDefault;
    private Wallet _wallet;
    private CheckPointMap _checkPointMap;
    private Score _score;
    private GeneralAudioActivityToggler _generalAudio;
    private bool IsInited = false;

    public CheckPointMap CheckPointMap => _checkPointMap;
    public Wallet Wallet => _wallet;
    public Score Score => _score;
    public IReadOnlyList<IReadonlyCheckPointProperty> CheckPointPropertiesDefault => _checkPointPropertiesDefault;

    public int BestDistance => PlayerPrefs.GetInt(BestDistanceKey) | 0;
    public int BestCollectedNuts => PlayerPrefs.GetInt(BestCollectedNutsKey) | 0;
    public int GenrealAudioAcitivity => PlayerPrefs.GetInt(GeneralAudioActivityKey) | 0;
    public uint Nuts => (uint) PlayerPrefs.GetInt(NutCountKey) | 0;
    public string CheckPoints => PlayerPrefs.GetString(CheckPointsKey);

    public void Init(Wallet wallet, CheckPointMap checkPointMap, Score score, 
        List<CheckPointProperty> checkPointPropertiesDefault, GeneralAudioActivityToggler generalAudio)
    {
        _score = score;
        _wallet = wallet;
        _checkPointMap = checkPointMap;
        _checkPointPropertiesDefault = checkPointPropertiesDefault;
        _generalAudio = generalAudio;

        IsInited = true;
    }

    public void Save()
    {
        if (IsInited == false)
            return;

        PlayerPrefs.SetInt(NutCountKey, (int)_wallet.NutCount);
        PlayerPrefs.SetInt(GeneralAudioActivityKey, Convert.ToInt32(_generalAudio.IsMuted));

        string output = JsonConvert.SerializeObject(CheckPointMap.CheckPointProperties);
        PlayerPrefs.SetString(CheckPointsKey, output);
    }

    public void Load()
    {
        Wallet.Init(Nuts);
        _generalAudio.Init(Convert.ToBoolean(GenrealAudioAcitivity));

        List<CheckPointProperty> checkPointProperties = JsonConvert.DeserializeObject<List<CheckPointProperty>>(CheckPoints);

        if (checkPointProperties == null)
            CheckPointMap.Init((List<CheckPointProperty>)CheckPointPropertiesDefault, Wallet, this);
        else
            CheckPointMap.Init(checkPointProperties, Wallet, this);
    }

    public void ResetState()
    {
        string output = JsonConvert.SerializeObject(_checkPointPropertiesDefault);

        PlayerPrefs.SetString(CheckPointsKey, output);
        PlayerPrefs.SetInt(NutCountKey, 0);
        PlayerPrefs.SetInt(BestDistanceKey, 0);
        PlayerPrefs.SetInt(BestCollectedNutsKey, 0);
        PlayerPrefs.SetInt(GeneralAudioActivityKey, 0);
    }

    public void TestMode()
    {
        string output = JsonConvert.SerializeObject(_checkPointPropertiesDefault);

        PlayerPrefs.SetString(CheckPointsKey, output);
        PlayerPrefs.SetInt(NutCountKey, 10000);
        PlayerPrefs.SetInt(BestDistanceKey, 10000);
        PlayerPrefs.SetInt(BestCollectedNutsKey, 0);
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