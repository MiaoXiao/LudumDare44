using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles interaction between Colony and Region
/// </summary>
public class Encounter : MonoBehaviour
{
    private beeColony _currentColony = null;
    private Region _currentRegion = null;

    private const int HONEY_PER_TICK = 5;
    private const float ENCOUNTER_TICK_RATE = 1.0f;

    [SerializeField]
    private float _energyDrainMovement = 0.1f;
    [SerializeField]
    private float _energyDrainRate = 0.1f;

    [SerializeField]
    private GameObject _beeObjTemplate;
    private GameObject _beeWorldObj; //use this to move some bee image or something when you send a bee over to a region and back

    public delegate void EncounterFinishedEvent();
    public EncounterFinishedEvent OnFinishedEncounter;

    public void PopulateInfo(beeColony c, Region r) {
        _currentColony = c;
        _currentRegion = r;
        
        //Update UI stuff here
    }

    public void StartEncounter() {
        EncounterGroup group = new EncounterGroup(_currentColony, _currentRegion, _energyDrainRate, _energyDrainMovement, ENCOUNTER_TICK_RATE);
    }

}

class EncounterGroup : MonoBehaviour {
    public bool isEncounterActive = false;
    private beeColony _currentColony;
    private Region _currentRegion;
    private GameObject _beeWorldObj;
    private float _energyDrainRate;
    private float _energyDrainMovement;
    private float _tickRate;
    private float _curColonyEnergy;
    private int _curColBeeCount;
    private int _potentialHoney;

    private const int HONEY_PER_TICK = 5;

    public EncounterGroup(beeColony c = null, Region r = null, float energyDrainRate = 0, float movementCost = 0, float tickRate = 0) {
        _currentColony = c;
        _currentRegion = r;
        if (c && r) {
            _curColonyEnergy = c.Energy;
            _curColBeeCount = c.numBees;
        }
        _energyDrainRate = energyDrainRate;
        _energyDrainMovement = movementCost;
    }

    IEnumerator TickCoroutine() {
        //calculate time to region
        float _timeToTravel = _currentRegion.HiveDistance / _currentColony.Speed;
        _curColonyEnergy -= _energyDrainMovement;

        //go to region
        yield return StartCoroutine(MoveGameObjectTo(_beeWorldObj, _currentRegion.transform.position, _currentColony.Speed, _timeToTravel));

        //at the cool food place, checking conditions per tick for completion
        bool isDoneGathering = false;
        while (!isDoneGathering) {
            _potentialHoney += _currentRegion.DrainHoney(HONEY_PER_TICK);
            _curColonyEnergy -= _energyDrainRate;

            if (_potentialHoney >= _currentColony.HoneyCapacity ||
                _currentRegion.RemainingHoneyCapacity <= 0 ||
                _curColonyEnergy - _energyDrainMovement <= 0f) {
                isDoneGathering = true;
            }

            yield return new WaitForSeconds(ENCOUNTER_TICK_RATE);//tick rate
            yield return 0f;
        }

        _curColonyEnergy -= _energyDrainMovement;
        //go back to the colony
        yield return StartCoroutine(MoveGameObjectTo(_beeWorldObj, HiveManager.Instance.transform.position, _currentColony.Speed, _timeToTravel));
        HiveManager.Instance.CurrentHoney += _potentialHoney;

        if (OnFinishedEncounter != null) OnFinishedEncounter();
        yield return 0f;
    }

    /// <summary>
    /// handles moving bees on the screen to the region
    /// </summary>
    /// <param name="go"></param>
    /// <param name="targetLocation"></param>
    /// <param name="travelSpeed"></param>
    /// <returns></returns>
    IEnumerator MoveGameObjectTo(GameObject go, Vector3 targetLocation, float travelSpeed, float timeToReach) {
        //move to the hive within
        while (timeToReach > 0) {
            go.transform.position = Vector3.MoveTowards(go.transform.position, targetLocation, Time.deltaTime * travelSpeed);
            timeToReach -= Time.deltaTime;
        }
        yield return 0f;
    }
}
