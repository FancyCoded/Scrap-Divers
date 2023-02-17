#pragma warning disable

using Agava.YandexGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LeaderBoardWindow : MonoBehaviour
{
    private const string LeaderBoardName = "LeaderBoard";
    private const string Anonymous = "Anonymous";

    [SerializeField] private PlayerRankView _playerRankViewTemplate;
    [SerializeField] private RectTransform _content;
    [SerializeField] private uint _topPlayersCount = 5;
    [SerializeField] private uint _competingPlayersCount = 4;
    [SerializeField] private MenuStorageComposition _storageComposition;
    [SerializeField] private Button _back;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide();       
    }

    private void OnEnable()
    {
        _back.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _back.onClick.RemoveListener(Hide);
    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        yield break;
#endif
        while (true)
        {
            if(YandexGamesSdk.IsInitialized)
            {
                if(_storageComposition.Storage.BestDistance == 0)
                    Leaderboard.SetScore(LeaderBoardName, 0);

                yield break;
            }

            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void Display()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;

#if UNITY_WEBGL && !UNITY_EDITOR
        ResetState();
        CreateLeaderBoard();
#endif
    }

    private void Hide()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
    }

    private void ResetState()
    {
        for (int i = 0; i < _content.childCount; i++)
            Destroy(_content.GetChild(i).gameObject);
    }

    private void CreateLeaderBoard()
    {
        Leaderboard.GetEntries(LeaderBoardName, OnGetEntriesSucceeded, null, (int)_topPlayersCount, (int)_competingPlayersCount);
    }

    private void OnGetEntriesSucceeded(LeaderboardGetEntriesResponse result)
    {
        foreach (var entry in result.entries)
        {
            string name = entry.player.publicName;

            if (string.IsNullOrEmpty(name))
                name = Anonymous;

            PlayerRankView playerRankView = Instantiate(_playerRankViewTemplate, _content);
            playerRankView.Init(entry.rank + ". " + name, entry.score);
        }
    }
}
