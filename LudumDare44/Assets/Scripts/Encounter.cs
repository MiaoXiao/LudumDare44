using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles interaction between Colony and Region
/// </summary>
public class Encounter : MonoBehaviour
{
    beeColony _currentColony = null;
    Region _currentRegion = null;

    private int _potentialHoney = 0;
    private float _energyRemaining = 0;

    [SerializeField]
    private float _energyDrainMovement = 0.1f;
    [SerializeField]
    private float _energyDrainRate = 0.1f;

    public void PopulateInfo(beeColony c, Region r) {
        _currentColony = c;
        _currentRegion = r;
        _energyRemaining = c.Energy;
    }

    public void StartEncounter() {
        if(!_currentColony || !_currentRegion) {
            return; //no region or colony set
        }

        StartCoroutine(TickCoroutine());
    }

    IEnumerator TickCoroutine() {
        //calculate time to region
        float _timeToTravel = _currentRegion.HiveDistance / _currentColony.Speed;
        _energyRemaining -= _energyDrainMovement * 2;
        yield return new WaitForSeconds(_timeToTravel);

        //at the cool food place, checking conditions per tick for completion
        bool isDoneGathering = false;
        while (!isDoneGathering) {
            _potentialHoney += _currentRegion.DrainHoney(5);
            _energyRemaining -= _energyDrainRate;

            if(_potentialHoney >= _currentColony.HoneyCapacity ||
                _currentRegion.RemainingHoneyCapacity <= 0 ||
                _energyRemaining <= 0f) {
                isDoneGathering = true;
            }

            yield return new WaitForSeconds(1f);//tick rate
            yield return 0f;
        }

        yield return new WaitForSeconds(_timeToTravel);

        yield return 0f;
    }
}
