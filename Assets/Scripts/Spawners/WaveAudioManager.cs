using UnityEngine;

public class WaveAudioManager {
    private AudioSource normalBGMusic;
    private AudioSource bossBGMusic;
    private AudioSource waveEndMusic;

    public WaveAudioManager(AudioSource normalBGMusic, AudioSource bossBGMusic, AudioSource waveEndMusic) {
        this.normalBGMusic = normalBGMusic;
        this.bossBGMusic = bossBGMusic;
        this.waveEndMusic = waveEndMusic;
    }

    public void playNormalBGMusic() {
        this.bossBGMusic.Stop();
    
        this.normalBGMusic.Play();
    }

    public void playBossBGMusic() {
        this.normalBGMusic.Stop();

        this.bossBGMusic.Play();
    }

    public void playWaveEndMusic() {
        this.normalBGMusic.Stop();
        this.bossBGMusic.Stop();

        this.waveEndMusic.Play();
    }
}