using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beeColony : MonoBehaviour {
    //Public Field
    [SerializeField]
    public int numUpgrades = 0;

    [SerializeField]
    public int numBees = 5;

    //Private field
    private int upgradeCost = 5;
    private int honeyCost = 200;
    private float _upgradeEnergy = 0f;

    private Trait[] beeTraits = new Trait[3];
    private float _speed = 1.0f; //****1.0/1 distance / time
    private float _energy = 0f;
    private float _honeyCapacity = 10f;
    private int _antiPredator = 0;

    private int _tempRes = 0; //spectrum from 10 to -10, where 10 is a cold bee group and -10 is a hot bee group

    private bool _isBusy = false;
    private bool _traitFull = false;

    #region properties
    public bool IsBusy {
        get { return _isBusy; }
        set { _isBusy = value; }
    }
    public float Speed {
        get { return _speed; }
    }

    public float Energy {
        get { return _energy; }
    }

    public float EnergyUpgrade{
        get { return _upgradeEnergy; }
    }

    public float HoneyCapacity {
        get { return _honeyCapacity; }
    }

    public float TemperatureResistance {
        get { return _tempRes; }
    }

    public bool TraitFull{
        get { return _traitFull; }
    }

    public int AntiPredator{
        get { return _antiPredator; }
    }
    #endregion
    void Awake(){
        
    }
    
    void Update(){
        _energy = ( HiveManager.Instance.CurrentHoney / 4 ) + upgradeEnergy;
        /*
        if (Input.GetKey("w")){
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (speed * Time.deltaTime), 0);
        }
        if (Input.GetKey("s")){
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (-1 * speed * Time.deltaTime), 0);
        }
        if (Input.GetKey("a"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + (-1 * speed * Time.deltaTime), gameObject.transform.position.y, 0);
        }
        if (Input.GetKey("d"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + (speed * Time.deltaTime), gameObject.transform.position.y, 0);
        }
        if (Input.GetKeyDown("space"))
        {
            addBees(10);
        }
        */
    }

    public void addBees(int honeyAmount){
        numBees += 1;
        Debug.Log(numBees);
        honeyCost += upgradeCost * numUpgrades;
        Debug.Log(honeyCost);
        //TODO: add subtration functionality here, update grabbed singleton grabbed [honeyAmt - honeyCost]
    }

    public void addTrait(Trait newT){
        beeTraits[numUpgrades] = newT;

        //updating values
        _tempRes += newT.temperatureRes;
        _antiPredator += newT.predatorPrevention;
        HiveManager.Instance.DNAGenerationPerSecond += newT.passiveDNA;
        upgradeEnergy += newT.maxEnergy;
        HiveManager.Instance.HoneyLossPerInterval += newT.honeyConsumption;
        _speed += newT.beeSpeedMod;
        honeyCost -= newT.beeRefund;

        numUpgrades++;
        if (numUpgrades >= 3) {
            _traitFull = true;
        }
        else{
            _traitFull = false;
        }
    }
}
