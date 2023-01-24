using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //change later
    public Vector2Int size;
    [SerializeField] private GameObject titlePrefab;
    private bgTitle[,]allTitle;
    //change later
    public GameObject[,]allDots;

    [SerializeField] private List<GameObject> dots;

    private void Start() {
        allTitle = new bgTitle[size.x, size.y];
        allDots = new GameObject[size.x, size.y];
        SetUP();
    }

    private void SetUP(){
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                Vector2 tempPosition = new Vector2(ix, iy);

                //bg
                GameObject bgTitle= Instantiate(titlePrefab, tempPosition, Quaternion.identity);
                bgTitle.transform.SetParent(transform);
                bgTitle.name = "( " + ix + ", " + iy + " )";
                
                //dots
                int dotToUse = Random.Range(0, dots.Count);
                int maxIterations = 0;

                while(MatchesAt(new Vector2Int(ix,iy),dots[dotToUse]) && maxIterations < 100){
                    dotToUse = Random.Range(0, dots.Count);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject dot = Instantiate(
                dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.SetParent(transform);
                dot.name = "( " + ix + ", " + iy + " )";
                allDots[ix, iy] = dot;
            }
        }
    }
    private bool MatchesAt(Vector2Int positionPiece, GameObject piece){
        if(positionPiece.x > 1 && positionPiece.y > 1){
            if(allDots[positionPiece.x-1, positionPiece.y].tag == piece.tag &&
            allDots[positionPiece.x-2, positionPiece.y].tag == piece.tag){
                return true;
            }
            if(allDots[positionPiece.x, positionPiece.y-1].tag == piece.tag &&
            allDots[positionPiece.x, positionPiece.y-2].tag == piece.tag){
                return true;
            }
        }else {
            if (positionPiece.y > 1)
            {
                if (allDots[positionPiece.x, positionPiece.y - 1].tag == piece.tag &&
            allDots[positionPiece.x, positionPiece.y - 2].tag == piece.tag)
                {
                    return true;
                }
            }

            if (positionPiece.x > 1)
            {
            if(allDots[positionPiece.x-1, positionPiece.y].tag == piece.tag &&
            allDots[positionPiece.x-2, positionPiece.y].tag == piece.tag){
                return true;
            }
            }
        }
        return false;
    }
    private void DestroyMatchesAt(Vector2Int positionPiece){
        if(allDots[positionPiece.x, positionPiece.y].GetComponent<Dot>().IsMatched){
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
                    allDots[ix, iy].GetComponent<Dot>().DotPosition(0, nullCount);
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
                    Vector2 tempPosition = new Vector2(ix, iy);
                    int dotToUse = Random.Range(0, dots.Count);
                    GameObject dot = Instantiate(
                    dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.transform.SetParent(transform);
                    dot.name = "( " + ix + ", " + iy + " )";
                    allDots[ix, iy] = dot;

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
                    if(allDots[ix, iy].GetComponent<Dot>().IsMatched){
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
    }
}
