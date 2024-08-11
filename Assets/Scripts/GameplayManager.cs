using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    int modeSelectedNo=0;
    // Mode Selection UI
    public GameObject modeSelectionCheck;
    public List< GameObject> modeButtons=new List<GameObject>();

    // Game Play Panel References
    public GameObject gamePlayDialoguesParent;
    public GameObject congratsPanel;
    // Main Menu panel references 
    public GameObject mainMenuScreen;
    public GameObject playScreen;
    public GameObject dialoguesParent;
    public GameObject modeSelectionPanel;

    public static GameplayManager instanceGM;

    private void Awake()
    {
        instanceGM = this;
    }

    private void Start()
    {
        mainMenuScreen.SetActive(true);
    }

    public void SelectMode(int ModeNo)
    {
        modeSelectedNo = ModeNo;
        modeSelectionCheck.transform.SetParent(modeButtons[modeSelectedNo].transform);
        modeSelectionCheck.transform.localPosition=new Vector3(-267,0);
        GameSoundManager.InsSoundManager.PlaySound("ButtonClicked");

    }

    public void PlayTheGame()
    {
        playScreen.SetActive(true);
        CardsManager.instanCM.AssignCards(modeSelectedNo);
        dialoguesParent.SetActive(false);
        modeSelectionPanel.SetActive(false);
        mainMenuScreen.SetActive(false);
        GameSoundManager.InsSoundManager.PlaySound("ButtonClicked");
    }
    

    public void PopUpDialogue(GameObject panel)
    {
        panel.transform.parent.gameObject.SetActive(true);
        panel.transform.localScale=Vector3.zero;
        panel.SetActive(true);
        panel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        GameSoundManager.InsSoundManager.PlaySound("ButtonClicked");
    }

    public void PopOutDialogue(GameObject panel)
    {
        panel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            panel.SetActive(false);
            panel.transform.parent.gameObject.SetActive(false);
        });
        GameSoundManager.InsSoundManager.PlaySound("ButtonClicked");
    }

    public void ShowLevelCompletePanel()
    {
        PopUpDialogue(congratsPanel);

        Invoke("LevelCompletedSound",0.3f);
    }
    public void LevelCompletedSound()
    {
        GameSoundManager.InsSoundManager.PlaySound("LevelComplete");
    }
    public void MoveToHome()
    {
        playScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        gamePlayDialoguesParent.SetActive(false);
        congratsPanel.SetActive(false);
        CardsManager.instanCM.ThingsDoneBackToHome();
        GameSoundManager.InsSoundManager.PlaySound("ButtonClicked");
    }

    public void RestartTheGame()
    {
        gamePlayDialoguesParent.SetActive(false);
        congratsPanel.SetActive(false);
        CardsManager.instanCM.RestartTheGamePlay(modeSelectedNo);
        GameSoundManager.InsSoundManager.PlaySound("ButtonClicked");
    }
}
