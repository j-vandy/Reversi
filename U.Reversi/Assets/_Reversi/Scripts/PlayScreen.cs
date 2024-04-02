using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScreen : Screen
{
    [SerializeField] private AudioSource onPressedAudio;
    [SerializeField] private Screen mainScreen;
    [SerializeField] private Screen playAIScreen;

    private void Start()
    {
        if (onPressedAudio == null)
            throw new NullReferenceException();
        if (mainScreen == null)
            throw new NullReferenceException();
        if (playAIScreen == null)
            throw new NullReferenceException();
    }

    public void OnPlayAIPressed()
    {
        onPressedAudio.Play();
        ScreenTransition(playAIScreen);
    }

    public void OnLocalPlayPressed()
    {
        onPressedAudio.Play();
        SceneManager.LoadScene(1);
    }

    public void OnBackPressed()
    {
        onPressedAudio.Play();
        ScreenTransition(mainScreen);
    }
}
