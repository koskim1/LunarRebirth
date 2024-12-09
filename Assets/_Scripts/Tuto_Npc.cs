using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto_Npc : MonoBehaviour
{    
    [SerializeField] private GameObject Tuto_Board;
    [SerializeField] private GameObject Tuto_01;
    [SerializeField] private GameObject Tuto_02;
    [SerializeField] private GameObject Tuto_03;
    [SerializeField] private GameObject Tuto_04;
    [SerializeField] private GameObject Tuto_05;
    [SerializeField] private GameObject Tuto_06;
    [SerializeField] private GameObject Tuto_07;

    private GameObject[] tutorialPages;
    private int currentIndex = 0;

    [SerializeField] private Button NextBtn;
    [SerializeField] private Button PrevBtn;
    [SerializeField] private Button CloseBtn;

    private void Awake()
    {
        Tuto_Board.SetActive(false);

        tutorialPages = new GameObject[] {
            Tuto_01,
            Tuto_02,
            Tuto_03,
            Tuto_04,
            Tuto_05,
            Tuto_06,
            Tuto_07
        };

        NextBtn.onClick.AddListener(NextPage);
        PrevBtn.onClick.AddListener(PrevPage);
        CloseBtn.onClick.AddListener(CloseTutorial);
    }

    
    public void OpenTutorial()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        Tuto_Board.SetActive(true);
        currentIndex = 0;
        ShowPage(currentIndex);
    }
    
    private void ShowPage(int index)
    {
        for(int i=0;i<tutorialPages.Length; i++)
        {
            tutorialPages[i].SetActive(false);
        }

        tutorialPages[index].SetActive(true);

        PrevBtn.interactable = (index > 0);
        NextBtn.interactable = (index < tutorialPages.Length - 1);
    }

    public void NextPage()
    {
        if (currentIndex < tutorialPages.Length - 1)
        {
            currentIndex++;
            ShowPage(currentIndex);
        }
    }

    public void PrevPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowPage(currentIndex);
        }
    }

    public void CloseTutorial()
    {
        Tuto_Board.SetActive(false);
        Time.timeScale = 1f;        
        Cursor.visible = false;
    }
}
