using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StarGroup : MonoBehaviour
{
    public Sprite spriteStar_Normal;
    public Sprite spriteStar_Dark;

    public Image imgProgress;

    private int currStar = 3;
    const float SCORE_MAX = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Config.gameState == Config.GAME_STATE.PLAYING)
        {
            if (Config.CheckTutorial_1()) return;
            if (Config.CheckTutorial_2()) return;
            if (Config.CheckTutorial_3()) return;
            if (Config.CheckTutorial_4()) return;
            countDownTimeA += Time.deltaTime;
            //Debug.Log(countDownTimeA);
            if (isCountDownTime)
            {
                if (countDownTimeA > configLevelGame.timeA)
                {
                    score -= ConfigLevelGame.scoreInterval_Decrease * Time.deltaTime;
                    if (score <= 0)
                    {
                        score = 0;
                        //GamePlayManager.instance.SetGameLose();
                        isCountDownTime = false;
                    }
                    UpdateScore();
                }
            }
        }
    }
    private bool isCountDownTime = false;
    private ConfigLevelGame configLevelGame;
    public void InitStarGroup(ConfigLevelGame _configLevelGame)
    {
        configLevelGame = _configLevelGame;
        score = SCORE_MAX;
        isCountDownTime = true;
    }

    public void Revive_InitStarGroup()
    {
        //isCountDownTime = true;
        //score = SCORE_MAX;
        //currStar = 3;
    }

    private float countDownTimeA;

    public float score = 100f;

    public void UpdateScore()
    {
        imgProgress.fillAmount = score / 100f;
    }

    public void AddScore()
    {
        if (isCountDownTime)
        {
            countDownTimeA = 0;
            score += ConfigLevelGame.scoreInterval_Increase;
            if (score >= 100)
            {
                score = 100;
            }
            else if (currStar <= 2)
            {
                if (score >= configLevelGame.listScrore_Stars[currStar + 1])
                {
                    score = configLevelGame.listScrore_Stars[currStar + 1] - 0.01f;
                }
            }
            DOTween.Kill(imgProgress);
            imgProgress.DOFillAmount(score / 100f, 0.2f).SetEase(Ease.Linear);
        }
    }

    public int GetCurrStar()
    {
        return currStar;
    }
}
