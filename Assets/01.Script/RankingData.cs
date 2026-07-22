using System.Collections.Generic;

[System.Serializable]
public class RankEntry
{
    public int wave;
    public int score;

    public RankEntry(int wave, int score)
    {
        this.wave = wave;
        this.score = score;
    }
}

[System.Serializable]
public class RankList
{
    public List<RankEntry> entries = new List<RankEntry>();
}