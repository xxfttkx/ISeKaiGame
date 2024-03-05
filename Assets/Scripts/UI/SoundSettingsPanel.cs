using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsPanel : Singleton<SoundSettingsPanel>
{
    public Slider totalVolume;
    public Slider ambientVolume;
    public Slider effectVolume;
    public TextMeshProUGUI total;
    public TextMeshProUGUI ambient;
    public TextMeshProUGUI effect;

    protected override void Awake()
    {
        base.Awake();
        totalVolume.onValueChanged.AddListener(value => OnSliderValueChanged(value, 0));
        ambientVolume.onValueChanged.AddListener(value => OnSliderValueChanged(value, 1));
        effectVolume.onValueChanged.AddListener(value => OnSliderValueChanged(value, 2));
    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    public void Init()
    {
        OnSliderValueChanged(SaveLoadManager.Instance.GetVolume(0), 0);
        OnSliderValueChanged(SaveLoadManager.Instance.GetVolume(1), 1);
        OnSliderValueChanged(SaveLoadManager.Instance.GetVolume(2), 2);
    }
    public void OnSliderValueChanged(float val,int index)
    {
        AudioManager.Instance.SetVolume(val, index);
        if(index==0) total.text = Mathf.RoundToInt(val * 100f) + "%";
        else if (index==1) ambient.text = Mathf.RoundToInt(val * 100f) + "%";
        else if (index==2) effect.text = Mathf.RoundToInt(val * 100f) + "%";
    }

    public void HideSelf()
    {
        this.gameObject.SetActive(false);
    }
}
