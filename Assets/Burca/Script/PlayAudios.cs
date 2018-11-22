using UnityEngine;
using System.Collections;

public class PlayAudios : MonoBehaviour {

    private static PlayAudios instance;

    public AudioSource music, sounds;
    public AudioClip right, wrong;

    void Awake ()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);
    }
	
	public void PlayRight ()
    {
        sounds.clip = right;
        sounds.Play();
    }

    public void PlayWrong()
    {
        sounds.clip = wrong;
        sounds.Play();
    }
}
