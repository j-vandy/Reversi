using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Unity.VisualScripting;

public class Icon : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Button button;
    public int x;
    public int y;

    private void Start()
    {
        if (board == null)
            throw new NullReferenceException();
        if (button == null)
            throw new NullReferenceException();
    }

    private void OnEnable()
    {
        board.OnGeneratedMoves += ShouldEnable;
        board.OnPlacePiece += Disable;
    }
    private void OnDisable()
    {
        board.OnGeneratedMoves -= ShouldEnable;
        board.OnPlacePiece -= Disable;
    }

    private void ShouldEnable()
    {
        foreach (var state in board.moves)
        {
            if (state.playSpot[0] == x && state.playSpot[1] == y)
            {
                Enable();
                return;
            }
        }
        Disable();
    }

    [Button]
    public void Enable() => button.gameObject.SetActive(true);

    [Button]
    public void Disable() => button.gameObject.SetActive(false);

    [Button]
    public void Pressed()
    {
        board.PlacePiece(x, y);
        Disable();
    }
}
