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
    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.dotPosition.y));
        }
        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.dotPosition.y));
        }
        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.dotPosition.y));
        }

        return currentDots;
    }

    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot1.dotPosition.x));
        }
        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot2.dotPosition.x));
        }
        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot3.dotPosition.x));
        }

        return currentDots;
    }
    private void AddToListAndMatch(GameObject dot){
        if (!currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
        }
        dot.GetComponent<Dot>().isMatched = true;
    }
    private  void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3){
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }
    private IEnumerator FindAllMatchesCo(){
        yield return new WaitForSeconds(0.2f);
        for (int ix = 0; ix < board.size.x; ix++)
        {
            for (int iy = 0; iy < board.size.y; iy++)
            {
                GameObject currentDot = board.allDots[ix, iy];
                Dot currentDotDot = currentDot.GetComponent<Dot>();
                if (currentDot != null)
                {
                    //x
                    if (ix > 0 && ix < board.size.x - 1)
                    {
                        GameObject leftDot = board.allDots[ix - 1, iy];
                        Dot leftDotDot = leftDot.GetComponent<Dot>();
                        GameObject rightDot = board.allDots[ix + 1, iy];
                        Dot rightDotDot = rightDot.GetComponent<Dot>();
                        
                        if (leftDot != null && rightDot != null)
                        {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {

                                currentMatches.Union(IsRowBomb(
                                    leftDotDot, currentDotDot,rightDotDot));

                                currentMatches.Union(IsColumnBomb(
                                    leftDotDot, currentDotDot, rightDotDot));

                                GetNearbyPieces(leftDot, currentDot, rightDot);

                            }
                        }
                    }
                    //y
                    if (iy > 0 && iy < board.size.y - 1)
                    {
                        GameObject UpDot = board.allDots[ix, iy + 1];
                        Dot upDotDot = UpDot.GetComponent<Dot>();
                        GameObject downDot = board.allDots[ix, iy - 1];
                        Dot downDotDot = downDot.GetComponent<Dot>();
                        if (downDot != null && UpDot != null)
                        {
                            if (downDot.tag == currentDot.tag && UpDot.tag == currentDot.tag)
                            {
                                currentMatches.Union(IsRowBomb(
                                    upDotDot, currentDotDot,downDotDot));

                                currentMatches.Union(IsColumnBomb(
                                    upDotDot, currentDotDot, downDotDot));

                                GetNearbyPieces(UpDot, currentDot, downDot);
                            }
                        }
                    }
                }
            }

        }
    }
    //change
    public void MatchPiecesOfColor(string color){
        for (int ix = 0; ix < board.size.x; ix++)
        {
            for (int iy = 0; iy < board.size.y; iy++)
            {
                //check if that piece exists
                if(board.allDots[ix,iy] != null){
                    //check the tag on that dot
                    if(board.allDots[ix,iy].tag==color){
                        //set that dot to be matched
                        board.allDots[ix, iy].GetComponent<Dot>().isMatched = true;
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
                /*
                int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb < 50){
                    //make a row bomb
                    board.currentDot.MakeRowBomb();
                }else{
                    //make a column bomb
                    board.currentDot.MakeColumnBomb();
                }
                */
                if((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                || (board.currentDot.swipeAngle < -135 && board.currentDot.swipeAngle >= 135)){
                    board.currentDot.MakeRowBomb();
                }else{
                    board.currentDot.MakeColumnBomb();
                }            
            }
            //is the other piece matcher
            else if(board.currentDot.otherDot != null){
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                //check is the other dot matched
                if(otherDot.isMatched){
                    //make it unmatched
                    otherDot.isMatched = false;
                    /*
                    //decide what kind of bomb to make
                     int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb < 50){
                    //make a row bomb
                    otherDot.MakeRowBomb();
                }else{
                    //make a column bomb
                    otherDot.MakeColumnBomb();
                }
                */
                if((otherDot.swipeAngle > -45 && otherDot.swipeAngle <= 45)
                || (otherDot.swipeAngle < -135 && otherDot.swipeAngle >= 135)){
                    otherDot.MakeRowBomb();
                }else{
                    otherDot.MakeColumnBomb();
                }
                }
            }
        }
    }
}
