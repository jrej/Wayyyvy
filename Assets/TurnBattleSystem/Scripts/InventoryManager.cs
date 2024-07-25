using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool active ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && active)
        {

            InventoryMenu.SetActive(false);
            active =false; 
        }

        else if (Input.GetKeyDown(KeyCode.W) && !active)
        {

            InventoryMenu.SetActive(true);
            active = true;
        }
        
    }
}
