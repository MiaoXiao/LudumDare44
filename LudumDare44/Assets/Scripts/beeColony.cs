using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beeColony : MonoBehaviour
{
    //Public Field
    public int numUpgrades = 0;

    //Private field
    private int numBees = 5;
    private int upgradeCost = 5;

    private float speed = 1.0f; //****1.0/1 distance / time
    private float energy = 1.5f; //****1.5/1 honeyGathered / honeyTotal

    private int tempRes = 0; //spectrum from 10 to -10, where 10 is a cold bee and -10 is a hot bee

    void Awake(){
        
    }
    
    void Update(){
        
    }

    public void addBees(int honeyAmount){
        numBees += 1;
        int honeyCost = 0;
        honeyCost = honeyAmount + (upgradeCost * numUpgrades);
        //TODO: add subtration functionality here, update grabbed singleton grabbed [honeyAmt - honeyCost]
    }
}
