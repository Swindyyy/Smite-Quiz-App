using UnityEngine;
using System.Collections;

public class RotateSkybox : MonoBehaviour {

    [SerializeField]
    float adjustment;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * adjustment);
    }
}
