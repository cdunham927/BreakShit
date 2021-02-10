using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController GC;
    [Header("Main")]
    public string nam;
    public int money;

    public enum spawns { basePot, randomPot };
    spawns curSpawn = spawns.basePot;

    [Header("Pots")]
    public int listSize = 100;
    public int brokenListSize = 100;
    public GameObject basePot;
    List<GameObject> pots;
    public Pot.colors colorToSpawn;
    [Space]

    //broken pots lists
    [Header("Broken pots")]
    public GameObject brokenPot;
    [HideInInspector]
    public List<GameObject> brokenPots;
    [Space]

    [Header("Pot Controlling")]
    public int numPots = 0;
    [Range(3, 100)]
    public int maxPots = 10;
    float potCools = 0f;
    public Color curColor;
    bool random = false;
    [Space]

    //UI
    [Header("Stuff for UI")]
    public GameObject moneyUI;
    public Image maxPotsImg;
    [Range(0, 5)]
    public float smoothAmt;
    public Image colorImg;
    public Animator unlockAnim;

	void Awake () {
        //Set color to default
        curColor = Color.white;
        //Reset money
        money = 0;
        //Allow only 1 GC to exist at any time
        if (GC == null)
        {
            DontDestroyOnLoad(gameObject);
            GC = this;
        }
        else if (GC != this)
        {
            Destroy(gameObject);
        }

        #region Pot Object Pooling
        //Set up list of gameobjects to spawn
        pots = new List<GameObject>();
        for (int i = 0; i < listSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(basePot);
            obj.SetActive(false);
            pots.Add(obj);
        }

        //broken pots lists
        brokenPots = new List<GameObject>();
        for (int i = 0; i < brokenListSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(brokenPot);
            obj.SetActive(false);
            brokenPots.Add(obj);
        }
        #endregion
    }
	
	void Update () {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                changeRedPotSpawn();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                changeBluePotSpawn();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                changeRandomPotSpawn();
            }
        }

        if (potCools > 0) potCools -= Time.deltaTime;

        if (potCools <= 0) numPots = 0;

        moneyUI.GetComponent<Text>().text = "Money: " + money;
        maxPotsImg.fillAmount = Mathf.Lerp(maxPotsImg.fillAmount, (float)numPots / (float)maxPots, Time.deltaTime * smoothAmt);
	}

    public void SpawnPot()
    {
        if (numPots < maxPots)
        {
            numPots++;
            potCools = 5f;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnObj(new Vector3(mousePos.x, mousePos.y, 0f), Quaternion.Euler(0f, 0f, Random.Range(0, 360)));
        }
    }

    void spawnObj(Vector3 pos, Quaternion rot)
    {
        switch (curSpawn)
        {
            case spawns.basePot:
                GameObject toSpawn = getObjectFromList(pots);
                toSpawn.GetComponent<SpriteRenderer>().color = curColor;
                toSpawn.GetComponent<Pot>().col = colorToSpawn;
                toSpawn.transform.position = pos;
                toSpawn.transform.rotation = rot;
                toSpawn.SetActive(true);
                break;
            case spawns.randomPot:
                toSpawn = getObjectFromList(pots);
                toSpawn.GetComponent<SpriteRenderer>().color = curColor;
                toSpawn.transform.position = pos;
                toSpawn.transform.rotation = rot;
                toSpawn.SetActive(true);
                break;
        }
    }

    public void ChangeCameraPos(int pos)
    {
        Camera.main.GetComponent<CameraController>().curPos = pos-1;
    }

    #region Changing Pot Spawns
    public void changeBasePotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = Color.white;
        colorToSpawn = Pot.colors.white;
        colorImg.color = curColor;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.basePot;
        unlockAnim.Play("SlideIn");
    }

    public void changeRedPotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = Color.red;
        colorToSpawn = Pot.colors.red;
        colorImg.color = curColor;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.basePot;
        unlockAnim.Play("SlideIn");
    }

    public void changeGreenPotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = Color.green;
        colorToSpawn = Pot.colors.green;
        colorImg.color = curColor;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.basePot;
        unlockAnim.Play("SlideIn");
    }

    public void changeBluePotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = Color.blue;
        colorToSpawn = Pot.colors.blue;
        colorImg.color = curColor;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.basePot;
        unlockAnim.Play("SlideIn");
    }

    public void changeYellowPotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = Color.yellow;
        colorToSpawn = Pot.colors.yellow;
        colorImg.color = curColor;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.basePot;
        unlockAnim.Play("SlideIn");
    }

    public void changeOrangePotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = new Color32(255, 165, 0, 255);
        colorImg.color = curColor;
        colorToSpawn = Pot.colors.orange;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.basePot;
        unlockAnim.Play("SlideIn");
    }

    public void changePinkPotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = new Color32(255, 192, 203, 255);
        colorImg.color = curColor;
        colorToSpawn = Pot.colors.pink;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.basePot;
        unlockAnim.Play("SlideIn");
    }

    public void changeRandomPotSpawn()
    {
        unlockAnim.GetComponent<Button>().interactable = true;
        curColor = Random.ColorHSV();
        colorToSpawn = Pot.colors.random;
        colorImg.GetComponent<Animator>().Play("Fade");
        colorImg.color = curColor;
        colorImg.gameObject.SetActive(true);
        curSpawn = spawns.randomPot;
        unlockAnim.Play("SlideIn");
    }
    #endregion

    public void AnimateUnlocks()
    {
        unlockAnim.GetComponent<Button>().interactable = false;
        unlockAnim.Play("SlideOut");
    }

    public GameObject getObjectFromList(List<GameObject> lst)
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (!lst[i].activeInHierarchy)
            {
                return lst[i];
            }
            //Add code here if you want the list to
            //grow after being depleted
        }
        return null;
    }

    #region Data management
    public void resetData()
    {
        Debug.Log(Application.persistentDataPath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.nam = "";
        data.money = 0;

        bf.Serialize(file, data);
        file.Close();
    }
    
    //For saving and loading data
    private void OnEnable()
    {
        resetData();
    }

    private void OnDisable()
    {
        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.nam = nam;
        data.money = money;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            nam = data.nam;
            money = data.money;
        }
    }
    #endregion
}

[System.Serializable]
class PlayerData
{
    public string nam;
    public int money;
}
