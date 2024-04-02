using UnityEngine;
using System;

public class MainScreen : Screen
{
    [SerializeField] private GameDataSO gameData;
    [SerializeField] private AudioSource onPressedAudio;
    [SerializeField] private Screen playScreen;
    [SerializeField] private Screen howToPlayScreen;

    private void Start()
    {
        if (gameData == null)
            throw new NullReferenceException();
        if (onPressedAudio == null)
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
        onPressedAudio.Play();
        ScreenTransition(playScreen);
    }

    public void OnHowToPlayPressed()
    {
        onPressedAudio.Play();
        ScreenTransition(howToPlayScreen);
    }

    public void OnExitPressed()
    {
        onPressedAudio.Play();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
