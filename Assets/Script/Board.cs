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
                /*
                GameObject bgTitle= Instantiate(titlePrefab, tempPosition, Quaternion.identity);
                bgTitle.transform.SetParent(transform);
                bgTitle.name = "( " + ix + ", " + iy + " )";
                */
                //dots
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
