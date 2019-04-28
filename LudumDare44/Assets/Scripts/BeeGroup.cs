using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Encounter to handle bee groups acquiring honey.
/// </summary>
public class BeeGroup : MonoBehaviour {
    private beeColony _currentColony;
    private Region _currentRegion;
    private GameObject _beeWorldObj;
    private float _energyDrainRate;
    private float ENCOUNTER_TICK_RATE;
    private float _curColonyEnergy;
    private int _curColBeeCount;
    private int _potentialHoney;

    private const int HONEY_PER_TICK = 5;

    public delegate void EncounterFinishedEvent(int honeyGains);
    public EncounterFinishedEvent OnFinishedEncounter;

    public void InitializeGroup(beeColony c = null, Region r = null, float energyDrainRate = 0, float tickRate = 0) {
        _currentColony = c;
        _currentRegion = r;
        if (c && r) {
            _curColonyEnergy = Formulas.Instance.energyGather(HiveManager.Instance.CurrentHoney, _currentRegion, _currentColony);
            _curColBeeCount = c.numBees;
        }
        _energyDrainRate = energyDrainRate;
        ENCOUNTER_TICK_RATE = tickRate;
    }

    public void StartGroup() {
        StartCoroutine(TickCoroutine());
    }

    IEnumerator TickCoroutine() {
        //calculate time to region
        float _timeToTravel = _currentRegion.HiveDistance / _currentColony.Speed;
        //go to region
        yield return StartCoroutine(MoveGameObjectTo(_currentRegion.transform.position, _currentColony.Speed, _timeToTravel));

        int determinedGatherAmt = Formulas.Instance.totalHoneyGathered(_currentRegion, _currentColony);
        //at the cool food place, checking conditions per tick for completion
        bool isDoneGathering = false;
        while (!isDoneGathering) {
            _potentialHoney += _currentRegion.DrainHoney(HONEY_PER_TICK);
            _curColonyEnergy -= _energyDrainRate;

            if (_potentialHoney >= determinedGatherAmt ||
                _currentRegion.RemainingHoneyCapacity <= 0 ||
                _curColonyEnergy <= 0f) {
                isDoneGathering = true;
            }

            yield return new WaitForSeconds(ENCOUNTER_TICK_RATE);//tick rate
            yield return 0f;
        }
        //go back to the colony
        yield return StartCoroutine(MoveGameObjectTo(HiveManager.Instance.transform.position, _currentColony.Speed, _timeToTravel));
        if (OnFinishedEncounter != null) OnFinishedEncounter(_potentialHoney);
        HiveManager.Instance.CurrentHoney += _potentialHoney;
        _currentColony.IsBusy = false;
        yield return 0f;
        Destroy(gameObject);
    }

    /// <summary>
    /// handles moving bees on the screen to the region
    /// </summary>
    /// <param name="go"></param>
    /// <param name="targetLocation"></param>
    /// <param name="travelSpeed"></param>
    /// <returns></returns>
    IEnumerator MoveGameObjectTo(Vector3 targetLocation, float travelSpeed, float timeToReach) {
        //move to the hive within
        while (timeToReach > 0) {
            transform.position = Vector3.MoveTowards(transform.position, targetLocation, Time.deltaTime * travelSpeed);
            timeToReach -= Time.deltaTime;
            yield return 0f;
        }
        yield return 0f;
    }
}
