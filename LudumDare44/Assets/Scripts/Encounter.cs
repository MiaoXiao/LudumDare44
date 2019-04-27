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

    private int potentialHoney = 0;

    public void PopulateInfo(beeColony c, Region r) {
        _currentColony = c;
        _currentRegion = r;
    }

    public void StartEncounter() {
        if(!_currentColony || !_currentRegion) {
            return; //no region or colony set
        }

        StartCoroutine(TickCoroutine());
    }

    IEnumerator TickCoroutine() {
        float traveledDist = 0f;

        //travel to the region
        while (traveledDist < _currentRegion.HiveDistance) {
            traveledDist += _currentColony.speed * Time.deltaTime;

            yield return 0f;

        }

        //at the cool food place
        while (_currentColony) {

            yield return 0f;
        }

        traveledDist = 0;
        //travel back to hive
        while (traveledDist < _currentRegion.HiveDistance) {
            traveledDist += _currentColony.speed * Time.deltaTime;

            yield return 0f;

        }

        yield return 0f;
    }
}
