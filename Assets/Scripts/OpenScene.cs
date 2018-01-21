using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour {


         
	public void OpenSceneNum(int i)
    {
        SceneManager.LoadScene(i);
    }
}
 