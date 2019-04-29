using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TraitData", menuName = "TraitValues", order = 1)]
public class Trait : ScriptableObject
{
    [TextArea]
    public string description = "Give a description here.";
    public Sprite icon = null; 

    public int temperatureRes = 0; //int from 10 to -10 (hot -1, cold +1)
    public int predatorPrevention = 0; //each point adds 1 number to probability, 1/8 -> 1/9 -> 1/10 etc
    public float beeSpeedMod = 0f; //change in speed of bee
    public int honeyConsumption = 0; //change in honey intake

    public int passiveDNA = 0; //DNA gain?
    //public int honeyGain = 0; //Extra honey gain because of temperature?
    public float maxEnergy = 0f; //energy modifier
    public int beeRefund = 0; //less honey comsumed when making bees

}
