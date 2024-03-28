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

        // ����������ڵĵ�ͼ�������
        int playerTileX = Mathf.FloorToInt(playerPosition.x / tileX);
        int playerTileY = Mathf.FloorToInt(playerPosition.y / tileY);

        // �����µĵ�ͼ��
        for (int x = playerTileX - 1; x <= playerTileX + 1; x++)
        {
            for (int y = playerTileY - 1; y <= playerTileY + 1; y++)
            {
                Vector2 tilePosition = new Vector2(x * tileX, y * tileY);

                // ����Ƿ��Ѿ����ɹ��õ�ͼ��
                if (!IsTileGenerated(tilePosition))
                {
                    // �����µĵ�ͼ��
                    GameObject newTile = Instantiate(mapTilePrefab, tilePosition, Quaternion.identity,this.transform );
                    // ������������õ�ͼ������ݺ���������

                    // ��¼�����ɵĵ�ͼ��
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
        // �������¼�����ɵĵ�ͼ��
        // ����ʹ���ֵ䡢�б�����ݽṹ����¼�����ɵĵ�ͼ��
        tileHashMap.Add(tilePosition, tileObject);
    }
}
