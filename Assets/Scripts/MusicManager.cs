using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("------------ AudioSource --------------")]
    [SerializeField] AudioSource music;

    [Header("------------ AudioClip -------------")]
    public AudioClip background;
    
    void Start()
    {
        music.clip = background;
        music.Play();
    }

   
}
