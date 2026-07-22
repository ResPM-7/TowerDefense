using System.IO;
using System.Linq;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "ranking.json");
    }

    public RankList LoadRanking()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            return JsonUtility.FromJson<RankList>(json);
        }
        else
        {
            //없으면 새로 만들기
            return new RankList();
        }
    }

    public void AddRankAndSave(int wave, int score)
    {
        RankList currentRanking = LoadRanking();

        currentRanking.entries.Add(new RankEntry( wave, score));

        //상위 10개만 자르기
        currentRanking.entries = currentRanking.entries
            .OrderByDescending(x => x.score)
            .Take(10)
            .ToList();

        string json = JsonUtility.ToJson(currentRanking, true);

        File.WriteAllText(savePath, json);

        //Debug.Log(savePath);
    }
}
