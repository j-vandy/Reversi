using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

public struct BoardState
{
    public bool bIsWhite;
    public int[,] board;
    public int[] playSpot;
    public HashSet<int[]> flipLocations;
}

public class Board : MonoBehaviour
{
    public const int EMPTY = 0;
    public const int WHITE = 1;
    public const int BLACK = 2;
    private const int WIDTH = 8;
    private const int HEIGHT = 8;
    private int numberOfTurns = 0;
    private Piece[,] pieces = new Piece[WIDTH,HEIGHT];
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Score whiteScore;
    [SerializeField] private Score blackScore;
    [SerializeField] private Screen passScreen;
    public List<BoardState> moves = new List<BoardState>();
    public Action OnPlacePiece;
    public Action OnGeneratedMoves;
    public Action OnEndOfTurn;
    public Action OnGameOver;
    public BoardState state;
    public AudioSource placeAudio;
    public AudioSource flipAudio;


    private void Start()
    {
        if (piecePrefab == null)
            throw new NullReferenceException();
        if (whiteScore == null)
            throw new NullReferenceException();
        if (blackScore == null)
            throw new NullReferenceException();
        if (passScreen == null)
            throw new NullReferenceException();
        if (placeAudio == null)
            throw new NullReferenceException();
        if (flipAudio == null)
            throw new NullReferenceException();

        // init board state
        state.bIsWhite = true;
        state.board = new int[WIDTH,HEIGHT];

        // place starting pieces
        PlacePiece(3, 3);
        PlacePiece(4, 3);
        PlacePiece(4, 4);
        PlacePiece(3, 4);
    }

    [Button]
    public void ResetGame()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (pieces[x,y] != null)
                {
                    if (x == 3 && y == 3)
                    {
                        if (pieces[x,y].bIsWhite)
                            continue;
                        pieces[x, y].Flip();
                        state.board[x, y] = WHITE;
                    }
                    else if (x == 3 && y == 4)
                    {
                        if (!pieces[x,y].bIsWhite)
                            continue;
                        pieces[x, y].Flip();
                        state.board[x, y] = BLACK;
                    }
                    else if (x == 4 && y == 3)
                    {
                         if (!pieces[x,y].bIsWhite)
                            continue;
                        pieces[x, y].Flip();
                        state.board[x, y] = BLACK;
                    }
                    else if (x == 4 && y == 4)
                    {
                         if (pieces[x,y].bIsWhite)
                            continue;
                        pieces[x, y].Flip();
                        state.board[x, y] = WHITE;
                    }
                    else
                    {
                        pieces[x,y].Remove();
                        pieces[x,y] = null;
                        state.board[x,y] = EMPTY;
                    }
                }
            }
        }
        
        numberOfTurns = 0;
        state.bIsWhite = true;
        UpdateScore();
        StartCoroutine(UpdateResetValues());
    }

    private IEnumerator UpdateResetValues()
    {
        yield return new WaitForSeconds(Piece.REMOVE_DURATION);
        moves = Moves(state);
        if (OnGeneratedMoves != null)
            OnGeneratedMoves();
        if (OnEndOfTurn != null)
            OnEndOfTurn();
    }

    private IEnumerator FlipPieces(int x, int y)
    {
        yield return new WaitForSeconds(Piece.PLACE_DURATION);
        bool bFlipped = false;
        
        foreach (BoardState s in moves)
        {
            if (s.playSpot[0] == x && s.playSpot[1] == y)
            {
                foreach (int[] pos in s.flipLocations)
                {
                    bFlipped = true;

                    if (pieces[pos[0], pos[1]].bIsWhite)
                        state.board[pos[0], pos[1]] = BLACK;
                    else
                        state.board[pos[0], pos[1]] = WHITE;
                    pieces[pos[0], pos[1]].Flip();
                }
            }
        }

        UpdateScore();

        if (bFlipped)
        {
            flipAudio.Play();
            yield return new WaitForSeconds(Piece.FLIP_DURATION);
        }

        // find all possible moves
        moves = Moves(state);
        if (OnGeneratedMoves != null)
            OnGeneratedMoves();

        // pass turn if there are no possible moves
        if (moves.Count == 0)
        {
            // find all possible moves for next player
            state.bIsWhite = !state.bIsWhite;
            moves = Moves(state);
            if (OnGeneratedMoves != null)
                OnGeneratedMoves();

            // end the game if next player doesn't have any moves
            if (moves.Count == 0)
            {
                if (OnGameOver != null)
                    OnGameOver();
            }
            else
            {
                passScreen.Enable();
                UpdateScore();
            }
        }

        if (numberOfTurns < 5)
        {
            StartCoroutine(EndOfTurn());
        }
        else
        {
            if (OnEndOfTurn != null)
                OnEndOfTurn();
        }
    }

    private IEnumerator EndOfTurn()
    {
        yield return new WaitForSeconds(0.5f);

        if (OnEndOfTurn != null)
            OnEndOfTurn();
    }

    public void PlacePiece(int x, int y)
    {
        if (x >= WIDTH || y >= HEIGHT)
            throw new ArgumentOutOfRangeException();

        numberOfTurns++;

        if (OnPlacePiece != null)
            OnPlacePiece();

        // initialize piece and update pieces
        float world_x = x - 3.5f;
        float world_z = -y + 3.5f;
        Vector3 position = new Vector3(world_x, Piece.MAX_PLACE_HEIGHT, world_z);
        Piece p = Instantiate(piecePrefab, position, Quaternion.identity).GetComponent<Piece>();
        pieces[x,y] = p;
        if (p == null)
            Debug.LogError("GameObject does not contain 'Piece' component.");
        p.bIsWhite = state.bIsWhite;
        p.Place();

        // update board state
        state.board[x,y] = state.bIsWhite ? WHITE : BLACK;
        state.bIsWhite = !state.bIsWhite;

        StartCoroutine(PlayAudioAfterDelay(Piece.PLACE_DURATION - 0.1f, placeAudio));

        // flip all necessary tokens
        StartCoroutine(FlipPieces(x, y));
    }

    private IEnumerator PlayAudioAfterDelay(float delay, AudioSource source)
    {
        yield return new WaitForSeconds(delay);
        source.Play();
    }

    private static bool InBounds(int x, int y)
    {
        return 0 <= x && x < WIDTH && 0 <= y && y < HEIGHT;
    }

    private static BoardState Copy(BoardState state)
    {
        BoardState copy;
        copy.bIsWhite = state.bIsWhite;
        copy.board = new int[WIDTH,HEIGHT];
        copy.playSpot = new int[2];
        copy.flipLocations = new HashSet<int[]>();
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
                copy.board[x,y] = state.board[x,y];
        }
        return copy;
    }

    // helper function for moves
    private static void CheckDirection(List<BoardState> moves, BoardState state, int player, int opponent, int x_pos, int y_pos, int x_dir, int y_dir)
    {
        HashSet<int[]> flipLocations = new HashSet<int[]>();
        int tmpX = x_pos + x_dir;
        int tmpY = y_pos + y_dir;

        if (!InBounds(tmpX, tmpY))
            return;

        // create a tmp board for the move
        BoardState tmp = Copy(state);
        tmp.bIsWhite = !state.bIsWhite;

        if (state.board[tmpX, tmpY] == opponent)
        {
            tmp.board[tmpX, tmpY] = player;
            flipLocations.Add(new int[2] { tmpX, tmpY });
            tmpX += x_dir;
            tmpY += y_dir;
            while (InBounds(tmpX, tmpY))
            {
                if (state.board[tmpX, tmpY] == EMPTY)
                {
                    foreach(BoardState s in moves)
                    {
                        // this move already exists
                        if (s.playSpot[0] == tmpX && s.playSpot[1] == tmpY)
                        {
                            foreach (int[] pos in flipLocations)
                            {
                                s.board[pos[0], pos[1]] = player;
                                s.flipLocations.Add(pos);
                            }
                            return;
                        }
                    }
                    tmp.board[tmpX, tmpY] = player;
                    tmp.playSpot = new int[2] { tmpX, tmpY };
                    tmp.flipLocations = flipLocations;
                    moves.Add(tmp);
                    return;
                }
                else if (state.board[tmpX, tmpY] == opponent)
                {
                    tmp.board[tmpX, tmpY] = player;
                    flipLocations.Add(new int[2] { tmpX, tmpY });
                }
                else
                {
                    break;
                }
                tmpX += x_dir;
                tmpY += y_dir;
            }
        }
    }

    public static List<BoardState> Moves(BoardState state)
    {
        List<BoardState> moves = new List<BoardState>();

        // find player and opponent colors
        int player, opponent;
        if (state.bIsWhite)
        {
            player = WHITE;
            opponent = BLACK;
        }
        else
        {
            player = BLACK;
            opponent = WHITE;
        }

        // find player color pieces on the board
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (state.board[x,y] == player)
                {
                    // top left
                    CheckDirection(moves, state, player, opponent, x, y, -1, -1);
                    // top
                    CheckDirection(moves, state, player, opponent, x, y, 0, -1);
                    // top right
                    CheckDirection(moves, state, player, opponent, x, y, 1, -1);
                    // left
                    CheckDirection(moves, state, player, opponent, x, y, -1, 0);
                    // right
                    CheckDirection(moves, state, player, opponent, x, y, 1, 0);
                    // bottom left
                    CheckDirection(moves, state, player, opponent, x, y, -1, 1);
                    // bottom
                    CheckDirection(moves, state, player, opponent, x, y, 0, 1);
                    // bottom right
                    CheckDirection(moves, state, player, opponent, x, y, 1, 1);
                }
            }
        }

        return moves;
    }

    public static bool Terminal(BoardState state)
    {
        BoardState tmp = Copy(state);
        tmp.bIsWhite = state.bIsWhite;
        return Moves(state).Count == 0 && Moves(tmp).Count == 0;
    }

    // each piece represents one point. positive score represents white
    // negative score represents black
    public static int Value(BoardState state)
    {
        int output = 0;

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (state.board[x,y] == WHITE)
                    output++;
                else if (state.board[x,y] == BLACK)
                    output--;
            }
        }

        return output;
    }

    public static int WhiteValue(BoardState state)
    {
        int whites = 0;

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (state.board[x,y] == WHITE)
                    whites++;
            }
        }

        return whites;
    }

    public static int BlackValue(BoardState state)
    {
        int blacks = 0;

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (state.board[x,y] == BLACK)
                    blacks++;
            }
        }

        return blacks;
    }

    private void UpdateScore()
    {
        whiteScore.SetScore(WhiteValue(state));
        blackScore.SetScore(BlackValue(state));
        if (state.bIsWhite)
        {
            whiteScore.Highlight();
            blackScore.Unhighlight();
        }
        else
        {
            whiteScore.Unhighlight();
            blackScore.Highlight();
        }
    }


    public static void Print(BoardState state)
    {
        string log = "";
        if (state.bIsWhite)
            log += "WHITES MOVE\n";
        else
            log += "BLACKS MOVE\n";
        log += "SCORE: " + Value(state) + "\n";
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (state.board[x,y] == WHITE)
                    log += "w";
                else if (state.board[x,y] == BLACK)
                    log += "b";
                else
                    log += " . ";
            }
            log += "\n";
        }
        Debug.Log(log);
    }
}
