using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uREPL;

public class Loader : MonoBehaviour {

    // Use this for initialization
	void Start () {
        Load();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Load()
    {
        var prefab = AssetHelper.Load<GameObject>("uREPL/Prefabs/uREPL.prefab");
        var prefabMain = prefab.GetComponent<Main>();
        var instance = UnityEngine.Object.Instantiate(prefab);
        var window = instance.GetComponent<Window>();
        window.Submit("5 + 5", false);
    }
}
