using System.Collections.Generic;

[System.Serializable]
public class RankEntry
{
    public string playerName;
    public int wave;
    public int score;

    public RankEntry(string playerName, int wave, int score)
    {
        this.playerName = playerName;
        this.wave = wave;
        this.score = score;
    }
}

[System.Serializable]
public class RankList
{
    public List<RankEntry> entries = new List<RankEntry>();
}