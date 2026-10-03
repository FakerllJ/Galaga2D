using UnityEngine;

// Toca um efeito sonoro uma vez (nao precisa de AudioSource no objeto)
public static class Sfx
{
    public static void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}
