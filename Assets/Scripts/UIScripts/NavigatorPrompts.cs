using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigatorPrompts : MonoBehaviour {
	public Text promptText;
	public TMPro.TextMeshPro tmpPrompt;

	private bool noLegWithArmCalled = false;

	public void CallIntroLine() {
		// Called from PlayerController.Start()
		promptText.text = "You've arrived! Quickly, enter the dam, your target is there.";
		tmpPrompt.text = "You've arrived! Quickly, enter the dam, your target is there.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallFollowUpIntroLine() {
		// Called from PlayerController.DelayedIntroPrompt()
		promptText.text = "The dam's under attack again. You have to make sure the wall stays intact.";
		tmpPrompt.text = "The dam's under attack again. You have to make sure the wall stays intact.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallSeeingBoss() {
		// Called from MusicZone.OnTriggerEnter()
		promptText.text = "You need to stop it from reaching the wall! Destroy its legs to stop it moving.";
		tmpPrompt.text = "You need to stop it from reaching the wall! Destroy its legs to stop it moving.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallTakingPhysicalDamage() {
		// Called from PlayerDamageableSection.OnTriggerEnter()

		promptText.text = "Be careful, your suit won't survive everything. Make sure to avoid its attacks.";
		tmpPrompt.text = "Be careful, your suit won't survive everything. Make sure to avoid its attacks.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallTakingMissileDamage() {
		// - Called from PlayerHealth.TakeDamage(float, GameObject)
		//promptText.text = "Looks like it has some pretty heavy weapons of it's own, look out for them.";


		// Called from PlayerDamageableSection.OnTriggerEnter()
		promptText.text = "Be careful, those missiles will destroy your armour, use you shield to reduce the damage.";
		tmpPrompt.text = "Be careful, those missiles will destroy your armour, use you shield to reduce the damage.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallYellowZone() {
		// Called from PlayerContoller.OnTrigggerEnter()
		promptText.text = "Warning: Approaching 'No Fly' zone, reduce height.";
		tmpPrompt.text = "Warning: Approaching 'No Fly' zone, reduce height.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallRedZone() {
		// Called from PlayerController.StopFlying()
		promptText.text = "Warning: Cutting engines to avoid detection. Prepare to fall.";
		tmpPrompt.text = "Warning: Cutting engines to avoid detection. Prepare to fall.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallNoLegWithArms() {
		// Called from giantBehaviour.CheckHealth()
		if (!noLegWithArmCalled) {
			noLegWithArmCalled = true;

			promptText.text = "Good job, it won't be... Hold on, is it repairing itself?";
			tmpPrompt.text = "Good job, it won't be... Hold on, is it repairing itself?";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);

			StartCoroutine(CallFollowUpNoLegWithArm());
		}
	}

	private IEnumerator CallFollowUpNoLegWithArm() {
		yield return new WaitForSeconds(4);
		promptText.text = "You'll have to destroy those arms first.";
		tmpPrompt.text = "You'll have to destroy those arms first.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallBatteryLow() {
		// Called from PlayerHealth.Update()
		promptText.text = "Your suit is low on energy, use your vacuum to recharge it.";
		tmpPrompt.text = "Your suit is low on energy, use your vacuum to recharge it.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallArmDestroyedOne() {
		// Called from giantBehaviour.CheckHealth()
		promptText.text = "Good job! Now you just have to destroy the other one.";
		tmpPrompt.text = "Good job! Now you just have to destroy the other one.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);

	}

	public void CallArmDestroyedTwo() {
		// Called from giantBehaviour.CheckHealth()
		promptText.text = "Nice! Now destroy the legs and it won't be able to go anywhere.";
		tmpPrompt.text = "Nice! Now destroy the legs and it won't be able to go anywhere.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	private bool boolEnterFinalPhaseShown = false;
	public void CallEnterFinalPhase() {
		// Called from giantBehaviour.CheckHealth()
		if (!boolEnterFinalPhaseShown) {
			boolEnterFinalPhaseShown = true;
			promptText.text = "It's almost dead, now attack its core and destroy it for good!";
			tmpPrompt.text = "It's almost dead, now attack its core and destroy it for good!";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);
		}
	}

	private IEnumerator Call30SecondsIntoFinal() {
		yield return new WaitForSeconds(30);

		promptText.text = "It's sending out a distress signal! It's going to give its all to destroy the wall, finish it quickly, you only have a few moments!";
		tmpPrompt.text = "It's sending out a distress signal! It's going to give its all to destroy the wall, finish it quickly, you only have a few moments!";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallWin() {
		// Called from GameManager.COWin()
		promptText.text = "You did it! Excellent work today, you're clear to return to base.";
		tmpPrompt.text = "You did it! Excellent work today, you're clear to return to base.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallLosePlayer() {
		// Called from PlayerHealth.Die();

		promptText.text = "Critical damage taken, return to base before you can’t.";
		tmpPrompt.text = "Critical damage taken, return to base before you can’t.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallLoseWall() {
		promptText.text = "The wall’s down. Mission failed, fall back to base for now, we need to prepare for the next attack.";
		tmpPrompt.text = "The wall’s down. Mission failed, fall back to base for now, we need to prepare for the next attack.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallLowHealth() {
		// Called from PlayerHealth.TakeDamage(float)
		// Called from PlayerHealth.TakeDamage(float, GameObject)

		promptText.text = "Major damage taken, back off and heal before re-engaging.";
		tmpPrompt.text = "Major damage taken, back off and heal before re-engaging.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}
}
