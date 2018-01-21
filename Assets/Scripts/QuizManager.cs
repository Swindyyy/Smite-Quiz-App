using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;


public class QuizManager : MonoBehaviour {

    public Question[] questions;
    
    private static List<Question> unansweredQ;

    private Question currentQ;
    private int currentAns;
    private List<string> godNames;
    private List<string> godTitles;
    private List<string> possAns;
    private int score;
    private int currentQNum;
    private ManagerScript manSc;
    private float timer;
    private bool answered;
    private Animator qzAnimator;

    [Header("Variables")]
    [SerializeField]
    private Text factText;

    [SerializeField]
    private Text ans0;

    [SerializeField]
    private Text ans1;

    [SerializeField]
    private Text ans2;

    [SerializeField]
    private Text ans3;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text qNumText;

    [SerializeField]
    private int timeForNextQ;

    [SerializeField]
    private Canvas QuizCanvas;

    [SerializeField]
    private Canvas EndCanvas;

    [SerializeField]
    private Text finalScore;

    [SerializeField]
    private int numOfQs;

    [SerializeField]
    private Text ansText;

    [SerializeField]
    private Text endScreenText;

    [SerializeField]
    private Button ans0but;

    [SerializeField]
    private Button ans1but;

    [SerializeField]
    private Button ans2but;

    [SerializeField]
    private Button ans3but;

    void Start()
    {
        manSc = ManagerScript.manager.GetComponent<ManagerScript>();
        qzAnimator = QuizCanvas.GetComponent<Animator>();
        currentQNum = manSc.GetQNum();  // Sets current question number (adds 1 as question 1 is coded as question 0)
        score = manSc.GetScore();
        scoreText.text = "Score: " + score.ToString();
        answered = false;

        if (currentQNum < (numOfQs+1))
        {
            qNumText.text = "Question: " + currentQNum + "/5";

            if (unansweredQ == null || unansweredQ.Count == 0)
            {
                unansweredQ = questions.ToList<Question>();
            }


            manSc.SetQNum(currentQNum + 1);
            SetQ();
        }
        else
        {
            qNumText.text = "Question: 5/5";
            LoadEndScreen();
        }      
        
    }


    void SetQ()
    {
        int qNum = Random.Range(0, unansweredQ.Count);
        currentQ = unansweredQ[qNum];
        string _ansType = currentQ.AnsType;
        possAns = currentQ.answers;

        factText.text = currentQ.quest;
        for(int i = 0; i<4; i++)
        {
            int _aNum = Random.Range(0, possAns.Count);
            string _ans = possAns[_aNum];
            if(i==0)
            {
                SetCurrentAns(i, _ans);
                ans0.text = _ans;
            }
            else  if(i==1)
            {
                SetCurrentAns(i, _ans);
                ans1.text = _ans;
            }
            else  if(i==2)
            {
                SetCurrentAns(i, _ans);
                ans2.text = _ans;

            }
            else
            {
                SetCurrentAns(i, _ans);
                ans3.text = _ans;
            }
            possAns.Remove(_ans);
        }

        unansweredQ.RemoveAt(qNum);
        qNumText.text = "Question: " + currentQNum + "/5";
        scoreText.text = "Score: " + score.ToString();

    }

    public void SetCurrentAns(int i,string ans)
    {
        if (ans == currentQ.answer)
        {
            currentAns = i;
        }
    }

    IEnumerator TransitionToNextQ()
    {
        unansweredQ.Remove(currentQ);

        yield return new WaitForSeconds(timeForNextQ);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void UserSelect(int i)
    {
        answered = true;
        DisableButtons();
        if(currentAns == 0)
        {
            qzAnimator.SetTrigger("TopLeftTrue");
        } else if (currentAns == 1)
        {
            qzAnimator.SetTrigger("TopRightTrue");
        }else if (currentAns == 2)
        {
            qzAnimator.SetTrigger("BotLeftTrue");
        }
        else
        {
            qzAnimator.SetTrigger("BotRightTrue");
        }

        if(i == currentAns)
        {
            Debug.Log("CORRECT");
            score += 1;
            manSc.SetScore(score);
            manSc.AddTimeSpentAnswering(timer);
            ansText.text = "CORRECT!";
            ansText.color = new Color(0f, 1f, 0f);
            ansText.enabled = true;
        }
        else if(i == 5)
        {
            Debug.Log("Out of time");
            ansText.text = "OUT OF TIME";
            ansText.color = new Color(1f,1f, 1f);
            ansText.enabled = true;

        }
        else
        {
            Debug.Log("WRONG");
            ansText.text = "WRONG!";
            ansText.color = new Color(1f, 0f, 0f);
            ansText.enabled = true;

        }

        StartCoroutine(TransitionToNextQ());
    }

    void LoadEndScreen()
    {
        QuizCanvas.gameObject.SetActive(false);
        EndCanvas.gameObject.SetActive(true);
        finalScore.text = "Final Score: " + score + " / " + numOfQs;
        endScreenText.text = getEndScreenText();
        int _favor = CalculateFavor();
        string quizType = SceneManager.GetActiveScene().name+"Favor";
        SaveFavor(quizType, _favor);
        
    }
    
    public void LoadMenu()
    {
        manSc.Reset();
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        manSc.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool getIsAnswered()
    {
        return answered;
    }

    public void setTimer(float f)
    {
        manSc.AddTimeSpentAnswering(f);
    }

    IEnumerator WaitTenthSec()
    {
        yield return new WaitForSeconds(0.1f);
    }

    private int CalculateFavor()
    {
        int _favor = 0;
        _favor += ((Mathf.FloorToInt(manSc.GetTimeSpentAnswering()))*(5/ score));
        return _favor;
    }

    private void SaveFavor(string s, int i)
    {
        string quizType = s;
        int _oldFavor = LoadPrevFavor(quizType);
        int _newFavor = AddNewFavor(_oldFavor, i);
        WriteNewFavor(quizType,_newFavor);

    }

    private int LoadPrevFavor(string s)
    {
        int favor = PlayerPrefs.GetInt(s);
        return favor;
    }

    private int AddNewFavor(int old, int addFav)
    {
        int _newFav = old + addFav;
        return 0;
    }

    private void WriteNewFavor(string s, int i)
    {
        PlayerPrefs.SetInt(s, i);
    }

    private string getEndScreenText()
    {
        string returnText;
        if(score == 0)
        {
            returnText = "That's a shame, better luck next time!";
        } else if(score == 1)
        {
            returnText = "Well, at least you got one right! Try again!";
        } else if(score==2)
        {
            returnText = "Not bad, but not terribly good";
        } else if(score==3)
        {
            returnText = "Not bad! You got 3 out of 5!";
        } else if(score==4)
        {
            returnText = "Nice job! You certainly know your Smite!";
        } else if(score==5)
        {
            returnText = "You know Smite like the back of your hand!";
        } else
        {
            returnText = "Nice job!";
        }
        return returnText;
    }

    private void DisableButtons()
    {
        ans0but.enabled = false;
        ans1but.enabled = false;
        ans2but.enabled = false;
        ans3but.enabled = false;
    }

    private void EnableButtons()
    {
        ans0but.enabled = true;
        ans1but.enabled = true;
        ans2but.enabled = true;
        ans3but.enabled = true;
    }
}
