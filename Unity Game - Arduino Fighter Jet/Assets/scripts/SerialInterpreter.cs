using UnityEngine;
using System.Collections.Generic;

public class SerialInterpreter : MonoBehaviour
{
    public SerialController serialController;
    public CubeMovement mevement;

    private void Awake()
    {
        serialController =
            GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    private void Update()
    {
        string message = serialController.ReadSerialMessage();
        if (message == null)
        {
            return;
        }

        if (CheckForConnectionEvent(message))
        {
            DispatchInformation(InterpretMessage(message));
        }
    }

    bool CheckForConnectionEvent(string message)
    {
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
        {
            Debug.Log("Connection established");
            return false;
        }
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
        {
            Debug.Log("Connection attempt failed or disconnection detected");
            return false;
        }
        else
        {
            //Debug.Log("Message arrived: " + message);
            return true;
        }
    }

    List<string> InterpretMessage(string message)
    {
        int header;
        string[] content = message.Split(';');
        //Debug.Log("header: " + content[0]);
        header = int.Parse(content[0]);
        if (header == 0)
        {
            Debug.Log("No header found");
            return null;
        }
        if (header > 3)
        {
            Debug.LogWarning("Unknown Header");
            return null;
        }

        List<string> tempList = new List<string>();

        for (int i = 0; i < content.Length; i++)
        {
            tempList.Add(content[i]);
        }

        return tempList;
    }

    void DispatchInformation(List<string> content)
    {
        if (content == null)
        {
            return;
        }

        int header = int.Parse(content[0]);
        if (header == 0)
        {
            return;
        }
        else if (header == 1)
        {
            Vector3 rot = UpdatePlayerDir(content);

            //mevement.UpdateRotation(rot.x, rot.y, rot.z);
            //mevement.UpdateRotation2(rot.x, rot.y, rot.z);
            mevement.UpdateRotation3(rot);
            //Update Player Dir

            return;
        }
        else if (header == 2)
        {
            CheckTrigger(int.Parse(content[1]));
            //act on trigger
            return;
        }
        else if (header == 3)
        {
            UpdatePlayerDir(content);
            CheckTrigger(int.Parse(content[4]));

            //update player dir
            //act on trigger

            return;
        }
    }

    Vector3 UpdatePlayerDir(List<string> content)
    {
        for (int i = 1; i < content.Count; i++)
        {
            content[i] = content[i].Replace('.', '.');
        }

        Vector3 playerDir = new Vector3(
            float.Parse(content[1]),
            float.Parse(content[2]),
            float.Parse(content[3]));

        return playerDir;
    }

    void CheckTrigger(int index)
    {

    }
}
