using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetCameraPos : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private string str;
    [SerializeField]
    private bool _bool;
    [SerializeField]
    private Button _button;

    private Animator camAnim;
	// Use this for initialization
	void Start () {
        camAnim = cam.GetComponent<Animator>();
        _button.onClick.AddListener(() => SetAnimState());
	}
	
    public void SetAnimState()
    {
        camAnim.SetBool(str, _bool);
    }
}
