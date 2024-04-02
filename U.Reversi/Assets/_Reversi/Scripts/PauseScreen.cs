using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PauseScreen : Screen
{
    [SerializeField] private AudioSource onPressedAudio;
    [SerializeField] private Screen howToPlayScreen;

    private void Start()
    {
        if (onPressedAudio == null)
            throw new NullReferenceException();
        if (howToPlayScreen == null)
            throw new NullReferenceException();
    }
    public void OnResumeButtonPressed()
    {
        onPressedAudio.Play();
        Disable();
    }

    public void OnHowToPlayPressed()
    {
        onPressedAudio.Play();
        ScreenTransition(howToPlayScreen);
    }

    public void OnExitPressed()
    {
        onPressedAudio.Play();
        SceneManager.LoadScene(0);
    }
}
