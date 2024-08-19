using UnityEngine;

public class WizardAnimationEvents : MonoBehaviour
{
    public void PlayFootStep()
    {
        GameManager.Instance.audioSystem.PlayWizardFootstep();
    }

    public void PlayStaffBonk()
    {
        GameManager.Instance.audioSystem.PlayWizardStaffBonk();
    }

    public void PlayStartCharge()
    {
        GameManager.Instance.audioSystem.PlayWizardChargeStart();
    }

    public void PlayChargeTwo()
    {
        GameManager.Instance.audioSystem.PlayWizardChargeTwo();
    }

    public void PlayChargeThree() 
    {
        GameManager.Instance.audioSystem.PlayWizardChargeThree();
    }

    public void PlayExplode()
    {
        GameManager.Instance.audioSystem.PlayWizardExplode();
    }


}
