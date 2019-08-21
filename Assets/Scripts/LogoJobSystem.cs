using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoJobSystem : MonoBehaviour
{
    [SerializeField]Transform testTransform;
    public Vector3 MousePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log(UtilsClass.GetMouseWorldPosition());
             Vector3 pos = UtilsClass.GetMouseWorldPosition();

            testTransform.position = new Vector3(pos.x, pos.y, 10);



        }
       
    }
}
