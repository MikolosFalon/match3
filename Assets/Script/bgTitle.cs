using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgTitle : MonoBehaviour
{
    [SerializeField] private List<GameObject> dots;
    private void Start() {
        //Initialize();
    }
   private void Initialize(){
        int dotToUse = Random.Range(0, dots.Count);
        GameObject dot = Instantiate(dots[dotToUse], transform.position, Quaternion.identity);
        dot.transform.SetParent(transform);
        dot.name = name;
    }
}
