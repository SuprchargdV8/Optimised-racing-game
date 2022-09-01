using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private string[] vehicles = {"Starter", "Sedan", "Truck", "Formula"};
    public int[] prices = {0, 500, 2000, 10000};
    private int ticketPrice = 20000;
    public GameObject Starter;
    public GameObject Sedan;
    public GameObject Truck;
    public GameObject Formula;
    private GameObject[] vehiclesOBJ = new GameObject[4];
    public int selected = 0;
    public Transform selectedCar;
    public Car Car{private set; get;}
    private float speed;
    private GameObject mainCamera;
    private GameObject mapCamera;
    private AudioSource error;

    public MenuManager menu;

    public int coinCount;
    public bool noDrive = false;
    public bool fillupCar;
    // Start is called before the first frame update
    void Awake()
    {
        error = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey("PlayerMoney")){
            coinCount = PlayerPrefs.GetInt("PlayerMoney");
        } else {
            coinCount = 0;
        }

        if(!PlayerPrefs.HasKey("Multiplier")){
            PlayerPrefs.SetInt("Multiplier", 1);
            PlayerPrefs.Save();
        }
        vehiclesOBJ[0] = Starter;
        vehiclesOBJ[1] = Sedan;
        vehiclesOBJ[2] = Truck;
        vehiclesOBJ[3] = Formula;

        foreach(string carName in vehicles){
            if(!PlayerPrefs.HasKey(carName)){
                if(carName.Equals("Starter")){
                    PlayerPrefs.SetInt(carName, 1);
                } else {
                    PlayerPrefs.SetInt(carName, 0);
                }
            }
        }
        PlayerPrefs.Save();

        mainCamera = GameObject.Find("MainCamera");

        mapCamera = GameObject.Find("MapFilmer");
        switchCar();
        menu.updateCoins();
        
    }

    public void switchCar(){

        mainCamera.GetComponent<CameraFollow>().TargetName = vehicles[selected];
        selectedCar = vehiclesOBJ[selected].GetComponent<Transform>();

        foreach(GameObject singleCar in vehiclesOBJ){
            if(singleCar.name.Equals(vehicles[selected])){
                Car = singleCar.GetComponent<Car>();
                Car.enabled = true;
                Car.GetComponent<AudioSource>().enabled = true;
            } else {
                singleCar.GetComponent<Car>().enabled = false;
                singleCar.GetComponent<AudioSource>().enabled = false;
            }
        }

        Car.getGas();

    }

    private void finnishGame(){
        int temp = PlayerPrefs.GetInt("Multiplier");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("Multiplier", temp * 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2);
    }

    public void changeCar(){
        selected = (selected+1)%4;
        switchCar();
        if(PlayerPrefs.GetInt(vehicles[selected]) != 1){
            noDrive = true;
            menu.buyCar(selected);
        } else {
            noDrive = false;
            menu.hideBuyMenu();
        }
    }

    public void buyCar(){
        if(coinCount >= prices[selected]){
            coinCount = coinCount - prices[selected];
            PlayerPrefs.SetInt(vehicles[selected], 1);
            PlayerPrefs.SetInt("PlayerMoney", coinCount);
            PlayerPrefs.Save();
            menu.hideBuyMenu();
            menu.enablePlayMenu();
            menu.updateCoins();
            noDrive = false;
        } else {
            menu.notEnoughCash();
        }
    }

    public void SaveGas(){
        PlayerPrefs.SetInt($"{vehicles[selected]}Gas", (int) Car.currGass);
        PlayerPrefs.Save();
    }

    void OnTriggerEnter(Collider other){

        if(other.gameObject.tag.Equals("GasStop")){
            fillupCar = true;
            SaveGas();
        }

        if(other.gameObject.tag.Equals("BuyTicket")){
            if(coinCount >= ticketPrice){
                finnishGame();
            } else {
                error.Play();
            }
        }


    }

    void OnTriggerExit(Collider other){

        if(other.gameObject.tag.Equals("GasStop")){
            fillupCar = false;
            SaveGas();
        }

    }

    void Update()
    {
        Vector3 temp = transform.position;
        temp.y = temp.y + 140;

        mapCamera.transform.position = temp;
        
        if(!noDrive){
            Car.Steer = GameManager.Instance.InputController.steer;
            Car.Throttle = GameManager.Instance.InputController.throttle;
            Car.Brake = GameManager.Instance.InputController.brake;
            speed = Car.Speed;
        }

        if(fillupCar & Car.Speed < 2){
            if(Car.currGass < Car.maxGas){ 
                Car.currGass += 1;
            } else {
                Car.currGass = Car.maxGas;
            }
        }

    }
}
