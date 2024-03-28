using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataList_SO", menuName = "Custom/EnemyDataList_SO", order = 2)]
public class EnemyDataList_SO : ScriptableObject
{
    public Enemy[] enemies;
    public Enemy GetEnemyByIndex(int index)
    {
        return enemies[index];
    }
}
