using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class                            SimpleAudioEvent : AudioEvent
{
	public AudioClip[]                  clips;
	public RangedFloat                  volume = new RangedFloat(1, 1);
	[MinMaxRange(0, 2)]
	public RangedFloat                  pitch = new RangedFloat(1, 1);

	public override void                Play(AudioSource source)
	{
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.Play();
	}

    public void                         PlayOneShot(AudioSource source)
    {
        if (clips.Length == 0) return;
        source.PlayOneShot(clips[Random.Range(0, clips.Length)], Random.Range(volume.minValue, volume.maxValue));
    }
}