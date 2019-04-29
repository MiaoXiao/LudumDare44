using UnityEngine;

public class SelectRegionUi : MonoBehaviour
{
    public void Set(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
   } 
}
