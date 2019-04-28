using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds region data and defines actions upon a region
/// </summary>
public class Region : MonoBehaviour
{
    [SerializeField]
    private int _maxHoney = 0;
    [SerializeField]
    private int _currentHoney = 0;
    [SerializeField]
    private float _distanceToHive = 0f;
    private int _temperature = 0;
    private int _predatorLevel = 0;

    #region properties
    public int MaxHoneyCapacity {
        get { return _maxHoney; }
    }

    public int RemainingHoneyCapacity {
        get { return _currentHoney; }
    }

    public float HiveDistance {
        get { return _distanceToHive; }
    }

    public int Temperature {
        get { return _temperature; }
    }

    public int PredatorLevel {
        get { return _predatorLevel; }
    }
    #endregion

    public delegate void RegionSelectedEvent(Region r);
    public RegionSelectedEvent OnRegionSelected;

    public delegate void OutOfHoneyEvent(Region r);
    public OutOfHoneyEvent OnOutOfHoney;

    /// <summary>
    /// Sets region data
    /// </summary>
    /// <param name="hivePosition">hive's position</param>
    /// <param name="temperature"></param>
    /// <param name="predatorLevel"></param>
    public void SetRegionData(Vector3 regionPosition, int temperature, int predatorLevel) {
        this._temperature = Mathf.Clamp(temperature, -10, 10);
        this._predatorLevel = Mathf.Clamp(predatorLevel, 0, 1);
        transform.position = regionPosition;
        _distanceToHive = (transform.position - HiveManager.Instance.transform.position).magnitude;

        _maxHoney = _currentHoney = 50;
    }

    /// <summary>
    /// Fires when clicked on
    /// </summary>
    public void OnMouseDown() {
        if (OnRegionSelected != null && _currentHoney > 0) OnRegionSelected(this);
    }

    /// <summary>
    /// return the amount drained after attempting to remove drainValue from reserves
    /// </summary>
    /// <param name="drainValue"></param>
    /// <returns></returns>
    public int DrainHoney(int drainValue) {
        _currentHoney -= drainValue;
        Mathf.Clamp(_currentHoney, 0, _maxHoney);
        if (_currentHoney > drainValue) {
            return drainValue;
        }
        else {
            StartCoroutine(DelayedRemove());
            return Mathf.Clamp(_currentHoney, 0, drainValue);
        }
    }

    IEnumerator DelayedRemove() {
        yield return new WaitForSeconds(1.1f);
        if(OnOutOfHoney!=null) OnOutOfHoney(this);
        Destroy(gameObject);
    }
}
