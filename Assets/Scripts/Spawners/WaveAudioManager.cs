using UnityEngine;

public class WaveAudioManager {
    private AudioSource normalBGMusic;
    private AudioSource bossBGMusic;
    private AudioSource waveEndMusic;
    private AudioSource waveCountdownMusic;

    public WaveAudioManager(AudioSource normalBGMusic, AudioSource bossBGMusic, AudioSource waveEndMusic, AudioSource waveCountdownMusic) {
        this.normalBGMusic = normalBGMusic;
        this.bossBGMusic = bossBGMusic;
        this.waveEndMusic = waveEndMusic;
        this.waveCountdownMusic = waveCountdownMusic;
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

    public void playWaveCountdownMusic() {
        // if waveCountdown music is already playing, do nothing
        if (this.waveCountdownMusic.isPlaying) return;
        
        this.waveCountdownMusic.Play();
    }
}