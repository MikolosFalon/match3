using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgTitle : MonoBehaviour
{
    public int hitPoints;
    private SpriteRenderer sr;
    private void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage) {
        hitPoints -= damage;
        MakeLighter();
        if(hitPoints <= 0){
            Destroy(this.gameObject);
        }
    }

    private void MakeLighter(){
        //take the current color
        Color color = sr.color;
        //Get the current color`s alpha value and cut it in half.
        float newAlpha = color.a*0.5f;
        sr.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
