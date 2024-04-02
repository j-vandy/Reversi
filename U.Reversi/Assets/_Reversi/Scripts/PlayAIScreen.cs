using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAIScreen : Screen
{
    [SerializeField] private GameDataSO gameData;
    [SerializeField] private AudioSource onPressedAudio;
    [SerializeField] private Screen playScreen;
    [SerializeField] private TMP_Dropdown difficulty;
    [SerializeField] private TMP_Dropdown color;

    private void Start()
    {
        if (gameData == null)
            throw new NullReferenceException();
        if (onPressedAudio == null)
            throw new NullReferenceException();
        if (playScreen == null)
            throw new NullReferenceException();
        if (difficulty == null)
            throw new NullReferenceException();
        if (color == null)
            throw new NullReferenceException();
    }

    public void OnPlayPressed()
    {
        onPressedAudio.Play();
        gameData.bAIEnabled = true;
        SceneManager.LoadScene(1);
    }

    public void OnDifficultyDropdownChanged(Int32 value)
    {
        onPressedAudio.Play();
        gameData.AIDifficulty = value;
    }

    public void OnColorDropdownChanged(Int32 value)
    {
        onPressedAudio.Play();
        if (value == 0)
            gameData.bAIIsWhite = Mathf.RoundToInt(UnityEngine.Random.Range(0f,1f)) == 0 ? true : false;
        else if (value == 1)
            gameData.bAIIsWhite = true;
        else
            gameData.bAIIsWhite = false;
    }

    public void OnBackPressed()
    {
        onPressedAudio.Play();
        difficulty.value = 0;
        color.value = 0;
        gameData.bAIEnabled = false;
        ScreenTransition(playScreen);
    }
}
