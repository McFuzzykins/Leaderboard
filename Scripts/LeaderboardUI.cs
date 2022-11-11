using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject lbCanvas;
    public GameObject[] lbEntries;

    public static LeaderboardUI instance;

    void Awake()
    {
        lbCanvas.SetActive(false);
        instance = this;
    }

    public void OnLoggedIn()
    {
        lbCanvas.SetActive(true);
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        GetLeaderboardRequest getlbReq = new GetLeaderboardRequest
        {
            StatisticName = "FastestTIme",
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(getlbReq,
            result =>
            {
                UpdateLeaderboardUI(result.Leaderboard);
            },
            error =>
            {
                Debug.Log(error.ErrorMessage);
            }
        );
    }

    void UpdateLeaderboardUI (List<PlayerLeaderboardEntry> leaderboard)
    {
        for(int i = 0; i < lbEntries.Length; i++)
        {
            lbEntries[i].SetActive(i < leaderboard.Count);
            if(i >= leaderboard.Count)
            {
                continue;
            }
            lbEntries[i].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = (leaderboard[i].Position + 1) + "." + leaderboard[i].DisplayName;
            lbEntries[i].transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = (-(float)leaderboard[i].StatValue * 0.001f).ToString();
        }
    }

    public void SetLeaderboardEntry(int newScore)
    {
        bool useLegacyMenthod = false;
        if (useLegacyMenthod)
        {
            ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
            {
                FunctionName = "UpdateHighScore",
                FunctionParameter = new { score = newScore }
            };

            PlayFabClientAPI.ExecuteCloudScript(request,
                result =>
                {
                    Debug.Log(result);
                    //Debug.Log("Success");
                    //Debug.Log(result.FunctionName);
                    //Debug.Log(result.FunctionResult);
                    //Debug.Log(result.FunctionResultTooLarge);
                    //Debug.Log(result.Error);
                    DisplayLeaderboard();
                    Debug.Log(result.ToJson());
                },
                error =>
                {
                    Debug.Log(error.ErrorMessage);
                    Debug.Log("Error");
                }
            );
        }
        else
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = "FastestTime",
                        Value = newScore
                    },
                }
            },
                result =>
                {
                    Debug.Log("User Stats Updated");
                },
                error =>
                {
                    Debug.LogError(error.GenerateErrorReport());
                }
            );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
