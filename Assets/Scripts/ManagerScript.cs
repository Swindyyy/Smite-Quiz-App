using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ManagerScript : MonoBehaviour {

    [SerializeField]
    public static GameObject manager;

    int currentQNum;
    int currentScore;

    float timeSpentAnswering;

    void Awake()
    {
        if(manager)
        {
            Destroy(this);
        }
        else
        {
            manager = this.gameObject;
            DontDestroyOnLoad(this);
        }
    }

    // Use this for initialization
    void Start () {
        Reset();
        string _begFav = "BeginnerFavor";
        string _medFav = "MediumFavor";
        string _harFav = "HardFavor";
        string _trivFav = "TriviaFavor";

        if (!PlayerPrefs.HasKey(_begFav))
        {
            PlayerPrefs.SetInt(_begFav, 0);
        }
        if (!PlayerPrefs.HasKey(_medFav))
        {
            PlayerPrefs.SetInt(_medFav, 0);
        }
        if(!PlayerPrefs.HasKey(_harFav))
        {
            PlayerPrefs.SetInt(_harFav,0);
        }
        if (!PlayerPrefs.HasKey(_trivFav))
        {
            PlayerPrefs.SetInt(_trivFav, 0);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetQNum(int i)
    {
        currentQNum = i;
    }

    public int GetQNum()
    {
        return currentQNum;
    }

    public void SetScore(int i)
    {
        currentScore = i;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void Reset()
    {
        currentQNum = 1;
        currentScore = 0;
        timeSpentAnswering = 0f;
        Debug.Log("Reset has occured");
    }

    public float GetTimeSpentAnswering()
    {
        return timeSpentAnswering;
    }

    public void AddTimeSpentAnswering(float f)
    {
        timeSpentAnswering += f;
    }


}
