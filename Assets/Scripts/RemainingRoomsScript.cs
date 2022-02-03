using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingRoomsScript : MonoBehaviour
{
    public static int remainingRoomsValue = 0;
    public static int allRoomsValue = 0;
    Text remainingRooms;

    // Start is called before the first frame update
    void Start()
    {
        remainingRooms = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        remainingRooms.text = "Remaining Rooms:\n" + remainingRoomsValue + "/" + allRoomsValue;
    }
}
