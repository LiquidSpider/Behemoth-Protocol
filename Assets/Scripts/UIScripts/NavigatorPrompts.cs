using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigatorPrompts : MonoBehaviour {
	public Text promptText;
	public TMPro.TextMeshProUGUI tmpPrompt;
	public GameObject audioobject;
    public AudioClip[] navLines = new AudioClip[18];
    public AudioClip radioOn;
    public AudioClip radioOff;

    private bool noLegWithArmCalled = false;
    private AutoHide ah;
    private AudioSource audio;

    bool isOff = true;
    bool[] flags = new bool[18]; // To stop certain voicelines from repeating. False = not yet played. Set to true to stop it from repeating.

    private void Awake()
    {
        //ah = promptText.transform.parent.GetComponent<AutoHide>();
        audio = audioobject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (audio != null)
        {
            if (!audio.isPlaying && !isOff)
            {
                audio.PlayOneShot(radioOff, 0.75f);
                isOff = true;
            }
        }
    }

    private void VoiceLine(int index)
    {
        if (flags[index] == true)
        {
            return;
        }
        isOff = false;
        audioobject.GetComponent<AudioSource>().Stop();
        audioobject.GetComponent<AudioSource>().PlayOneShot(radioOn, 0.35f);
        audioobject.GetComponent<AudioSource>().clip = navLines[index];
        audioobject.GetComponent<AudioSource>().Play();
        promptText.transform.parent.GetComponent<AutoHide>().appearTime = navLines[index].length;
    }

	public void CallIntroLine() {
		// Called from PlayerController.Start()
		promptText.text = "You've arrived! Quickly, enter the dam, your target is there.";
		tmpPrompt.text = "You've arrived! Quickly, enter the dam, your target is there.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(0);
	}

	public void CallFollowUpIntroLine() {
		// Called from PlayerController.DelayedIntroPrompt()
		promptText.text = "The dam's under attack again. You have to make sure the wall stays intact.";
		tmpPrompt.text = "The dam's under attack again. You have to make sure the wall stays intact.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(1);
    }

	private bool seenLineSeeingBoss = false;
	public void CallSeeingBoss() {
		if (!seenLineSeeingBoss) {
			seenLineSeeingBoss = true;

			// Called from MusicZone.OnTriggerEnter()
			promptText.text = "You need to stop it from reaching the wall! Destroy its legs to stop it moving.";
			tmpPrompt.text = "You need to stop it from reaching the wall! Destroy its legs to stop it moving.";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);
			VoiceLine(2);
			flags[2] = true;
		}
    }

	public void CallTakingPhysicalDamage() {
		// Called from PlayerDamageableSection.OnTriggerEnter()

		promptText.text = "Be careful, your suit won't survive everything. Make sure to avoid its attacks.";
		tmpPrompt.text = "Be careful, your suit won't survive everything. Make sure to avoid its attacks.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(3);
        flags[3] = true;
    }

	public void CallTakingMissileDamage() {
		// - Called from PlayerHealth.TakeDamage(float, GameObject)
		//promptText.text = "Looks like it has some pretty heavy weapons of it's own, look out for them.";


		// Called from PlayerDamageableSection.OnTriggerEnter()
		promptText.text = "Be careful, those missiles will destroy your armour, use you shield to reduce the damage.";
		tmpPrompt.text = "Be careful, those missiles will destroy your armour, use you shield to reduce the damage.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(4);
        flags[4] = true;
    }

	public void CallYellowZone() {
		// Called from PlayerContoller.OnTrigggerEnter()
		promptText.text = "Warning: Approaching 'No Fly' zone, reduce height.";
		tmpPrompt.text = "Warning: Approaching 'No Fly' zone, reduce height.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(5);
    }

	public void CallRedZone() {
		// Called from PlayerController.StopFlying()
		promptText.text = "Warning: Cutting engines to avoid detection. Prepare to fall.";
		tmpPrompt.text = "Warning: Cutting engines to avoid detection. Prepare to fall.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(6);
    }

	public void CallNoLegWithArms() {
		// Called from giantBehaviour.CheckHealth()
		if (!noLegWithArmCalled) {
			noLegWithArmCalled = true;

			promptText.text = "Good job, it won't be... Hold on, is it repairing itself?";
			tmpPrompt.text = "Good job, it won't be... Hold on, is it repairing itself?";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);
            VoiceLine(7);

            StartCoroutine(CallFollowUpNoLegWithArm());
		}
	}

	private IEnumerator CallFollowUpNoLegWithArm() {
		yield return new WaitForSeconds(navLines[7].length);
		promptText.text = "You'll have to destroy those arms first.";
		tmpPrompt.text = "You'll have to destroy those arms first.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(8);
    }

	public void CallBatteryLow() {
		// Called from PlayerHealth.Update()
		promptText.text = "Your suit is low on energy, use your vacuum to recharge it.";
		tmpPrompt.text = "Your suit is low on energy, use your vacuum to recharge it.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(9);
    }

	public void CallArmDestroyedOne() {
		// Called from giantBehaviour.CheckHealth()
		promptText.text = "Good job! Now you just have to destroy the other one.";
		tmpPrompt.text = "Good job! Now you just have to destroy the other one.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(10);

    }

	public void CallArmDestroyedTwo() {
		// Called from giantBehaviour.CheckHealth()
		promptText.text = "Nice! Now destroy the legs and it won't be able to go anywhere.";
		tmpPrompt.text = "Nice! Now destroy the legs and it won't be able to go anywhere.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(11);
    }

	private bool boolEnterFinalPhaseShown = false;
	public void CallEnterFinalPhase() {
		// Called from giantBehaviour.CheckHealth()
		if (!boolEnterFinalPhaseShown) {
			boolEnterFinalPhaseShown = true;
			promptText.text = "It's almost dead, now attack its core and destroy it for good!";
			tmpPrompt.text = "It's almost dead, now attack its core and destroy it for good!";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);
            VoiceLine(12);
        }
	}

	private IEnumerator Call30SecondsIntoFinal() {
		yield return new WaitForSeconds(30);

		promptText.text = "It's sending out a distress signal! It's going to give its all to destroy the wall, finish it quickly, you only have a few moments!";
		tmpPrompt.text = "It's sending out a distress signal! It's going to give its all to destroy the wall, finish it quickly, you only have a few moments!";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(13);
    }

	public void CallWin() {
		// Called from GameManager.COWin()
		promptText.text = "You did it! Excellent work today, you're clear to return to base.";
		tmpPrompt.text = "You did it! Excellent work today, you're clear to return to base.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(14);
    }

	public void CallLosePlayer() {
		// Called from PlayerHealth.Die();

		promptText.text = "Critical damage taken, return to base before you can’t.";
		tmpPrompt.text = "Critical damage taken, return to base before you can’t.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(15);
    }

	public void CallLoseWall() {
		promptText.text = "The wall’s down. Mission failed, fall back to base for now, we need to prepare for the next attack.";
		tmpPrompt.text = "The wall’s down. Mission failed, fall back to base for now, we need to prepare for the next attack.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(16);
    }

	public void CallLowHealth() {
		// Called from PlayerHealth.TakeDamage(float)
		// Called from PlayerHealth.TakeDamage(float, GameObject)

		promptText.text = "Major damage taken, back off and heal before re-engaging.";
		tmpPrompt.text = "Major damage taken, back off and heal before re-engaging.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
        VoiceLine(17);
    }
}
