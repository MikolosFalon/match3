using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches;
    private void Start() {
        currentMatches = new List<GameObject>();
        board = FindObjectOfType<Board>();

    }
    public void RemoveMatches(GameObject dot){
        currentMatches.Remove(dot);
    }
    public void FindAllMatched(){
        StartCoroutine(FindAllMatchesCo());
        
    }
    private IEnumerator FindAllMatchesCo(){
        yield return new WaitForSeconds(0.2f);
        for (int ix = 0; ix < board.size.x; ix++)
        {
            for (int iy = 0; iy < board.size.y; iy++)
            {
                GameObject currentDot = board.allDots[ix, iy];
                if (currentDot != null)
                {
                    //x
                    if (ix > 0 && ix < board.size.x - 1)
                    {
                        GameObject leftDot = board.allDots[ix - 1, iy];
                        GameObject rightDot = board.allDots[ix + 1, iy];
                        if (leftDot != null && rightDot != null)
                        {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                if(currentDot.GetComponent<Dot>().isRowBomb 
                                || leftDot.GetComponent<Dot>().isRowBomb
                                || rightDot.GetComponent<Dot>().isRowBomb){
                                    currentMatches.Union(GetRowPieces(iy));
                                }
                                if(currentDot.GetComponent<Dot>().isColumnBomb){
                                    currentMatches.Union(GetColumnPieces(ix));
                                }
                                if(leftDot.GetComponent<Dot>().isColumnBomb){
                                    currentMatches.Union(GetColumnPieces(ix-1));
                                }
                                if(rightDot.GetComponent<Dot>().isColumnBomb){
                                    currentMatches.Union(GetColumnPieces(ix+1));
                                }

                                if(!currentMatches.Contains(leftDot)){
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().isMatched = true;

                                if(!currentMatches.Contains(rightDot)){
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().isMatched = true;

                                if(!currentMatches.Contains(currentDot)){
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                    //y
                    if (iy > 0 && iy < board.size.y - 1)
                    {
                        GameObject UpDot = board.allDots[ix, iy + 1];
                        GameObject DownDot = board.allDots[ix, iy - 1];
                        if (DownDot != null && UpDot != null)
                        {
                            if (DownDot.tag == currentDot.tag && UpDot.tag == currentDot.tag)
                            {
                                if(currentDot.GetComponent<Dot>().isColumnBomb 
                                || DownDot.GetComponent<Dot>().isColumnBomb
                                || UpDot.GetComponent<Dot>().isColumnBomb){
                                    currentMatches.Union(GetColumnPieces(ix));
                                }
                                if(currentDot.GetComponent<Dot>().isRowBomb){
                                    currentMatches.Union(GetRowPieces(iy));
                                }
                                if(DownDot.GetComponent<Dot>().isRowBomb){
                                    currentMatches.Union(GetRowPieces(iy-1));
                                }
                                if(UpDot.GetComponent<Dot>().isRowBomb){
                                    currentMatches.Union(GetRowPieces(iy+1));
                                }
                                
                                if(!currentMatches.Contains(DownDot)){
                                    currentMatches.Add(DownDot);
                                }
                                DownDot.GetComponent<Dot>().isMatched = true;

                                if(!currentMatches.Contains(UpDot)){
                                    currentMatches.Add(UpDot);
                                }
                                UpDot.GetComponent<Dot>().isMatched = true;

                                if(!currentMatches.Contains(currentDot)){
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                }
            }

        }
    }
    List<GameObject> GetColumnPieces(int column){
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.size.y; i++)
        {
            if(board.allDots[column, i] !=null){
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetRowPieces(int row){
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.size.x; i++)
        {
            if(board.allDots[i, row] !=null){
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }

    public void CheckBombs(){
        // did the player move something
        if(board.currentDot != null){
            //is the piece they moved matched
            if(board.currentDot.isMatched){
                //make it unmatched
                board.currentDot.isMatched = false;
                //decide what kind of bomb to make
                int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb < 50){
                    //make a row bomb
                    board.currentDot.MakeRowBomb();
                }else{
                    //make a column bomb
                    board.currentDot.MakeColumnBomb();
                }
            }
            //is the other piece matcher
            else if(board.currentDot.otherDot != null){
                
            }
        }
    }
}
