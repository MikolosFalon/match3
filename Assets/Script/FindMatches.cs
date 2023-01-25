using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    [SerializeField] private List<GameObject> currentMatches;
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
}
