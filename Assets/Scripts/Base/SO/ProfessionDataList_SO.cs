using UnityEngine;

[CreateAssetMenu(fileName = "ProfessionDataList_SO", menuName = "Custom/ProfessionDataList_SO", order = 1)]
public class ProfessionDataList_SO : ScriptableObject
{
    public ProfessionData[] professionDataList;
    public ProfessionData GetProfessionDataByProfession(Profession p)
    {
        return professionDataList[(int)p];
    }
}
