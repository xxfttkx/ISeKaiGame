using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsPanel : MonoBehaviour
{
    public Slider totalVolume;
    public Slider ambientVolume;
    public Slider effectVolume;
    public TextMeshProUGUI total;
    public TextMeshProUGUI ambient;
    public TextMeshProUGUI effect;
    private bool bInit = false;

    protected void Awake()
    {
        
    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    public void Init()
    {
        if(!bInit)
        {
            bInit = true;
            totalVolume.onValueChanged.AddListener(value => OnSliderValueChanged(value, 0));
            ambientVolume.onValueChanged.AddListener(value => OnSliderValueChanged(value, 1));
            effectVolume.onValueChanged.AddListener(value => OnSliderValueChanged(value, 2));
        }
        totalVolume.value = SaveLoadManager.Instance.GetVolume(0);
        ambientVolume.value = SaveLoadManager.Instance.GetVolume(1);
        effectVolume.value = SaveLoadManager.Instance.GetVolume(2);
    }
    public void OnSliderValueChanged(float val,int index)
    {
        AudioManager.Instance.SetVolume(val, index);
        SaveLoadManager.Instance.SetVolume(val, index);
        if(index==0) total.text = Mathf.RoundToInt(val * 100f) + "%";
        else if (index==1) ambient.text = Mathf.RoundToInt(val * 100f) + "%";
        else if (index==2) effect.text = Mathf.RoundToInt(val * 100f) + "%";
    }
}
