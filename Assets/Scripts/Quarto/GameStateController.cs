using Quarto;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    public GameObject grid;
    public GameObject drawBoard;
    public Board board;
    public GameManager gameManager;
    
    public TextMeshProUGUI statusText;
    
    

    public enum State
    {
        Start,
        Player2Draw,
        Player1Place,
        Player1Draw,
        Player2Place,
        End
    }

    private State currentstate;
    
    private bool isDoneDrawing = false;
    private bool isDonePlaceing = false;

    void ChangeState(State state)
    {
        currentstate = state;
        if (currentstate == State.Start)
        {
            statusText.text = "Game Started";
            grid.SetActive(true);
            drawBoard.SetActive(false);
        }
        else if (currentstate == State.Player2Draw)
        {
            statusText.text = "Player 2 Draw";
            
            grid.SetActive(false);
            drawBoard.SetActive(true);
        }
        else if (currentstate == State.Player1Place)
        {
            statusText.text = "Player 1 Place";
            
            grid.SetActive(true);
            drawBoard.SetActive(false);
        }
        else if (currentstate == State.Player1Draw)
        {
            statusText.text = "Player 1 Draw";
            grid.SetActive(false);
            drawBoard.SetActive(true);
        }
        else if (currentstate == State.Player2Place)
        {
            statusText.text = "Player 2 Place";
            grid.SetActive(true);
            drawBoard.SetActive(false);
        }
        else if (currentstate == State.End)
        {
            statusText.text = "Game Over";
            grid.SetActive(true);
            drawBoard.SetActive(false);
        }
        
        Debug.Log(currentstate);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.Find("Grid");
        drawBoard = GameObject.FindGameObjectWithTag("Draw Board");
        board = GameObject.Find("Game Manager").GetComponent<Board>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ChangeState(State.Start);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentstate == State.Start)
        {
            board.ResetGrid();
            ChangeState(State.Player2Draw);
        }
        else if (currentstate == State.Player2Draw)
        {
            if (board.drawingConfirmed)
            {
                board.drawingConfirmed = false;
                ChangeState(State.Player1Place);
            }
        }
        
        else if (currentstate == State.Player1Place)
        {
            if (board.piecePlaced)
            {
                board.piecePlaced = false;
                {
                    if (gameManager.hasWinner || gameManager.BoardIsFull())
                    {
                        ChangeState(State.End);
                    }
                    else
                    {
                        ChangeState(State.Player1Draw);
                    }
                }
            }
        }
        
        else if (currentstate == State.Player1Draw)
        {
            if (board.drawingConfirmed)
            {
                board.drawingConfirmed = false;
                ChangeState(State.Player2Place);
            }
        }
        
        else if (currentstate == State.Player2Place)
        {
            if (board.piecePlaced)
            {
                board.piecePlaced = false;
                {
                    if (gameManager.hasWinner || gameManager.BoardIsFull())
                    {
                        ChangeState(State.End);
                    }
                    else
                    {
                        ChangeState(State.Player2Draw);
                    }
                }
            }
        }
        
        else if (currentstate == State.End)
        {
            
        }
    }
}
