using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskController : MonoBehaviour
{

    public GameObject Deliveries;
    public GameObject FoodItem;
    public GameObject PickukpItem;

    private Transform[] locations;

    public Transform activeTask;
    public Transform GPSrot;
    public Transform pickupLocation;
    private Player player;
    public int taskID;
    public string address;
    public bool isActive;

    public int price;
    public int tip;
    public int premiumMult;
    public int itemsDelivered;
    public int pricePerItem;

    public string[] boostName = {"AVG SPEED", "TOP SPEED", "DRIFT"};
    public int boostID = 0;
    public int boostScore;
    private int tempMult = 1;

    public MenuManager menu;

    // Start is called before the first frame update
    void Start()
    {
        locations = Deliveries.GetComponentsInChildren<Transform>();
        player = GetComponent<Player>();
        returnFromDelivery();
        tempMult = PlayerPrefs.GetInt("Multiplier");
    }

    void OnTriggerEnter(Collider other){

        if(other.name.Equals("Food Place")){
            if(!isActive){
                newTask();
            }
        } else if(other.name.Equals(address)){
            deliveryComplete();
            returnFromDelivery();
        }
    }

    private void newTask(){
        boostScore = 0;
        player.SaveGas();
        PickukpItem.SetActive(false);

        bool temp = false;

        boostID = Random.Range(0, 3);

        while(!temp || address.Equals("Food Place") || address.Equals("Delivery")){
            taskID = Random.Range(0, locations.Length);
            address = locations[taskID].name;
            temp = true;
        }
        FoodItem.SetActive(true);
        FoodItem.transform.position = locations[taskID].position;
        isActive = true;
        menu.updateAddress();
    }

    private void deliveryComplete(){
        player.SaveGas();
        pricePerItem = Random.Range(10, 15);

        switch(player.selected){
            case 1:
                itemsDelivered = Random.Range(3, 10);
                premiumMult = 5 * tempMult;
                break;
            case 2:
                itemsDelivered = Random.Range(10, 100);
                premiumMult = 4 * tempMult;
                break;
            case 3:
                itemsDelivered = Random.Range(3, 7);
                premiumMult = 100 * tempMult;
                break;
            default:
                itemsDelivered = Random.Range(1, 3);
                premiumMult = 2 * tempMult;
                break;
        }
        price = pricePerItem*itemsDelivered*premiumMult;
        tip = boostScore;
        price = price + tip;
        player.coinCount = player.coinCount + price;
        boostScore = 0;

        PlayerPrefs.SetInt("PlayerMoney", player.coinCount);
        PlayerPrefs.Save();
        menu.updateCoins();
        menu.updateDeliveryMenu(premiumMult, itemsDelivered, pricePerItem, tip, price);
    }

    public void cancelDelivery(){
        PickukpItem.SetActive(true);
        FoodItem.SetActive(false);
        returnFromDelivery();
    }


    private void returnFromDelivery(){
        PickukpItem.SetActive(true);
        FoodItem.SetActive(false);
        FoodItem.transform.position = pickupLocation.position;
        taskID = -1;
        address = "Pickup Delivery Food";
        isActive = false;
        menu.updateAddress();
    }

    void Update()
    {

        Vector3 direction = (FoodItem.transform.position - player.selectedCar.position).normalized;

        float tempRotation = Vector3.SignedAngle(player.selectedCar.forward, direction, Vector3.up);

        Quaternion tempRot = Quaternion.Euler(0, 0, -tempRotation);

        GPSrot.rotation = tempRot;

        switch(boostID){
            case 1:
                if(player.Car.Speed * 3 > boostScore){
                    boostScore = (int) player.Car.Speed * 3;
                }
                break;
            case 2:
                if(player.Car.Speed > 10 && player.Car.DriftAngle > 5){
                    boostScore += 1;
                }
                break;
            default:
                boostScore = (int)(boostScore*9 + player.Car.Speed)/10;
                break;
        }
    }
}
