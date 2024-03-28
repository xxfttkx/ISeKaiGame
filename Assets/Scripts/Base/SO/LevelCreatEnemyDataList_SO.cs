using UnityEngine;

[CreateAssetMenu(fileName = "LevelCreatEnemyDataList_SO", menuName = "Custom/LevelCreatEnemyDataList_SO", order = 3)]
public class LevelCreatEnemyDataList_SO : ScriptableObject
{
    public LevelCreatEnemy[] levels;
    public LevelCreatEnemy GetLevelByIndex(int index)
    {
        if (index > levels.Length || index < 0)
        {
            index = 0;
            Debug.Log("should not reach here");
        }
        return levels[index];
    }
}
