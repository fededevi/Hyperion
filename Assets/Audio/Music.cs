using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour 
{
	public enum Status
	{
		Calm,
		Combat,
	}
	
	public ListsKeeper listsKeeper;
	public Status status;
	public AudioSource audioSource;
	public AudioClip[] combatMusics;
	public AudioClip[] calmMusics;
	//public AudioClip endLoseMusic;
	//public AudioClip endWinMusic;
	
	private int combatMusicIndex;
	private int calmMusicIndex;
		
	void Awake ()
	{
		GameObject list = GameObject.Find("ListsKeeper");
		listsKeeper = list.GetComponent<ListsKeeper>();
	}
	
	void Start () 
	{
		combatMusicIndex =  (int) (Random.value * combatMusics.Length);
		calmMusicIndex =  (int) (Random.value * calmMusics.Length);
	}
	
	void Update () 
	{
		if ( listsKeeper.inCombat )
		{
			status = Music.Status.Combat;
		} else
		{
			status = Music.Status.Calm;	
		}
		
		if ( !audioSource.isPlaying )
		{
			if ( status == Music.Status.Calm )
			{
				audioSource.clip = calmMusics[calmMusicIndex++];
				if (calmMusicIndex > calmMusics.Length) calmMusicIndex = 0;
			}
			if ( status == Music.Status.Combat )
			{
				audioSource.clip = combatMusics[combatMusicIndex++];
				if (combatMusicIndex > combatMusics.Length) combatMusicIndex = 0;
			}
			audioSource.Play();
		}
	}
}
