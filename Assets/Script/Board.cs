using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{
        wait,
        move
}

public enum TileKind{
    breakable,
    Blank,
    Normal
}

[System.Serializable]
public class TileType{
    public Vector2Int tilePosition;
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    //change later
    public GameState currentState;
    public Vector2Int size;
    [SerializeField]private int offSet;
    [SerializeField] private GameObject titlePrefab;
    [SerializeField] private List<GameObject> dots;
    [SerializeField] private GameObject DestroyEffect;
    [SerializeField] private List<TileType> boardLayout;
    private bool[,]blankSpaces;
    //change later
    public GameObject[,]allDots;
    public Dot currentDot;
    private FindMatches findMatches;


    private void Start() {
        currentState = GameState.move;
        findMatches=FindObjectOfType<FindMatches>();
        blankSpaces = new bool[size.x, size.y];
        allDots = new GameObject[size.x, size.y];
        SetUP();
    }

    public void GenerateBlankSpaces(){
        for (int i = 0; i < boardLayout.Count; i++)
        {
            if(boardLayout[i].tileKind == TileKind.Blank){
                blankSpaces[boardLayout[i].tilePosition.x, boardLayout[i].tilePosition.y] = true;
            }
        }
    }

    private void SetUP(){
        GenerateBlankSpaces();
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                if (!blankSpaces[ix, iy])
                {
                    Vector2 tempPosition = new Vector2(ix, iy + offSet);
                    Vector2 tempPositionBG = new Vector2(ix, iy);
                    //bg
                    GameObject bgTitle = Instantiate(
                        titlePrefab, tempPositionBG, Quaternion.identity);
                    bgTitle.transform.SetParent(transform);
                    bgTitle.name = "( " + ix + ", " + iy + " )";

                    //dots
                    int dotToUse = Random.Range(0, dots.Count);
                    int maxIterations = 0;

                    while (MatchesAt(new Vector2Int(ix, iy), dots[dotToUse]) && 
                        maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Count);
                        maxIterations++;
                    }
                    maxIterations = 0;

                    GameObject dot = Instantiate(
                    dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().DotPosition(ix, iy);
                    dot.transform.SetParent(transform);
                    dot.name = "( " + ix + ", " + iy + " )";
                    allDots[ix, iy] = dot;
                }
            }
        }
    }
    private bool MatchesAt(Vector2Int positionPiece, GameObject piece){
        if(positionPiece.x > 1 && positionPiece.y > 1){
            if (allDots[positionPiece.x - 1, positionPiece.y] != null &&
            allDots[positionPiece.x - 2, positionPiece.y] != null)
            {
                if (allDots[positionPiece.x - 1, positionPiece.y].tag == piece.tag &&
                allDots[positionPiece.x - 2, positionPiece.y].tag == piece.tag)
                {
                    return true;
                }
            }
             
            if (allDots[positionPiece.x, positionPiece.y - 1] != null &&
            allDots[positionPiece.x, positionPiece.y - 2] != null)
            {
                if (allDots[positionPiece.x, positionPiece.y - 1].tag == piece.tag &&
                allDots[positionPiece.x, positionPiece.y - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }else if(positionPiece.x <= 1 || positionPiece.y <= 1){
            if (positionPiece.y > 1)
            {
                if ((allDots[positionPiece.x, positionPiece.y - 1] != null &&
                allDots[positionPiece.x, positionPiece.y - 2] != null))
                {
                    if (allDots[positionPiece.x, positionPiece.y - 1].tag == piece.tag &&
                allDots[positionPiece.x, positionPiece.y - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }

            if (positionPiece.x > 1)
            {
                if ((allDots[positionPiece.x, positionPiece.y - 1] != null &&
                allDots[positionPiece.x, positionPiece.y - 2] != null))
                {
                    if (allDots[positionPiece.x - 1, positionPiece.y].tag == piece.tag &&
                    allDots[positionPiece.x - 2, positionPiece.y].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private bool ColumnOrRow(){
        Vector2Int number= Vector2Int.zero;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if (firstPiece != null)
        {
            foreach (var item in findMatches.currentMatches)
            {
                Dot dot = item.GetComponent<Dot>();
                if(dot.dotPosition.y ==firstPiece.dotPosition.y){
                    number.x++;
                }
                if(dot.dotPosition.x ==firstPiece.dotPosition.x){
                    number.y++;
                }
            }
        }
        return (number.x ==5 || number.y==5);
    }
    private void CheckToMakeBombs(){
        if(findMatches.currentMatches.Count==4 || findMatches.currentMatches.Count==7){
            findMatches.CheckBombs();
        }
        if(findMatches.currentMatches.Count==5 || findMatches.currentMatches.Count==8){
            if(ColumnOrRow()){
                //make a color bomb
                //is the current dot matched
                if(currentDot !=null){
                    if(currentDot.isMatched){
                        if(!currentDot.isColorBomb){
                            currentDot.isMatched = false;
                            currentDot.MakeColumnBomb();
                        }
                    }else{
                        if(currentDot.otherDot != null){
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if(otherDot.isColorBomb){
                                otherDot.isMatched = false;
                                otherDot.MakeColorBomb();
                            }
                        }
                    }
                }
            }else{
                //make a adjacent bomb
                //is the current dot matched
                if(currentDot !=null){
                    if(currentDot.isMatched){
                        if(!currentDot.isAdjacentBomb ){
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }else{
                        if(currentDot.otherDot != null){
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if(otherDot.isAdjacentBomb){
                                otherDot.isMatched = false;
                                otherDot.MakeAdjacentBomb();
                            }
                        }
                    }
                }
            }
        }
    }
    
    private void DestroyMatchesAt(Vector2Int positionPiece){
        if(allDots[positionPiece.x, positionPiece.y].GetComponent<Dot>().isMatched){
            //how many elements are in the matched pieces list from findMatches
            if(findMatches.currentMatches.Count >=4){
                CheckToMakeBombs();
            }

            GameObject particle= Instantiate(DestroyEffect, 
            allDots[positionPiece.x, positionPiece.y].transform.position,Quaternion.identity);
            Destroy(particle, 1.0f);
            Destroy(allDots[positionPiece.x, positionPiece.y]);
            allDots[positionPiece.x, positionPiece.y] = null;
        }
    }
    public void DestroyMatches(){
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                if (allDots[ix, iy] !=null)
                {
                    DestroyMatchesAt(new Vector2Int(ix, iy));
                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(RecreateRowCo());
    }
    private IEnumerator RecreateRowCo(){
        int nullCount = 0;
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            { 
                if (allDots[ix, iy] ==null)
                {
                    nullCount++;
                }else if(nullCount >0){
                    allDots[ix, iy].GetComponent<Dot>().dotPosition.y-= nullCount;
                    allDots[ix, iy] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());
    }
    private void RefillBoard(){
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                if (allDots[ix, iy] == null)
                { 
                    Vector2 tempPosition = new Vector2(ix, iy+offSet);
                    int dotToUse = Random.Range(0, dots.Count);
                    GameObject dot = Instantiate(
                    dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.transform.SetParent(transform);
                    dot.name = "( " + ix + ", " + iy + " )";
                    allDots[ix, iy] = dot;
                    dot.GetComponent<Dot>().DotPosition(ix,iy);
                }
            }
        }
    }
    private bool MatchesOnBoard(){
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                if (allDots[ix, iy] != null)
                { 
                    if(allDots[ix, iy].GetComponent<Dot>().isMatched){
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator FillBoardCo(){
        RefillBoard();
        yield return new WaitForSeconds(0.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches();
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(0.5f);
        currentState = GameState.move;
    }
}
