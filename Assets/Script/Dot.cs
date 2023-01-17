using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 fistTouchPosition;
    private Vector2 finalTouchPosition;

    [SerializeField] private float swipeAngle = 0;

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
        
        
    }
}
