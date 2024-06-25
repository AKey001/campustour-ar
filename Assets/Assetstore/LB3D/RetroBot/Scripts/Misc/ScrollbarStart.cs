using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollbarStart : MonoBehaviour
{

    void Start()
    {
        GetComponent<Scrollbar>().value = 1;
    }


}
