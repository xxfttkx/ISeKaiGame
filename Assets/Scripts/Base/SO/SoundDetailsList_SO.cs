using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDetailsList_SO", menuName = "Sound/SoundDetailsList")]
public class SoundDetailsList_SO : ScriptableObject
{
    public List<BGMDetails> bgmList;
    public List<SoundDetails> soundDetailsList;

    public SoundDetails GetSoundDetails(SoundName name)
    {
        return soundDetailsList.Find(s => s.soundName == name);
    }
    public BGMDetails GetBGMDetails(BGMName name)
    {
        return bgmList.Find(s => s.bgmName == name);
    }
}

