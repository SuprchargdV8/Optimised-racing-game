using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public Canvas PauseMenu;
    public Canvas PlayMenu;
    public Canvas DeliveryMenu;
    public Canvas BuyMenu;
    public Canvas InfoMenu;
    public Player player;
    public TaskController task;
    public Text coins;
    public Text taskAddress;
    public Text UImultiplier;
    public Text UInumberDelivered;
    public Text UIPPI;
    public Text UItip;
    public Text UItotal;

    public Text carName;
    public Text price;
    public Text infos;

    public Text score;

    public GameObject notEnoughCashText;
    private bool removeTimer;
    private float timer = 1f;

    public RectTransform gasGauge;

    void Start(){
        player = GameObject.Find("Player").GetComponent<Player>();
        PauseMenu.enabled = false;
        DeliveryMenu.enabled = false;
        InfoMenu.enabled = false;
        BuyMenu.enabled = false;
        Time.timeScale = 1;

        if(!PlayerPrefs.HasKey("TutorialMenu")){
            PauseGame();
            openInfo();
            PlayerPrefs.SetInt("TutorialMenu", 1);
            PlayerPrefs.Save();
        }
    }

    public void HideDeliveryMenu(){
        DeliveryMenu.enabled = false;
        PlayMenu.enabled = true;
    }

    
    public void updateDeliveryMenu(int premiumMult, int itemsDelivered, int pricePerItem, int tip, int price){
        UImultiplier.text = $"{premiumMult}";
        UInumberDelivered.text = $"{itemsDelivered}";
        UIPPI.text = $"{pricePerItem}";
        UItip.text = $"{tip}";
        UItotal.text = $"{price}";
        DeliveryMenu.enabled = true;
        PlayMenu.enabled = false;
    }

    public void enablePlayMenu(){
        PlayMenu.enabled = true;
    }

    public void disablePlayMenu(){
        PlayMenu.enabled = false;
    }


    public void PauseGame(){
        //remove noise
        PlayMenu.enabled = false;
        PauseMenu.enabled = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame(){
        //play noise
        PlayMenu.enabled = true;
        PauseMenu.enabled = false;
        Time.timeScale = 1;
    }

    public void updateAddress(){
        taskAddress.text = task.address;
    }

    public void updateCoins(){
        coins.text = $"{player.coinCount}";
    }

    public void buyCar(int carID){
        BuyMenu.enabled = true;
        switch(player.selected){
            case 1:
                carName.text = "SPORT";
                price.text = $"{player.prices[player.selected]}";
                infos.text = "5-10\n5x";
                break;
            case 2:
                carName.text = "TRUCK";
                price.text = $"{player.prices[player.selected]}";
                infos.text = "10-100\n3x";
                break;
            case 3:
                carName.text = "FORMULA";
                price.text = $"{player.prices[player.selected]}";
                infos.text = "3-7\n100x";
                break;
            default:
                carName.text = "BUGGY";
                price.text = $"0";
                infos.text = "1-3\n1x";
                break;
        }

    }

    void Update(){
        if(removeTimer){
            timer = timer - Time.deltaTime;
            if(timer < 0){
                timer = 1f;
                notEnoughCashText.SetActive(false);
                removeTimer = false;
            }
        }

        float percGas = player.Car.currGass / player.Car.maxGas;
        Vector3 tempP = gasGauge.localPosition;

        tempP.x = -130 + 150 * percGas;

        gasGauge.localPosition = tempP;

        tempP.x = percGas;
        tempP.y = 1f;
        tempP.z = 1f;

        gasGauge.localScale = tempP;


        if(task.taskID > -1){
            score.text = $"{task.boostName[task.boostID]} SCORE : {task.boostScore}";
        } else {
            score.text = "NO DELIVERY ACTIVE";
        }
    }

    public void openInfo(){
        InfoMenu.enabled = true;
        PauseMenu.enabled = false;
    }

    
    public void closeInfo(){
        InfoMenu.enabled = false;
        PauseMenu.enabled = true;
    }

    public void notEnoughCash(){
        removeTimer = true;
        timer = 1f;
        notEnoughCashText.SetActive(true);
    }

    public void hideBuyMenu(){
        BuyMenu.enabled = false;
    }

}
