using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDNA : MonoBehaviour
{
    public float r;
    public float g;
    public float b;
    public float bodyR;
    public float bodyG;
    public float bodyB;
    public int healthGene;
    public float sCaleGene;
    public bool isDead= false;
    public float deathTime = 0;
    public float bodySize;
    public int bodyLengthGene;
    public float bodyWidthGene;
    public float speed;
    public int damageGene;
    public int damageCaused;
    private int enemyFitness;

    public bool fitnessSetByUser = false;
    
    
    [SerializeField]SpriteRenderer enemySprite;
    // Start is called before the first frame update
    void Start()
    {
        enemySprite =GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  int  GetFitness()
    {
        return enemyFitness;
    }
    public void SetFitness(int fitness)
    {
        this.enemyFitness =  fitness;
    }

    public void SetFitnessFromSlider(float sliderValue){
        enemyFitness = Mathf.RoundToInt(sliderValue);
        fitnessSetByUser = true;
        Debug.Log($"Fitness definido pelo slider{enemyFitness}");
    }
    public void ResetFitness(){
        enemyFitness = 0;
        fitnessSetByUser = false;
    }
}
