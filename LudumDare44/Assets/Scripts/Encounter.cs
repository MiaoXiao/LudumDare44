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
        if (Input.GetKeyDown(KeyCode.W)) {
            PopulateInfo(TESTCOLONY, TESTREGION);
            StartEncounter();
        }
    }
    public void PopulateInfo(beeColony c, Region r) {
        _currentColony = c;
        _currentRegion = r;
        
        //Update UI stuff here
    }

    public void StartEncounter() {
        BeeGroup group = Instantiate(_beeObjTemplate, transform.position, Quaternion.identity);
        group.InitializeGroup(_currentColony, _currentRegion, _energyDrainRate, _energyDrainMovement, ENCOUNTER_TICK_RATE);
        group.StartGroup(); 
    }

}

