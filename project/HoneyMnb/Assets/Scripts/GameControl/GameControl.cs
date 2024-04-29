using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeGame : MonoBehaviour
{
    Progress progress;

    // Start is called before the first frame update
    void Start()
    {
        progress = GameObject.Find("Progress").GetComponent<Progress>();
    }

    // Update is called once per frame
    void Update()
    {
        if(progress.prog >= 100) {
            switch (SceneManager.GetActiveScene().name)
            {
                case "straight":
                    SceneManager.LoadScene("zigzag");
                    break;

                case "zigzag":
                    SceneManager.LoadScene("curve");
                    break;

                case "curve":
                    break;              
            }
        }
    }
}
