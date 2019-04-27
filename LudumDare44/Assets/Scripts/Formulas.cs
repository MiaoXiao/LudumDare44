using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formulas : Singleton<Formulas>
{
    //Time helper formula
    private float timeTaken(Region rNum, beeColony colonyNum){
        return rNum.HiveDistance / colonyNum.Speed;
    }

    //TODO: Energy Formulas
    private float energyCost(Region rNum, beeColony colonyNum){
        float timeDif = 0f;
        timeDif = timeTaken(rNum, colonyNum);
        return  5 * timeDif * 2; //TODO: take the offset value of drain honey and change this to the offset variable
    }

    private float energyGather(int honeyTotal, Region rNum, beeColony colonyNum){
        float energyOffset = 0f;
        energyOffset = energyCost(rNum, colonyNum);
        if ((honeyTotal / 4) - energyOffset <= 0)
        {
            return 0;
        }
        else
        {
            return (honeyTotal / 4) - energyOffset;
        }
    }

    //TODO: Predator & Temperature formulas
    public void killBees(Region rNum, beeColony colonyNum){
        int tempBees = 0;
        for (int i = 0; i < colonyNum.numBees - 1; ++i){
            if(Random.Range(0,7) < rNum.PredatorLevel){
                tempBees++;
            }
        }
        colonyNum.numBees -= tempBees;
    }

    public float tInfluence(Region rNum, beeColony colonyNum){
        float tValue = Mathf.Abs(rNum.Temperature + colonyNum.TemperatureResistance);
        return (20 - tValue) / 20;
    }

    //TODO: Honey formulas
    public int totalHoneyGathered(Region rNum, beeColony colonyNum){
        int honeyAmount = 500; //TODO: grab value from hive
        int predatorInfluence = 20;
        int energyInfluence = 2;
        float energyVal = energyGather(honeyAmount, rNum, colonyNum);
        return Mathf.CeilToInt( (rNum.PredatorLevel * predatorInfluence) + (energyVal * energyInfluence) );
    }
}
