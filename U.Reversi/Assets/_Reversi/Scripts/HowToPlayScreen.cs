using System;
using UnityEngine;

public class HowToPlayScreen : Screen
{
    [SerializeField] private AudioSource onPressedAudio;
    [SerializeField] private Screen pauseScreen;

    private void Start()
    {
        if (onPressedAudio == null)
            throw new NullReferenceException();
        if (pauseScreen == null)
            throw new NullReferenceException();
    }

    public void OnBackPressed()
    {
        onPressedAudio.Play();
        ScreenTransition(pauseScreen);
    }
}
