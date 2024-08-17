using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public CinemachineBrain brain;
    public CinemachineVirtualCamera gameVirtualCamera;
    public CinemachineVirtualCamera noInputGameVirtualCamera;

    private bool allowInput = true;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            EnableCameraInputReading(allowInput);
            allowInput = !allowInput;
        }
    }

    public void EnableCameraInputReading(bool canReadInput)
    {
        //CinemachineBrain brain = CinemachineCore.Instance.GetActiveBrain(0);
        if (brain == null)
        {
            Debug.Log("no brain");
            return;
        }

        ICinemachineCamera activeCam = brain.ActiveVirtualCamera;
        if (activeCam == null)
        {
            Debug.Log("No active cam");
            return;
        }

        if (activeCam.Name == gameVirtualCamera.name && !canReadInput)
        {
            Debug.Log("priority " + noInputGameVirtualCamera.Priority);

            // cant read input set priority of no input cam higher else lower priority          
            noInputGameVirtualCamera.Priority = 12;// !canReadInput ? 12 : 9;
            Debug.Log("No input is higher priority cam" + !canReadInput + " priority " + noInputGameVirtualCamera.Priority);
            //AxisState.IInputAxisProvider input = gameVirtualCamera.GetInputAxisProvider();
            Debug.Log("New prio " + activeCam.Priority);
        }
        else if (activeCam.Name == noInputGameVirtualCamera.name && canReadInput)
        {
            noInputGameVirtualCamera.Priority = 9;
            Debug.Log("back to having input");
            Debug.Log("prio rest " + activeCam.Priority);
        }
        else
        {
            Debug.Log("active cam = " + activeCam.Name + " input state unchanged");
            return;
        }
    }
}