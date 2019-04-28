using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles interaction between Colony and Region
/// </summary>
public class Encounter : MonoBehaviour
{
    private beeColony _currentColony = null;
    private Region _currentRegion = null;

    private const float ENCOUNTER_TICK_RATE = 1.0f;

    [SerializeField]
    private readonly float _energyDrainRate = 0.1f;

    [SerializeField]
    private BeeGroup _beeObjTemplate;

    [SerializeField]
    private Button _embarkButton;

    public void Update() {
        _embarkButton.interactable = !_currentColony.IsBusy && _currentRegion && !_currentRegion.IsOccupied;
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
        if ((!_currentRegion && !_currentColony) || _currentColony.IsBusy) return;
        _currentColony.IsBusy = true;
        _currentRegion.IsOccupied = true;
        Debug.Log("Sending out a bee from Colony: " + _currentColony + " to Region: " + _currentRegion.transform.position);
        BeeGroup group = Instantiate(_beeObjTemplate, transform.position, Quaternion.identity);
        group.InitializeGroup(_currentColony, _currentRegion, _energyDrainRate, ENCOUNTER_TICK_RATE);
        group.StartGroup(); 
    }

}

