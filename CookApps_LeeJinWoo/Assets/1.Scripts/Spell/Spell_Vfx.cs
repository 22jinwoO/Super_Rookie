using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Vfx : MonoBehaviour
{
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	IEnumerator CheckIfAlive()
	{

		yield return new WaitForSeconds(1.5f);
		gameObject.SetActive(false);
	}

}
