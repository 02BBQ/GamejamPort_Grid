using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{

    public Image m_Image;

    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;

    private int m_IndexSprite;

    private bool playing = false;

    private void OnEnable()
    {
        playing = true;
        StartCoroutine("Func_PlayAnimUI");
    }

    private void OnDisable()
    {
        playing = false;
        StopCoroutine("Func_PlayAnimUI");
    } 
    IEnumerator Func_PlayAnimUI()
    {
        while (playing)
        {
            yield return new WaitForSecondsRealtime(m_Speed);
            if (m_IndexSprite >= m_SpriteArray.Length)
            {
                m_IndexSprite = 0;
            }
            m_Image.sprite = m_SpriteArray[m_IndexSprite];
            m_IndexSprite += 1;
        }
    }
}