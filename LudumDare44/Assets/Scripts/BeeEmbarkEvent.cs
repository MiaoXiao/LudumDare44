using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeEmbarkEvent : MonoBehaviour
{
    [SerializeField]
    Text[] _embarkingText;

    [SerializeField]
    beeColony[] c;

    private Dictionary<beeColony, Text> _embarkTextBinding = new Dictionary<beeColony, Text>();
    private void Awake()
    {
        for (int i = 0; i < 3; ++i)
        {
            _embarkTextBinding.Add(c[i], _embarkingText[i]);
        }
    }

    public void UpdateEmbarkText(beeColony c, bool active)
    {
        _embarkTextBinding[c].gameObject.SetActive(active);
    }

}
