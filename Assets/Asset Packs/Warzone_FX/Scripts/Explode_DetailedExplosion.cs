using UnityEngine;
using System.Collections;

public class Explode_DetailedExplosion : MonoBehaviour {



	public ParticleSystem ExplodeVideoParticles;
	public ParticleSystem SmokeParticles;
	public ParticleSystem SparkParticles;
	public Light ExplodeLight;
	public AudioSource ExplodeAudio;
	public GameObject ScorchMark;
	public ParticleSystem Fireball;

	private float fadeStart= 4;
	private float fadeEnd= 0;
	private float fadeTime= 1;
	private float t= 0.0f;

	void  Update (){

		if (Input.GetButtonDown("Fire1")) //check to see if the left mouse was pushed.
		{ 
			// Stop any previous explosion
			ExplodeVideoParticles.Clear();
			SmokeParticles.Clear();
			SparkParticles.Clear();
			Fireball.Clear();
			Explosion();      
		}

	}


	IEnumerator FadeLight (){


		while (t < fadeTime) 

		{
			t += Time.deltaTime;

			ExplodeLight.intensity = Mathf.Lerp(fadeStart, fadeEnd, t / fadeTime);
			yield return 0;

		}              

		t = 0;

	}


	void  Explosion (){

		ExplodeVideoParticles.Play();
		SmokeParticles.Play();
		SparkParticles.Play();
		Fireball.Play();
		ExplodeAudio.Play();
		ScorchMark.SetActive(true);
		StartCoroutine("FadeLight");

	}


}