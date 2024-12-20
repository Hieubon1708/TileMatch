using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePopup2 : MonoBehaviour
{
    public BBUIView popup;
    public BBUIButton btnMusic, btnSound, btnRate;
    public Image iconSound;
    public Sprite soundOn, soundOff;
    public Image iconMusic;
    public Sprite musicOn, musicOff;
    public BBUIButton btnHome, btnRestart, btnContinue;
    public BBUIButton btnClose;
    public GameObject lockGroup;

    // Start is called before the first frame update
    void Start()
    {
        popup.ShowBehavior.onCallback_Completed.AddListener(ShowView_Finished);
        popup.HideBehavior.onCallback_Completed.AddListener(HideView_Finished);

        btnHome.OnPointerClickCallBack_Completed.AddListener(TouchHome);
        btnRestart.OnPointerClickCallBack_Completed.AddListener(TouchRestart);
        btnContinue.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnSound.OnPointerClickCallBack_Completed.AddListener(TouchSound);
        btnMusic.OnPointerClickCallBack_Completed.AddListener(TouchMusic);
        btnRate.OnPointerClickCallBack_Completed.AddListener(TouchRate);

        ShowIconMusic();
        ShowIconSound();
    }

    enum STATE_CLOSEPOPUP
    {
        CONTINNUE, RESTART, HOME, LEVELSELECT
    }

    STATE_CLOSEPOPUP stateClosePopup = STATE_CLOSEPOPUP.CONTINNUE;

    // Update is called once per frame
    void Update()
    {

    }


    int level = 1;
    public void OpenPausePopup(int _level)
    {
        Config.interstitialAd_countPause++;
        level = _level;
        gameObject.SetActive(true);
        ShowViews();
    }

    private void ShowViews()
    {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.SetActive(true);



        StartCoroutine(ShowViews_IEnumerator());
    }

    private IEnumerator ShowViews_IEnumerator()
    {
        popup.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        popup.gameObject.SetActive(true);
        popup.ShowView();

        yield return new WaitForSeconds(0.2f);
        btnClose.gameObject.SetActive(true);
        btnClose.GetComponent<BBUIView>().ShowView();
    }


    private void ShowIconSound()
    {
        iconSound.sprite = Config.isSound ? soundOn : soundOff;
    }

    private void ShowIconMusic()
    {
        iconMusic.sprite = Config.isMusic ? musicOn : musicOff;
    }


    private void ShowView_Finished()
    {
        lockGroup.SetActive(false);
    }
    private void HideView_Finished()
    {
        if (stateClosePopup == STATE_CLOSEPOPUP.CONTINNUE)
        {
            Time.timeScale = 1f;
            GamePlayManager.instance.SetUnPause();
        }
        else if (stateClosePopup == STATE_CLOSEPOPUP.RESTART)
        {
            SceneManager.LoadScene("TileGame");
        }
        else if (stateClosePopup == STATE_CLOSEPOPUP.HOME)
        {
            SceneManager.LoadScene("MainGame");
        }
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void TouchHome()
    {
        stateClosePopup = STATE_CLOSEPOPUP.HOME;
        lockGroup.gameObject.SetActive(true);
        UnityEvent onInter = new UnityEvent();
        onInter.AddListener(delegate
        {
           /* CanvasFade.instance.LoadScene(delegate
            {*/
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainGame");
            //});
        });
        ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("tilematch_home", onInter);
    }

    private void TouchRestart()
    {
        stateClosePopup = STATE_CLOSEPOPUP.RESTART;
        lockGroup.gameObject.SetActive(true);
        UnityEvent onInter = new UnityEvent();
        onInter.AddListener(delegate
        {
            /*CanvasFade.instance.LoadScene(delegate
            {*/
                Time.timeScale = 1f;
                SceneManager.LoadScene("TileGame");
           // }, null, false);
        });
        ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("tilematc_restart", onInter);
    }

    public void Show_Static_Ad() { }

    private void TouchClose()
    {
        Debug.LogError("GAME CONTINNUE");
        stateClosePopup = STATE_CLOSEPOPUP.CONTINNUE;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    private void TouchSound()
    {
        Config.SetSound(!Config.isSound);

        ShowIconSound();
    }

    private void TouchMusic()
    {
        Config.SetMusic(!Config.isMusic);
        if (Config.isMusic)
        {
            MusicManager.instance.PlayMusicBG();
        }
        else
        {
            MusicManager.instance.StopMusicBG();
        }
        ShowIconMusic();
    }

    private void TouchRate()
    {
        GamePlayManager.instance.OpenRatePopup();
    }
}
