﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ACEPlay.Bridge;
public class BuyItemPopup : MonoBehaviour
{
    public static BuyItemPopup instance;

    public BBUIButton btnClose, btnWatchAds;
    public GameObject popup;
    public GameObject lockGroup;
    public Image icon;
    public Text txtCount;
    public Sprite sprite_Undo;
    public Sprite sprite_Suggest;
    public Sprite sprite_Shuffle;
    private void Awake()
    {
        instance = this;
    }

    private ConfigItemShopData configItemShop;
    public void OpenBuyItemPopup(Config.ITEMHELP_TYPE itemType)
    {
        configItemShop = new ConfigItemShopData();
        configItemShop.shopItemType = Config.ConvertItemHelpTypeToShopItem(itemType);
        configItemShop.countItem = 1;
        configItemShop.price = Config.COIN_PRICE_ITEM;
        Config.BuySucces_ItemShop(configItemShop);
        GamePlayManager.instance.SetFreeItem_Success();
    }

    private void ShowRewardItem()
    {
        if (configItemShop.shopItemType == Config.SHOPITEM.UNDO)
        {
            icon.sprite = sprite_Undo;
        }
        else if (configItemShop.shopItemType == Config.SHOPITEM.SUGGEST)
        {
            icon.sprite = sprite_Suggest;
        }
        else if (configItemShop.shopItemType == Config.SHOPITEM.SHUFFLE)
        {
            icon.sprite = sprite_Shuffle;
        }
        icon.SetNativeSize();
        txtCount.text = $"{configItemShop.countItem}";
    }
    public void TouchClose()
    {
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();

    }
    public void TouchBuyItem()
    {
        if (Config.currCoin >= configItemShop.price)
        {
            Config.SetCoin(Config.currCoin - configItemShop.price);       

            NotificationPopup.instance.AddNotification("Buy Success!");
        }
        else
        {
            NotificationPopup.instance.AddNotification("Not enough Coin!");
            //GamePlayManager.instance.OpenShopPopup();
        }

    }
    public void HidePopup_Finished()
    {
        GamePlayManager.instance.SetUnPause();
        gameObject.SetActive(false);
    }

    public void InitViews()
    {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnWatchAds.gameObject.SetActive(false);
        if (BridgeController.instance.IsRewardReady())
        {
            btnWatchAds.Interactable = true;
        }
        else
        {
            btnWatchAds.Interactable = false;
        }
        btnClose.gameObject.SetActive(false);
        InitView_ShowView();
    }

    public void InitView_ShowView()
    {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });


        sequenceShowView.InsertCallback(0.4f, () =>
        {
            btnWatchAds.gameObject.SetActive(false);
            btnWatchAds.gameObject.GetComponent<BBUIView>().ShowView();
        });
        sequenceShowView.InsertCallback(1.2f, () =>
        {
            btnClose.gameObject.SetActive(false);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();
        });
    }
}
