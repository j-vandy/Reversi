using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class Score : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    private void Start()
    {
        if (image == null)
            throw new NullReferenceException();
        if (text == null)
            throw new NullReferenceException();
    }

    public void SetScore(int score)
    {
        text.text = "" + score;
    }

    [Button]
    public void Highlight()
    {
        Color imgColor = image.color;
        imgColor.a = 1f;
        image.color = imgColor;

        Color txtColor = text.color;
        txtColor.a = 1f;
        text.color = txtColor;
    }

    [Button]
    public void Unhighlight()
    {
        Color imgColor = image.color;
        imgColor.a = 0.5f;
        image.color = imgColor;

        Color txtColor = text.color;
        txtColor.a = 0.5f;
        text.color = txtColor;
    }
}
