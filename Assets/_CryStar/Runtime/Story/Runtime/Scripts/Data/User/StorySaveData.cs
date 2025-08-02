using System;

/// <summary>
/// ストーリーのセーブデータ用クラス
/// </summary>
[Serializable]
public class StorySaveData
{
    public int PartId;
    public int ChapterId;
    public int SceneId;

    public StorySaveData(int partId, int chapterId, int sceneId)
    {
        PartId = partId;
        ChapterId = chapterId;
        SceneId = sceneId;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is StorySaveData other)
        {
            return PartId == other.PartId && 
                   ChapterId == other.ChapterId && 
                   SceneId == other.SceneId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PartId, ChapterId, SceneId);
    }

    public override string ToString()
    {
        return $"Story({PartId}-{ChapterId}-{SceneId})";
    }
}