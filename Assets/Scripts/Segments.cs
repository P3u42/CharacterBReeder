using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segments : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 endingPosition;
    public float lenght;


    public void Follow(Vector3 targetPosition)
    {
        endingPosition =  targetPosition;
        Vector3 difference = endingPosition-startingPosition;
        Vector3 normalized = difference.normalized;
        startingPosition = endingPosition-(normalized*lenght);
    }

    public void AnchorStartAt(Vector3 targetPosition)
    {
        Vector3 difference = endingPosition-startingPosition;
        startingPosition = targetPosition;
        endingPosition =startingPosition+difference;
    }
    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 difference = targetPosition-startingPosition;
        endingPosition =startingPosition+(difference.normalized*lenght);
    }
    public void AnchorEndAt(Vector3 targetPosition)
    {
        Vector3 diffrence = endingPosition-startingPosition;
        endingPosition=targetPosition;
        startingPosition =endingPosition-diffrence;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
