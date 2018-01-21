using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour {

    [SerializeField]
    private Image timerBar;

    [SerializeField]
    private QuizManager qm;

    [SerializeField]
    float questionTime;


    // Update is called once per frame
    void Update () {
        
        if (qm.getIsAnswered() == false)
        {
            float timer = 1.0f / questionTime * Time.deltaTime;
            timerBar.fillAmount -= timer;      
        }
        else
        {
            qm.setTimer(timerBar.fillAmount);
            Debug.Log("Time Spent: " + (1.0f - timerBar.fillAmount));
        }

        if(timerBar.fillAmount == 0)
        {
            qm.UserSelect(5);
        }
	}
}
