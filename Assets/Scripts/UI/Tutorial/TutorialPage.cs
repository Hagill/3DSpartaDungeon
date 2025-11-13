using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    public int pageIndex;
    public UITutorial container;

    private void Awake()
    {
        Transform prevBtn = transform.Find("Prev");
        if (prevBtn != null)
        {
            prevButton = prevBtn.GetComponent<Button>();
        }

        Transform nextBtn = transform.Find("Next");
        if (nextBtn != null)
        {
            nextButton = nextBtn.GetComponent<Button>();
        }

        if (prevButton != null)
        {
            prevButton.onClick.AddListener(OnPrevClicked);
        }
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextClicked);
        }
    }

    void OnPrevClicked()
    {
        container.ShowPage(pageIndex - 1);
    }

    void OnNextClicked()
    {
        if (pageIndex < container.PageCount - 1)
        {
            container.ShowPage(pageIndex + 1);
        }
        else
        {
            container.CloseTutorial();
        }
    }
}
