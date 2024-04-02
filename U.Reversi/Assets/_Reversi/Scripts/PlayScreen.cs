using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScreen : Screen
{
    [SerializeField] private Screen mainScreen;
    [SerializeField] private Screen playAIScreen;

    private void Start()
    {
        if (mainScreen == null)
            throw new NullReferenceException();
        if (playAIScreen == null)
            throw new NullReferenceException();
    }

    public void OnPlayAIPressed()
    {
        ScreenTransition(playAIScreen);
    }

    public void OnLocalPlayPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void OnBackPressed()
    {
        ScreenTransition(mainScreen);
    }
}
