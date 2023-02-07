using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Board board;
    [SerializeField]private float cameraOffset;
    private float aspectRatio = 0.625f;
    [SerializeField] private float padding=2.0f;

    private void Start(){
        board = FindObjectOfType<Board>();
        if(board != null){
            repositionCamera(board.size-Vector2Int.one);
        }
    }
    private void repositionCamera(Vector2 tempPosition){
        tempPosition = new Vector2((tempPosition.x-0.25f) / 2,(tempPosition.y)/ 2);
        transform.position = new Vector3(tempPosition.x, tempPosition.y,cameraOffset);
        if (board.size.x >= board.size.y)
        {
            Camera.main.orthographicSize = (board.size.x / 2 + padding) / aspectRatio;
        }else{
            Camera.main.orthographicSize = board.size.y / 2 + padding;
        }
    }
}
