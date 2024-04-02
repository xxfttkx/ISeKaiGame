using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("音乐数据库")]
    public SoundDetailsList_SO soundDetailsList_SO;
    [Header("Audio Source")]
    public AudioSource ambientSource;
    public AudioSource gameSource;
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;
    private void OnEnable()
    {
        EventHandler.LoadFinishEvent += OnLoadFinishEvent;
        EventHandler.EnterDungeonEvent += OnEnterDungeonEvent;
        EventHandler.ExitDungeonEvent += OnExitDungeonEvent;
    }

    private void OnDisable()
    {
        EventHandler.LoadFinishEvent -= OnLoadFinishEvent;
        EventHandler.EnterDungeonEvent -= OnEnterDungeonEvent;
        EventHandler.ExitDungeonEvent -= OnExitDungeonEvent;
    }

    public SoundDetails GetSoundDetailsBySoundName(SoundName soundName)
    {
        return soundDetailsList_SO.GetSoundDetails(soundName);
    }
    public void PlaySoundEffect(SoundName soundName)
    {
        var soundDetails = GetSoundDetailsBySoundName(soundName);
        if (soundDetails != null)
            PoolManager.Instance.PlaySoundEffect(soundDetails);
    }
    public void PlaySoundAmbient(BGMName bgmName)
    {
        var d = soundDetailsList_SO.GetBGMDetails(bgmName);
        if (d != null)
        {
            ambientSource.clip = d.soundClip;
            if (ambientSource.isActiveAndEnabled)
                ambientSource.Play();
        }
    }

    public void SetVolume(float value,int index)
    {
        /*// 使用指数函数进行映射
        value = Mathf.Pow(value, 2f); // 2 可以调整为其他指数，根据需要调整映射的形状
        // 将映射后的值转换到输出范围
        float result = Mathf.Lerp(-80, 20, value);*/
        value = 0.1f * value;
        float val = Mathf.Clamp(value, 0.00001f, 1.0f);
        float result = (20 + 20 * Mathf.Log10(val));
        string s = GetStringForNumber(index);
        audioMixer.SetFloat(s, result);
        
    }
    public static string GetStringForNumber(int number)
    {
        return number switch
        {
            0 => "Master",
            1 => "Ambient",
            2 => "Effect",
            _ => "Master",
        };
    }
    public void SetTotalVolume(float value)
    {
        value = 0.1f * value;
        float val = Mathf.Clamp(value, 0.00001f, 1.0f);
        int result = (int)(20 + 20 * Mathf.Log10(val));
        audioMixer.SetFloat("Master", result);
    }
    public void SetAmbientVolume(float value)
    {
        value = 0.1f * value;
        float val = Mathf.Clamp(value, 0.00001f, 1.0f);
        int result = (int)(20 + 20 * Mathf.Log10(val));
        audioMixer.SetFloat("Ambient", result);
    }
    public void SetEffectVolume(float value)
    {
        value = 0.1f * value;
        float val = Mathf.Clamp(value, 0.00001f, 1.0f);
        int result = (int)(20 + 20 * Mathf.Log10(val));
        audioMixer.SetFloat("Effect", result);
    }

    private void OnLoadFinishEvent()
    {
        SetVolume(SaveLoadManager.Instance.GetVolume(0), 0);
        SetVolume(SaveLoadManager.Instance.GetVolume(1), 1);
        SetVolume(SaveLoadManager.Instance.GetVolume(2), 2);
        PlaySoundAmbient(BGMName.Fantasy);
    }
    void OnEnterDungeonEvent(List<int> _)
    {
        PlaySoundAmbient(BGMName.Battle);
    }
    void OnExitDungeonEvent()
    {
        PlaySoundAmbient(BGMName.Fantasy);
    }
}
