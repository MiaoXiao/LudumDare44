using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideShowManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _allSlides;

    private int currentSlide = 0;

    public void NextSlide()
    {
        _allSlides[currentSlide].gameObject.SetActive(false);
        _allSlides[++currentSlide].gameObject.SetActive(true);
    }

    public void PrevSlide()
    {
        _allSlides[currentSlide].gameObject.SetActive(false);
        _allSlides[--currentSlide].gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("hivemanager");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
