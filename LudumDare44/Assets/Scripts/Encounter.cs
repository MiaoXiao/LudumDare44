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

    private const float ENCOUNTER_TICK_RATE = 1.0f;

    [SerializeField]
    private float _energyDrainMovement = 0.1f;
    [SerializeField]
    private float _energyDrainRate = 0.1f;

    [SerializeField]
    private BeeGroup _beeObjTemplate;

    [SerializeField]
    private beeColony TESTCOLONY;
    [SerializeField]
    private Region TESTREGION;

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            PopulateInfo(TESTCOLONY, TESTREGION);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            StartEncounter();
        }
    }
    public void PopulateInfo(beeColony c, Region r) {
        if(c != null) _currentColony = c;
        if(r != null) _currentRegion = r;

        Debug.Log(_currentColony + " | " + _currentRegion);

        if (_currentColony && _currentRegion) {
            //Update UI stuff here
        }
    }

    public void StartEncounter() {
        BeeGroup group = Instantiate(_beeObjTemplate, transform.position, Quaternion.identity);
        group.InitializeGroup(_currentColony, _currentRegion, _energyDrainRate, _energyDrainMovement, ENCOUNTER_TICK_RATE);
        group.StartGroup(); 
    }

}

