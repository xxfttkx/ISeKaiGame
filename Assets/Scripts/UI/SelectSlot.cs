using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSlot : MonoBehaviour
{
    public Image playerImage;

    public void SetImage(int charIndex)
    {
        if(charIndex==-1)
        {
            playerImage.gameObject.SetActive(false);
        }
        else
        {
            playerImage.sprite = SOManager.Instance.characterDataList_SO.characters[charIndex].sprite;
            playerImage.gameObject.SetActive(true);
        }
        
    }
}
