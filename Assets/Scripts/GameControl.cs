using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    private GameObject[,] squares = new GameObject[4, 4];
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform blockParent;
    private void Start()
    {
        CreateBlock();
    }

    private void CreateBlock()
    {
        Vector2 index = GetRandomEmptySquareIndex();
        GameObject go = Instantiate(blockPrefab, blockParent);
        Block block = go.GetComponent<Block>();

        block.index = index;
        block.UpdatePosition();
        block.UpdateTextValue();
        squares[(int)index.x, (int)index.y] = go;

        Debug.Log("Created block at : " + index.x + "--" + index.y);
    }

    private Vector2 GetRandomEmptySquareIndex()
    {
        List<Vector2> emptySquares = new();
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                if (squares[i, j] == null) emptySquares.Add(new Vector2(i, j));
            }
        }

        return emptySquares[Random.Range(0, emptySquares.Count)];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveSquares(Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveSquares(Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveSquares(Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveSquares(Direction.Right);
        }
    }

    private void MoveSquares(Direction dir)
    {
        bool flag = false;

        for (int k = 0; k < 3; k++)
        {
            for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                if (squares[i, j] != null)
                {
                    if (MoveOrMerge(i, j, dir)) flag = true;
                }
            }
        }
        }

        if (flag)
        {
            CreateBlock();
            //TODO: play click sound
        }
            
    }

    private bool MoveOrMerge(int i, int j, Direction dir)
    {
        bool movedOrMerged = false;

        switch (dir)
        {
            case Direction.Up:
                Action action = ControlNeighbour(i, j - 1, squares[i, j].GetComponent<Block>().value);
                int jCopy = j;
                while(action != Action.None)
                {
                    
                    if (action == Action.Move)
                    {
                        squares[i, jCopy - 1] = squares[i, jCopy];
                        squares[i, jCopy] = null;
                        squares[i, jCopy - 1].GetComponent<Block>().index = new Vector2(i, jCopy - 1);
                        squares[i, jCopy - 1].GetComponent<Block>().UpdatePosition();
                        movedOrMerged = true;
                    }
                    else if (action == Action.Merge)
                    {
                        squares[i, jCopy - 1].GetComponent<Block>().value *= 2;
                        ScoreHandler.Instance.AddScore(squares[i, jCopy - 1].GetComponent<Block>().value);
                        squares[i, jCopy - 1].GetComponent<Block>().UpdateTextValue();
                        squares[i, jCopy - 1].GetComponent<Block>().UpdateColor();
                        Destroy(squares[i, jCopy]);
                        squares[i, jCopy] = null;
                        movedOrMerged = true;
                        break;
                    }

                    jCopy--;
                    if (jCopy < 0) break;
                    action = ControlNeighbour(i, jCopy - 1, squares[i, jCopy].GetComponent<Block>().value);
                }
                

                break;
            case Direction.Down:
                Action action2 = ControlNeighbour(i, j + 1, squares[i, j].GetComponent<Block>().value);
                int jCopy2 = j;
                while (action2 != Action.None)
                {

                    if (action2 == Action.Move)
                    {
                        squares[i, jCopy2 + 1] = squares[i, jCopy2];
                        squares[i, jCopy2] = null;
                        squares[i, jCopy2 + 1].GetComponent<Block>().index = new Vector2(i, jCopy2 + 1);
                        squares[i, jCopy2 + 1].GetComponent<Block>().UpdatePosition();
                        movedOrMerged = true;
                    }
                    else if (action2 == Action.Merge)
                    {
                        squares[i, jCopy2 + 1].GetComponent<Block>().value *= 2;
                        ScoreHandler.Instance.AddScore(squares[i, jCopy2 + 1].GetComponent<Block>().value);
                        squares[i, jCopy2 + 1].GetComponent<Block>().UpdateTextValue();
                        squares[i, jCopy2 + 1].GetComponent<Block>().UpdateColor();
                        Destroy(squares[i, jCopy2]);
                        squares[i, jCopy2] = null;
                        movedOrMerged = true;
                        break;
                    }

                    jCopy2++;
                    if (jCopy2 > 3) break;
                    action2 = ControlNeighbour(i, jCopy2 + 1, squares[i, jCopy2].GetComponent<Block>().value);
                }
                break;
            case Direction.Left:
                Action action3 = ControlNeighbour(i - 1, j, squares[i, j].GetComponent<Block>().value);
                int iCopy = i;
                while (action3 != Action.None)
                {
                    Debug.Log("Trying to move left, action = " + action3 + " iCopy = " + iCopy);
                    if (action3 == Action.Move)
                    {
                        squares[iCopy - 1, j] = squares[iCopy, j];
                        squares[iCopy, j] = null;
                        squares[iCopy - 1, j].GetComponent<Block>().index = new Vector2(iCopy - 1, j);
                        squares[iCopy - 1, j].GetComponent<Block>().UpdatePosition();
                        movedOrMerged = true;
                    }
                    else if (action3 == Action.Merge)
                    {
                        squares[iCopy - 1, j].GetComponent<Block>().value *= 2;
                        ScoreHandler.Instance.AddScore(squares[iCopy - 1, j].GetComponent<Block>().value);
                        squares[iCopy - 1, j].GetComponent<Block>().UpdateTextValue();
                        squares[iCopy - 1, j].GetComponent<Block>().UpdateColor();
                        Destroy(squares[iCopy, j]);
                        squares[iCopy, j] = null;
                        movedOrMerged = true;
                        break;
                    }

                    iCopy--;
                    if (iCopy < 0) break;
                    action3 = ControlNeighbour(iCopy - 1, j, squares[iCopy, j].GetComponent<Block>().value);
                }
                break;
            case Direction.Right:
                Action action4 = ControlNeighbour(i + 1, j, squares[i, j].GetComponent<Block>().value);
                int iCopy2 = i;
                while (action4 != Action.None)
                {

                    if (action4 == Action.Move)
                    {
                        squares[iCopy2 + 1, j] = squares[iCopy2, j];
                        squares[iCopy2, j] = null;
                        squares[iCopy2 + 1, j].GetComponent<Block>().index = new Vector2(iCopy2 + 1, j);
                        squares[iCopy2 + 1, j].GetComponent<Block>().UpdatePosition();
                        movedOrMerged = true;
                    }
                    else if (action4 == Action.Merge)
                    {
                        squares[iCopy2 + 1, j].GetComponent<Block>().value *= 2;
                        ScoreHandler.Instance.AddScore(squares[iCopy2 + 1, j].GetComponent<Block>().value);
                        squares[iCopy2 + 1, j].GetComponent<Block>().UpdateTextValue();
                        squares[iCopy2 + 1, j].GetComponent<Block>().UpdateColor();
                        Destroy(squares[iCopy2, j]);
                        squares[iCopy2, j] = null;
                        movedOrMerged = true;
                        break;
                    }

                    iCopy2++;
                    if (iCopy2 > 3) break;
                    action4 = ControlNeighbour(iCopy2 + 1, j, squares[iCopy2, j].GetComponent<Block>().value);
                }
                break;

            default: break;
        }


        ControlGameOver();
        return movedOrMerged;
    }

    private void ControlGameOver()
    {
        bool flag = false;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                
                if(squares[i, j] == null) flag = true;
            }
        }

        if(!flag) ScoreHandler.Instance.FinishGame();
    }

    

    private Action ControlNeighbour(int i, int j, int value)
    {
        if(i < 0 || i > 3 || j < 0 || j > 3) return Action.None;

        if(squares[i, j] == null) return Action.Move;

        else
        {
            Block block = squares[i, j].GetComponent<Block>();
            if (block.value == value) return Action.Merge;
            else return Action.None;
        }
    }



    public enum Action
    {
        Move,
        Merge,
        None
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
