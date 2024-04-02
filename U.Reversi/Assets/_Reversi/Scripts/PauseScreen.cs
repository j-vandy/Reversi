using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PauseScreen : Screen
{
    [SerializeField] private Screen howToPlayScreen;

    private void Start()
    {
        if (howToPlayScreen == null)
            throw new NullReferenceException();
    }
    public void OnResumeButtonPressed()
    {
        Disable();
    }

    public void OnHowToPlayPressed()
    {
        ScreenTransition(howToPlayScreen);
    }

    public void OnExitPressed()
    {
        SceneManager.LoadScene(0);
    }
}
