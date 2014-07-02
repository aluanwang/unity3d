using UnityEngine;
using System.Collections;


public class FFT : MonoBehaviour {

	public AudioSource audioSauce;
	public string CurrentAudioInput = "none";
	int deviceNum = 0;
	public int WINDOW_SIZE = 256;
	public float[] spectrum; 
	public float loudness = 0;
	public const float freq = 24000f;
	public float rr = 0.0f;


	IEnumerator Start() {
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
		if (Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone)) {
		} else {
		}

		spectrum = new float[WINDOW_SIZE];
		string[] inputDevices = new string[Microphone.devices.Length];
		deviceNum = 0;
		
		for (int i = 0; i < Microphone.devices.Length; i++) {
			inputDevices [i] = Microphone.devices [i].ToString ();
			Debug.Log("Device: " + inputDevices [i]);
		}
		CurrentAudioInput = Microphone.devices[deviceNum].ToString();
		StartMic ();
		audioSauce.mute = true; 


	}
	
	


	public void StartMic(){
		audioSauce.clip = Microphone.Start(CurrentAudioInput, true, 5, (int) freq); 
	}

	void Update() {
		rr = (rr + 1.0f) % 360.0f;
		loudness = GetAveragedVolume ()*100.0f+1.0f;

		if (Input.GetKeyDown(KeyCode.Equals))
		{
			Microphone.End (CurrentAudioInput);
			deviceNum+= 1;
			if (deviceNum > Microphone.devices.Length - 1)
				deviceNum = 0;
			CurrentAudioInput = Microphone.devices[deviceNum].ToString();
			
			StartMic ();
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			audioSauce.Play ();
		}
		
		float delay = 0.030f;
		int microphoneSamples = Microphone.GetPosition (CurrentAudioInput);

		if (microphoneSamples / freq > delay) {
			if (!audioSauce.isPlaying) {
				Debug.Log ("Starting thing");
				audioSauce.timeSamples = (int) (microphoneSamples - (delay * freq));
				audioSauce.Play ();
			}
		}
		audioSauce.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);
		transform.localScale = new Vector3(loudness,loudness,loudness);
		transform.Rotate (new Vector3(15,30,45)*Time.deltaTime);
	}


	float GetAveragedVolume()
	{ 
		float[] data = new float[WINDOW_SIZE];
		float a = 0;
		audio.GetOutputData(data,0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/WINDOW_SIZE;
	}
}
