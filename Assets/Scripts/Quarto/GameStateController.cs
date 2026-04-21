using Quarto;
using TMPro;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public Board board;
    public GameManager gameManager;
    public TextMeshProUGUI statusText;

    public enum State
    {
        Player2Draw,
        Player1Place,
        Player1Draw,
        Player2Place,
        End
    }

    private State currentState;

    void Start()
    {
        if (board == null)
            board = FindAnyObjectByType<Board>();

        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();

        ChangeState(State.Player2Draw);
    }

    void Update()
    {
        if (currentState == State.Player2Draw)
        {
            if (board.drawingConfirmed)
            {
                board.drawingConfirmed = false;
                ChangeState(State.Player1Place);
            }
        }
        else if (currentState == State.Player1Place)
        {
            if (board.piecePlaced)
            {
                board.piecePlaced = false;

                if (IsGameOver())
                    ChangeState(State.End);
                else
                    ChangeState(State.Player1Draw);
            }
        }
        else if (currentState == State.Player1Draw)
        {
            if (board.drawingConfirmed)
            {
                board.drawingConfirmed = false;
                ChangeState(State.Player2Place);
            }
        }
        else if (currentState == State.Player2Place)
        {
            if (board.piecePlaced)
            {
                board.piecePlaced = false;

                if (IsGameOver())
                    ChangeState(State.End);
                else
                    ChangeState(State.Player2Draw);
            }
        }
    }

    void ChangeState(State newState)
    {
        currentState = newState;

        if (statusText == null) return;

        if (currentState == State.Player2Draw)
            statusText.text = "Player 2 Draw";
        else if (currentState == State.Player1Place)
            statusText.text = "Player 1 Place";
        else if (currentState == State.Player1Draw)
            statusText.text = "Player 1 Draw";
        else if (currentState == State.Player2Place)
            statusText.text = "Player 2 Place";
        else if (currentState == State.End)
            statusText.text = "Game Over";
    }

    bool IsGameOver()
    {
        return false;
    }
}