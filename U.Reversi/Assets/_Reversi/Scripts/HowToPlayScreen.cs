using System;
using UnityEngine;

public class HowToPlayScreen : Screen
{
    [SerializeField] private Screen pauseScreen;

    private void Start()
    {
        if (pauseScreen == null)
            throw new NullReferenceException();
    }

    public void OnBackPressed()
    {
        ScreenTransition(pauseScreen);
    }
}
