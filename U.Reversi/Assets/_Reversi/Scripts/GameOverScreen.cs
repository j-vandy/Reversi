using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameOverScreen : Screen
{
    [SerializeField] private AudioSource onPressedAudio;
    [SerializeField] private Board board;
    [SerializeField] private GameObject white;
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject draw;

    private void OnEnable() => board.OnGameOver += Enable;
    private void OnDisable() => board.OnGameOver -= Enable;

    private void Start()
    {
        if (onPressedAudio == null)
            throw new NullReferenceException();
        if (board == null)
            throw new NullReferenceException();
        if (white == null)
            throw new NullReferenceException();
        if (black == null)
            throw new NullReferenceException();
        if (draw == null)
            throw new NullReferenceException();
    }

    public override void Enable()
    {
        // enable all items but the title
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if (obj == white || obj == black || obj == draw)
                continue;
            obj.SetActive(true);
        }

        // enable the correct title
        int white_val = Board.WhiteValue(board.state);
        int black_val = Board.BlackValue(board.state);
        if (white_val > black_val)
            white.SetActive(true);
        else if (black_val > white_val)
            black.SetActive(true);
        else
            draw.SetActive(true);
    }

    public void OnPlayAgainPressed()
    {
        onPressedAudio.Play();
        board.ResetGame();
        Disable();
    }

    public void OnExitPressed()
    {
        onPressedAudio.Play();
        SceneManager.LoadScene(0);
    }
}
