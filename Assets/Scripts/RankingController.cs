using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;

public class RankingController : MonoBehaviour
{
    public int ID;
    int MaxScores = 5;
    public Text[] entries;

    private void Start()
    {
        LootLockerSDKManager.StartSession("Player", (response) =>
        {
            if (response.success)
            {
                ShowScores();
            }
            else
            {

            }
        });
    }

    public void ShowScores()
    {
        LootLockerSDKManager.GetScoreList(ID, MaxScores, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;
                Debug.Log(scores.Length);

                for (int i = 0; i < scores.Length; i++)
                {
                    entries[i].text = (scores[i].rank + ". " + scores[i].member_id + " (" + scores[i].score + ")");
                }

                if (scores.Length < MaxScores)
                {
                    for(int i = scores.Length; i < MaxScores; i++)
                    {
                        entries[i].text = (i + 1).ToString() + ".    none";
                    }
                }
            }
            else
            {

            }
        });
    }
}
