using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    public void IncreaseScore(int amountToIncrease){
        score += amountToIncrease;
        scoreText.text = score.ToString();
    }
}
