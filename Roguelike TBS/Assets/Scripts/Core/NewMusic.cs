using UnityEngine;

public class NewMusic : MonoBehaviour {
    [SerializeField] AudioClip newMusic = null;
    
    private void Awake () {
        GameObject musicPlayer = GameObject.Find("Music Player");
        if (musicPlayer.GetComponent<AudioSource>().clip != newMusic) {
            musicPlayer.GetComponent<AudioSource>().clip = newMusic;
            musicPlayer.GetComponent<AudioSource>().Play();
        }
    }
}
