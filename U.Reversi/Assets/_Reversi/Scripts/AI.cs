using System;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private GameDataSO gameData;
    [SerializeField] private Board board;
    private int skillLevel = 0;
    public bool bEnabled = true;
    public bool bIsWhite;

    private void OnEnable() => board.OnEndOfTurn += MakeMove;

    private void OnDisable() => board.OnEndOfTurn -= MakeMove;

    private void Start()
    {
        if (gameData == null)
            throw new NullReferenceException();
        if (board == null)
            throw new NullReferenceException();

        bEnabled = gameData.bAIEnabled;
        bIsWhite = gameData.bAIIsWhite;
        skillLevel = gameData.AIDifficulty;
    }

    public void MakeMove()
    {
        if (!bEnabled)
            return;

        if (bIsWhite != board.state.bIsWhite)
            return;

        int[] bestMove = null;
        int bestMiniMax = 0;
        int tmpMiniMax;
        foreach(BoardState state in Board.Moves(board.state))
        {
            if (bestMove == null)
            {
                bestMove = state.playSpot;
                bestMiniMax = MiniMax(state, skillLevel);
                continue;
            }

            tmpMiniMax = MiniMax(state, skillLevel);
            if (board.state.bIsWhite)
            {
                if (tmpMiniMax > bestMiniMax)
                {
                    bestMiniMax = tmpMiniMax;
                    bestMove = state.playSpot;
                }
            }
            else
            {
                if (tmpMiniMax < bestMiniMax)
                {
                    bestMiniMax = tmpMiniMax;
                    bestMove = state.playSpot;
                }
            }
        }

        if (bestMove != null)
            board.PlacePiece(bestMove[0], bestMove[1]);
    }

    // alpha is the guaranteed minimum value for White
    // beta is the guaranteed minimum value for Black
    private int MiniMax(BoardState state, int level)
    {
        if (Board.Terminal(state) || level <= 0)
        {
            return Board.Value(state);
        }

        if (state.bIsWhite)
        {
            // given the current move for black, value is the
            // best possible move white can make
            int value = int.MinValue;
            foreach (BoardState s in Board.Moves(state))
            {
                //value = Mathf.Max(value, MiniMax(s, level - 1, alpha, beta));
                value = Mathf.Max(value, MiniMax(s, level - 1));
            }
            return value;
        }
        else
        {
            // given the current move for white, value is the
            // best possible move black can make
            int value = int.MaxValue;
            foreach (BoardState s in Board.Moves(state))
            {
                value = Mathf.Min(value, MiniMax(s, level - 1));
            }
            return value;
        }
    }
}
