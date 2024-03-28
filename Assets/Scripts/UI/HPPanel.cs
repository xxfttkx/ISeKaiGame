using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPanel : MonoBehaviour
{
    public GameObject prefab;
    public Queue<ChangeHPUI> changeHPUIs;

    private void Start()
    {
        changeHPUIs = new Queue<ChangeHPUI>();  
    }
    private void OnEnable()
    {
        EventHandler.PlayerHurtEnemyEvent += OnPlayerHurtEnemyEvent;
    }
    private void OnDisable()
    {
        EventHandler.PlayerHurtEnemyEvent -= OnPlayerHurtEnemyEvent;
    }
    void OnPlayerHurtEnemyEvent(int atkIndex, EnemyBase e, int atk)
    {
        ChangeHPUI ui;
        if (changeHPUIs.Count > 0)
        {
            ui = changeHPUIs.Dequeue();
        }
        else
        {
            var go = Instantiate(prefab, this.transform);
            ui = go.GetComponent<ChangeHPUI>();
        }
        ui.gameObject.SetActive(true);
        ui.Init(e, atk, ()=>Release(ui));
    }
    public void Release(ChangeHPUI ui)
    {
        changeHPUIs.Enqueue(ui);
        ui.gameObject.SetActive(false);
    }
}
