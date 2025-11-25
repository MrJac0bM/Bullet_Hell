using UnityEngine;

public class DoomMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doomMusic;
    [SerializeField] private float volume = 0.8f;
    [SerializeField] private bool loopMusic = true;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No hay AudioSource en este GameObject");
            return;
        }

        if (doomMusic == null)
        {
            Debug.LogError("No hay música asignada");
            return;
        }

        // Fuerza que el audio se escuche
        AudioListener.volume = 1f;
        
        audioSource.clip = doomMusic;
        audioSource.volume = volume;
        audioSource.loop = loopMusic;
        audioSource.Play();

        Debug.Log($"🎵 MÚSICA ÉPICA DE DOOM INICIADA 🎵");
        Debug.Log($"Volume: {audioSource.volume}, Playing: {audioSource.isPlaying}, Listener Volume: {AudioListener.volume}");
    }
}