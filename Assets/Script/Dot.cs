using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private Vector2Int dotPosition;
    [SerializeField] private Vector2Int TargetPosition;
    //change later
    private Board board;
    private GameObject otherDot;

    private Vector2 fistTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 TempPosition;

    [SerializeField] private float swipeAngle = 0;
    [SerializeField] private float moveTime = 1;
    private void Start() {
        //change 
        //need singleton
        board = FindObjectOfType<Board>();
        TargetPosition =new Vector2Int((int)transform.position.x, (int)transform.position.y);
        dotPosition = TargetPosition;
        moveTime = 0.4f;
    }

    private void Update() {
        TargetPosition =dotPosition;
        //change later (copy code )
        //x
        if(Mathf.Abs(TargetPosition.x-transform.position.x) > 0.1 ){
            //move towards the target
            TempPosition = new Vector2(TargetPosition.x, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, TempPosition, moveTime);
        }else{
            //directly set the position
            TempPosition = new Vector2(TargetPosition.x, transform.position.y);
            transform.position = TempPosition;
            board.allDots[dotPosition.x, dotPosition.y] = gameObject;
        }
        //y
        if(Mathf.Abs(TargetPosition.y-transform.position.y)> 0.1 ){
            //move towards the target
            TempPosition = new Vector2(transform.position.x, TargetPosition.y);
            transform.position = Vector2.Lerp(transform.position, TempPosition, moveTime);
        }else{
            //directly set the position
            TempPosition = new Vector2(transform.position.x, TargetPosition.y);
            transform.position = TempPosition;
            board.allDots[dotPosition.x, dotPosition.y] = gameObject;
        } 
    }

    private void OnMouseDown()
    {
        fistTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngel();
    }
    private void CalculateAngel(){
        swipeAngle = Mathf.Atan2(
            finalTouchPosition.y - fistTouchPosition.y,
            finalTouchPosition.x - fistTouchPosition.x) * 180 / Mathf.PI;
        MovePieces();
    }
    private void MovePieces(){
        if(swipeAngle > -45 && swipeAngle <= 45 && dotPosition.x< board.size.x){
            //swipe right
            otherDot = board.allDots[dotPosition.x+1, dotPosition.y];

            otherDot.GetComponent<Dot>().dotPosition.x-=1;
            dotPosition.x++;
        }else if(swipeAngle > 45 && swipeAngle <= 135 && dotPosition.y< board.size.y){
            //swipe up
            otherDot = board.allDots[dotPosition.x, dotPosition.y+1];
            otherDot.GetComponent<Dot>().dotPosition.y-=1;
            dotPosition.y++;
        }else if((swipeAngle > 135 || swipeAngle <= -135) && dotPosition.x>0){
            //swipe left
            otherDot = board.allDots[dotPosition.x-1, dotPosition.y];
            otherDot.GetComponent<Dot>().dotPosition.x+=1;
            dotPosition.x--;
        }else if(swipeAngle < -45 && swipeAngle >= -135 && dotPosition.y>0){
            //swipe down
            otherDot = board.allDots[dotPosition.x, dotPosition.y-1];
            otherDot.GetComponent<Dot>().dotPosition.y+=1;
            dotPosition.y--;
        }

    }
}
