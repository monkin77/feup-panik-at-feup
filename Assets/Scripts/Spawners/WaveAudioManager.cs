using UnityEngine;

public class WaveAudioManager {
    private AudioSource normalBGMusic;
    private AudioSource bossBGMusic;
    private AudioSource waveEndMusic;
    private AudioSource waveCountdownMusic;
    private AudioSource bossWaveCountdownMusic;

    public WaveAudioManager(AudioSource normalBGMusic, AudioSource bossBGMusic, AudioSource waveEndMusic, 
        AudioSource waveCountdownMusic, AudioSource bossWaveCountdownMusic) {
        this.normalBGMusic = normalBGMusic;
        this.bossBGMusic = bossBGMusic;
        this.waveEndMusic = waveEndMusic;
        this.waveCountdownMusic = waveCountdownMusic;
        this.bossWaveCountdownMusic = bossWaveCountdownMusic;
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

    public void playBossWaveCountdownMusic() {
        // if bossWaveCountdown music is already playing, do nothing
        if (this.bossWaveCountdownMusic.isPlaying) return;
        
        this.bossWaveCountdownMusic.Play();
    }
}