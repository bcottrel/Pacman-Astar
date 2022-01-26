using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControllerOLD : MonoBehaviour
{
    public Transform[] dotArray;
    public GameObject dot;
    public GameObject bigDot;
    public int arrayLength;
    

    int bigDotlocation1;
    int bigDotlocation2;
    int bigDotlocation3;
    int bigDotlocation4;

    // Start is called before the first frame update
    void Start()
    {
        arrayLength = dotArray.Length;
        bigDotlocation1 = Random.Range(0, 190);
        bigDotlocation2 = Random.Range(0, 190);      
        bigDotlocation3 = Random.Range(0, 190);
        bigDotlocation4 = Random.Range(0, 190);
        
        
        for (int i = 0; i < dotArray.Length; i++)
        {
            if(i == bigDotlocation1 || i == bigDotlocation2 || i == bigDotlocation3 || i ==bigDotlocation4)
            {
                Instantiate(bigDot, dotArray[i]);
            }
            else
            {
                Instantiate(dot, dotArray[i]);
            }           
        }       
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
