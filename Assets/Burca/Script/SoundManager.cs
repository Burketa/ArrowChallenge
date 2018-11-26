using UnityEngine;

public class SoundManager : MonoBehaviour {

    private static SoundManager instance;

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
