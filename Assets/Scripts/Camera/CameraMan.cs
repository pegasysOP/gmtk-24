using Cinemachine;
using System;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public CinemachineBrain brain;
    public CinemachineVirtualCamera mainMenuVirtualCamera;
    public CinemachineVirtualCamera flyThroughVirtualCamera;
    public CinemachineVirtualCamera gameVirtualCamera;
    public CinemachineVirtualCamera noInputGameVirtualCamera;

    public Receptacle menuReceptacle;

    public GameObject introGameObj;
    public Animation introCamAnim;

    private bool allowInput = true;
    protected bool inMainMenu = true;

    public delegate void CameraTransition(string newCameraName, string previousCameraName);
    public static event CameraTransition OnCameraTransition;

    public void Start()
    {
        //CinemachineCore.CameraCutEvent.AddListener(OnCameraUpdated);
        brain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
    }

    private void OnCameraActivated(ICinemachineCamera newCam, ICinemachineCamera previousCam)
    {
        Debug.Log("cam 1: " + newCam.Name + " cam 2:" + previousCam.Name);
        // Camera has changed
        if (OnCameraTransition != null)
        {
            OnCameraTransition(newCam.Name, previousCam.Name);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("straight to game");
            mainMenuVirtualCamera.Priority = 8;
            gameVirtualCamera.Priority = 12;
            return;
        }

        if (menuReceptacle.IsCompleted())//Input.anyKeyDown)
        {
            // read for any input if current cam is main menu cam
            if (inMainMenu && IsVirtualCameraActive(mainMenuVirtualCamera))
            {
                {
                    Debug.Log("any input received menu cam -> fly cam");
                    TransitionFromMenuToIntro();
                }
            }
        }

        else if (Input.GetKeyUp(KeyCode.B) && !inMainMenu)
        {
            Debug.Log("key up");

            EnableCameraInputReading(allowInput);
            allowInput = !allowInput;
        }
    }

    /// <summary>
    /// Set bool for in main menu to false and changes main menu cam priority to 8 also enable random shit here to play intro anim. This should be less than the FlyThrough camera priority and the cinamchine will then automatically blend to that camera. get fukt.
    /// </summary>
    public void TransitionFromMenuToIntro()
    {
        inMainMenu = false;
        introGameObj.SetActive(true);
        mainMenuVirtualCamera.Priority = 8;
        introCamAnim.Play();
    }

    public void ForceGameCam()
    {
        mainMenuVirtualCamera.Priority = 8;
        gameVirtualCamera.Priority = 12;
    }

    public bool IsVirtualCameraActive(CinemachineVirtualCamera virtualCamera)
    {
        return IsVirtualCameraActive(virtualCamera.name);
    }

    public bool IsVirtualCameraActive(string virtualCameraName)
    {
        if (brain == null)
        {
            Debug.Log("no brain");
            return false;
        }

        ICinemachineCamera activeCam = brain.ActiveVirtualCamera;
        if (activeCam == null)
        {
            Debug.Log("No active cam");
            return false;
        }

        // check if active cam is same as one being checked
        if (activeCam.Name == virtualCameraName)
        {
            return true;
        }

        return false;
    }

    public void EnableCameraInputReading(bool allowInput)
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

        // swap between virtual cameras (game camera to no input game camera)
        if (activeCam.Name == gameVirtualCamera.name && !allowInput)
        {
            // cant read input set priority of no input cam higher          
            noInputGameVirtualCamera.Priority = 12;
            //AxisState.IInputAxisProvider input = gameVirtualCamera.GetInputAxisProvider();
        }
        // swap from no input game camera to regular game camera
        else if (activeCam.Name == noInputGameVirtualCamera.name && allowInput)
        {
            noInputGameVirtualCamera.Priority = 9;
            //Debug.Log("back to having input");
        }
        else
        {
            Debug.Log("active cam = " + activeCam.Name + " input state unchanged");
            return;
        }
    }
}