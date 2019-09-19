using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigatorPrompts : MonoBehaviour {
	public Text promptText;

	private bool noLegWithArmCalled = false;

	public void CallIntroLine() {
		// Called from PlayerController.Start()
		promptText.text = "You've arrived! Quickly, enter the dam, your target is there.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallFollowUpIntroLine() {
		// Called from PlayerController.DelayedIntroPrompt()
		promptText.text = "The dam's under attack again. You have to make sure the wall stays intact.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallSeeingBoss() {
		// Called from MusicZone.OnTriggerEnter()
		promptText.text = "You need to stop it from reaching the wall! Destroy it's legs to stop it moving.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallTakingPhysicalDamage() {
		// Called from PlayerDamageableSection.OnTriggerEnter()

		promptText.text = "Be careful, your suit won't survive everything. Make sure to avoid its attacks.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallTakingMissileDamage() {
		// - Called from PlayerHealth.TakeDamage(float, GameObject)
		//promptText.text = "Looks like it has some pretty heavy weapons of it's own, look out for them.";


		// Called from PlayerDamageableSection.OnTriggerEnter()
		promptText.text = "Be careful, those missiles will destroy your armour, use you shield to reduce the damage.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallYellowZone() {
		// Called from PlayerContoller.OnTrigggerEnter()
		promptText.text = "Warning: Approaching 'No Fly' zone, reduce height.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallRedZone() {
		// Called from PlayerController.StopFlying()
		promptText.text = "Warning: Cutting engines to avoid detection. Prepare to fall.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallNoLegWithArms() {
		// Called from giantBehaviour.CheckHealth()
		if (!noLegWithArmCalled) {
			noLegWithArmCalled = true;

			promptText.text = "Good job, it won't be... Hold on, is it repairing itself?";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);

			StartCoroutine(CallFollowUpNoLegWithArm());
		}
	}

	private IEnumerator CallFollowUpNoLegWithArm() {
		yield return new WaitForSeconds(4);
		promptText.text = "You'll have to destroy those arms first.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallBatteryLow() {
		// Called from PlayerHealth.Update()
		promptText.text = "Your suit is low on energy, use your vacuuum to recharge it.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallArmDestroyedOne() {
		// Called from giantBehaviour.CheckHealth()
		promptText.text = "Good job! Now you just have to destroy the other one.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);

	}

	public void CallArmDestroyedTwo() {
		// Called from giantBehaviour.CheckHealth()
		promptText.text = "Nice! Now destroy the legs and it won't be able to go anywhere.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	private bool boolEnterFinalPhaseShown = false;
	public void CallEnterFinalPhase() {
		// Called from giantBehaviour.CheckHealth()
		if (!boolEnterFinalPhaseShown) {
			boolEnterFinalPhaseShown = true;
			promptText.text = "It's almost dead, now attack its core and destroy it for good!";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);
		}
	}

	private IEnumerator Call30SecondsIntoFinal() {
		yield return new WaitForSeconds(30);

		promptText.text = "It's sending out a distress signal! It's going to give its all to destroy the wall, finish it quickly, you only have a few moments!";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallWin() {
		// Called from GameManager.COWin()
		promptText.text = "You did it! Excellent work today, you're clear to return to base.";
		promptText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	public void CallLosePlayer() {

	}

	public void CallLoseWall() {

	}
}
