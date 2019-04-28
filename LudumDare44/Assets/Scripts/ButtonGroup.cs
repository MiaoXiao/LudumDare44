using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField]
    private Transform _buttonparent;

    [SerializeField]
    private Image _selectGraphic;

    private Button _currentlySelectedButton;
    Button[] _allButtons;

    private void Start()
    {
        _allButtons = new Button[_buttonparent.childCount];
        for(int i = 0; i < _buttonparent.childCount; ++i)
        {
            Button button = _buttonparent.GetChild(i).GetComponent<Button>();
            _allButtons[i] = button;
        }

        for (int i = 0; i < _allButtons.Length; ++i)
        {
            Button button = _allButtons[i];
            _allButtons[i].onClick.AddListener(
                delegate {
                    ShowSelectedButton(button);
                });
        }
     }

    private void ShowSelectedButton(Button b)
    {
        _selectGraphic.transform.position = b.transform.position;
    }
}
