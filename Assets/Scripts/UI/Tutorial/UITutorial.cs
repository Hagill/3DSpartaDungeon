using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private Transform tutorialContainer;

    private List<GameObject> pages = new List<GameObject>();
    private int curPage = 0;
    public int PageCount => pages.Count;

    private PlayerController controller;

    private void Awake()
    {
        pages.Clear();
        for (int i = 0; i < tutorialContainer.childCount; i++)
        {
            GameObject pageObject = tutorialContainer.GetChild(i).gameObject;
            pages.Add(pageObject);

            TutorialPage pageScript = pageObject.GetComponent<TutorialPage>();
            if (pageScript != null)
            {
                pageScript.pageIndex = i;
                pageScript.container = this;
            }
        }
    }

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;

        controller.tutorial += ToggleTutorial;

        tutorialContainer.gameObject.SetActive(true);
        ShowPage(curPage);
    }
    public void ShowPage(int index)
    {
        if (index < 0 || index >= pages.Count) { return; }

        curPage = index;
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == index);
        }
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }

    public void ToggleTutorial()
    {
        if (IsOpen())
        {
            tutorialContainer.gameObject.SetActive(false);
        }
        else
        {
            tutorialContainer.gameObject.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return tutorialContainer.gameObject.activeInHierarchy;
    }
}
