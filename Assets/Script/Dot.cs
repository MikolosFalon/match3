using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    //board variables
    public Vector2Int dotPosition;
    public void DotPosition(int xPosition, int yPosition){
        dotPosition.x =xPosition;
        dotPosition.y =yPosition;
    }
    private Vector2Int dotPrevious;
    private Vector2Int TargetPosition;
    public bool isMatched = false;

    private Color matchedColor;
    private FindMatches findMatches;
    private Board board;
    public GameObject otherDot;

    private Vector2 fistTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 TempPosition;
    //swipe stuff
    private float swipeAngle = 0;
    private float swipeResist = 1;
    //powerUp stuff
    public bool isColumnBomb;
    public bool isRowBomb;
    [SerializeField] private GameObject rowArrow;
    [SerializeField] private GameObject columnArrow;

    private float moveTime;
    private SpriteRenderer sr;
    private void Start() {
        //change 
        //need singleton
        sr = GetComponent<SpriteRenderer>();
        matchedColor = new Color(1, 1, 1, 0.2f);
        board = FindObjectOfType<Board>();
        findMatches=FindObjectOfType<FindMatches>();
        //TargetPosition =new Vector2Int((int)transform.position.x, (int)transform.position.y);
        //dotPosition = TargetPosition;
        //dotPrevious = dotPosition;
        moveTime = 0.6f;

        isColumnBomb = false;
        isRowBomb = false;
    }

    //testing
    
    private void OnMouseOver()
    {
        /*
        if (Input.GetMouseButtonDown(1)) {
            isColumnBomb = true;
            GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
            arrow.transform.SetParent(transform);
        }
        */
        //
        if (Input.GetMouseButtonDown(1)) {
            isRowBomb = true;
            GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
            arrow.transform.SetParent(transform);
        }
        //
    }
    

    private void Update() {
        //FindMatches();
        /*
        if (isMatched)
        {
            sr.color = matchedColor;
        }
        */
        TargetPosition =dotPosition;
        //change later (copy code )
        //x
        if(Mathf.Abs(TargetPosition.x-transform.position.x) > 0.1 ){
            //move towards the target
            TempPosition = new Vector2(TargetPosition.x, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, TempPosition, moveTime);
            if(board.allDots[dotPosition.x, dotPosition.y] != this.gameObject){
                board.allDots[dotPosition.x, dotPosition.y] = this.gameObject;
            }
            findMatches.FindAllMatched();
        }else{
            //directly set the position
            TempPosition = new Vector2(TargetPosition.x, transform.position.y);
            transform.position = TempPosition;
        }
        //y
        if(Mathf.Abs(TargetPosition.y-transform.position.y)> 0.1 ){
            //move towards the target
            TempPosition = new Vector2(transform.position.x, TargetPosition.y);
            transform.position = Vector2.Lerp(transform.position, TempPosition, moveTime);
            if(board.allDots[dotPosition.x, dotPosition.y] != this.gameObject){
                board.allDots[dotPosition.x, dotPosition.y] = this.gameObject;
            }
            findMatches.FindAllMatched();
        }else{
            //directly set the position
            TempPosition = new Vector2(transform.position.x, TargetPosition.y);
            transform.position = TempPosition;
        } 
    }

    private IEnumerator CheckMoveCo(){
        yield return new WaitForSeconds(0.5f);
        if(otherDot != null){
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched){
                otherDot.GetComponent<Dot>().dotPosition = dotPosition;
                dotPosition = dotPrevious;
                yield return new WaitForSeconds(0.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }else{
                board.DestroyMatches();
            }
            //otherDot = null;
        }
    }
    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            fistTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngel();
    }
    private void CalculateAngel(){
        if (Mathf.Abs(finalTouchPosition.y - fistTouchPosition.y) > swipeResist ||
        Mathf.Abs(finalTouchPosition.x - fistTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(
                finalTouchPosition.y - fistTouchPosition.y,
                finalTouchPosition.x - fistTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentState = GameState.wait;
            board.currentDot = this;
        }else{
            board.currentState = GameState.move;
        }
    }
    private void MovePieces(){
        if(swipeAngle > -45 && swipeAngle <= 45 && dotPosition.x< board.size.x-1){
            //swipe right
            otherDot = board.allDots[dotPosition.x+1, dotPosition.y];
            dotPrevious = dotPosition;
            otherDot.GetComponent<Dot>().dotPosition.x-=1;
            dotPosition.x++;
        }else if(swipeAngle > 45 && swipeAngle <= 135 && dotPosition.y< board.size.y-1){
            //swipe up
            otherDot = board.allDots[dotPosition.x, dotPosition.y+1];
            dotPrevious = dotPosition;
            otherDot.GetComponent<Dot>().dotPosition.y-=1;
            dotPosition.y++;
        }else if((swipeAngle > 135 || swipeAngle <= -135) && dotPosition.x>0){
            //swipe left
            otherDot = board.allDots[dotPosition.x-1, dotPosition.y];
            dotPrevious = dotPosition;
            otherDot.GetComponent<Dot>().dotPosition.x+=1;
            dotPosition.x--;
        }else if(swipeAngle < -45 && swipeAngle >= -135 && dotPosition.y>0){
            //swipe down
            otherDot = board.allDots[dotPosition.x, dotPosition.y-1];
            dotPrevious = dotPosition;
            otherDot.GetComponent<Dot>().dotPosition.y+=1;
            dotPosition.y--;
        }
        StartCoroutine(CheckMoveCo());
    }
    private void FindMatches(){
        if(dotPosition.x>0 && dotPosition.x<board.size.x-1){
            GameObject leftDot1 = board.allDots[dotPosition.x-1, dotPosition.y];
            GameObject rightDot1 = board.allDots[dotPosition.x+1, dotPosition.y];

            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == gameObject.tag && rightDot1.tag == gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if(dotPosition.y>0 && dotPosition.y<board.size.y-1){
            GameObject downDot1 = board.allDots[dotPosition.x, dotPosition.y-1];
            GameObject upDot1 = board.allDots[dotPosition.x, dotPosition.y+1];
            if (downDot1 != null && upDot1 != null)
            {
                if (downDot1.tag == gameObject.tag && upDot1.tag == gameObject.tag)
                {
                    downDot1.GetComponent<Dot>().isMatched = true;
                    upDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
    public void MakeRowBomb(){
        isRowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.SetParent(transform);
    }
    public void MakeColumnBomb(){
        isColumnBomb = true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.SetParent(transform);
    }
}
