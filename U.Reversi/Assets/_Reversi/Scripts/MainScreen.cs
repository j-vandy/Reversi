using UnityEngine;
using System;

public class MainScreen : Screen
{
    [SerializeField] private GameDataSO gameData;
    [SerializeField] private Screen playScreen;
    [SerializeField] private Screen howToPlayScreen;

    private void Start()
    {
        if (gameData == null)
            throw new NullReferenceException();
        if (playScreen == null)
            throw new NullReferenceException();
        if (howToPlayScreen == null)
            throw new NullReferenceException();
        gameData.bAIEnabled = false;
        gameData.bAIIsWhite = false;
        gameData.AIDifficulty = 0;
    }
    public void OnPlayPressed()
    {
        ScreenTransition(playScreen);
    }

    public void OnHowToPlayPressed()
    {
        ScreenTransition(howToPlayScreen);
    }

    public void OnExitPressed()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
