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
            playerImage.sprite = SOManager.Instance.GetPlayerSpriteByIndex(charIndex);
            playerImage.gameObject.SetActive(true);
        }
        
    }
}
