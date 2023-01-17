using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]private Vector2Int size;
    [SerializeField] private GameObject titlePrefab;
    private bgTitle[,]allTitle;

    private void Start() {
        allTitle = new bgTitle[size.x, size.y];
        SetUP();
    }

    private void SetUP(){
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                Vector2 tempPosition = new Vector2(ix, iy);
                Instantiate(titlePrefab, tempPosition, Quaternion.identity);
            }
        }
    }
}
