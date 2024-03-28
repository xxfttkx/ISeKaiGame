using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGManager : Singleton<BGManager>
{
    public GameObject mapTilePrefab;
    public float tileX = 35.65f;
    public float tileY = 20.75f;
    public Dictionary<Vector2, GameObject> tileHashMap;

    protected override void Awake()
    {
        base.Awake();
        tileHashMap = new Dictionary<Vector2, GameObject>();
    }

    void Update()
    {
        GenerateMapTiles();
    }

    void GenerateMapTiles()
    {
        if (!GameStateManager.Instance.InGamePlay()) return;
        var p = PlayerManager.Instance.GetPlayerInControl();
        Vector2 playerPosition = p.transform.position;

        // 计算玩家所在的地图块的索引
        int playerTileX = Mathf.FloorToInt(playerPosition.x / tileX);
        int playerTileY = Mathf.FloorToInt(playerPosition.y / tileY);

        // 生成新的地图块
        for (int x = playerTileX - 1; x <= playerTileX + 1; x++)
        {
            for (int y = playerTileY - 1; y <= playerTileY + 1; y++)
            {
                Vector2 tilePosition = new Vector2(x * tileX, y * tileY);

                // 检查是否已经生成过该地图块
                if (!IsTileGenerated(tilePosition))
                {
                    // 生成新的地图块
                    GameObject newTile = Instantiate(mapTilePrefab, tilePosition, Quaternion.identity,this.transform );
                    // 在这里可以设置地图块的内容和其他属性

                    // 记录已生成的地图块
                    RecordGeneratedTile(tilePosition, newTile);
                }
            }
        }
    }

    bool IsTileGenerated(Vector2 tilePosition)
    {
        if (tileHashMap.ContainsKey(tilePosition)) return true;
        return false;
    }

    void RecordGeneratedTile(Vector2 tilePosition, GameObject tileObject)
    {
        // 在这里记录已生成的地图块
        // 可以使用字典、列表等数据结构来记录已生成的地图块
        tileHashMap.Add(tilePosition, tileObject);
    }
}
