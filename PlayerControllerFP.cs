using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Really simple character controller.
/// Just for moving capsule.
/// </summary>
public class PlayerControllerFP : MonoBehaviour {

   
	void Update () {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 250.0f;
        if (x < 0.1) transform.Rotate(0, 0, 0);
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 9.0f;
      
        transform.Rotate(0, x, 0);
        transform.position += transform.forward * z;
    }
}
