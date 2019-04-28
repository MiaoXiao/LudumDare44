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
        return  HiveManager.Instance.HoneyLossPerInterval * timeDif * 2;
    }

    /// <summary>
    /// Get's total energy used for gathering while at a region.
    /// Travel Costs included
    /// </summary>
    /// <param name="honeyTotal"></param>
    /// <param name="rNum"></param>
    /// <param name="colonyNum"></param>
    /// <returns></returns>
    public float energyGather(int honeyTotal, Region rNum, beeColony colonyNum){
        float energyOffset = 0f;
        energyOffset = energyCost(rNum, colonyNum);
        if ((honeyTotal / 4) - energyOffset <= 0)
        {
            return 0;
        }
        else
        {
            return (honeyTotal / 4) - energyOffset + colonyNum.EnergyUpgrade;
        }
    }

    //TODO: Predator & Temperature formulas
    public void killBees(Region rNum, beeColony colonyNum){
        int tempBees = 0;
        for (int i = 0; i < colonyNum.numBees - 1; ++i){
            if(Random.Range(0, 7 + colonyNum.AntiPredator) < rNum.PredatorLevel){ 
                tempBees++;
            }
        }
        colonyNum.numBees -= tempBees;
    }

    public float tInfluence(Region rNum, beeColony colonyNum){
        float tValue = Mathf.Abs(rNum.Temperature + colonyNum.TemperatureResistance);
        return (20 - tValue) / 20;
    }

    public float showPotential(Region rNum, beeColony colonyNum){
        return tInfluence(rNum, colonyNum) * 100;
        //returns % value of potential honey gain, can multiply later to view how much gain there actually is
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
