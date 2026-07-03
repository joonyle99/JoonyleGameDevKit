using System;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace JoonyleGameDevKit
{
    [Serializable]
    public struct BgmEntry<TBgm> where TBgm : Enum
    {
        public TBgm type;
        public AudioClip clip;
    }

    [Serializable]
    public struct SfxEntry<TSfx> where TSfx : Enum
    {
        public TSfx type;
        public AudioClip clip;
    }

    /// <summary>
    /// BGM/SFX 타입을 프로젝트별 enum(TBgm, TSfx)으로 주입받는 사운드 매니저 베이스.
    /// 프로젝트에서는 SoundManager : SoundManagerBase&lt;BgmType, SfxType, SoundManager&gt; 형태로 상속해서 사용한다.
    /// TSelf는 Instance가 서브클래스 타입(예: OnStateChanged 등 프로젝트 전용 멤버)으로 반환되도록 하기 위한 CRTP 파라미터.
    /// </summary>
    public abstract class SoundManagerBase<TBgm, TSfx, TSelf> : Singleton<TSelf>, IManager
        where TBgm : Enum
        where TSfx : Enum
        where TSelf : SoundManagerBase<TBgm, TSfx, TSelf>
    {
        public int Priority => 10;

        [SerializeField] private BgmEntry<TBgm>[] _bgmEntries;
        [SerializeField] private SfxEntry<TSfx>[] _sfxEntries;

        [SerializeField] protected AudioSource bgmSource;
        [SerializeField] protected AudioSource sfxSource;

        [Space]

        [SerializeField] private float _bgmFadeDuration = 0.35f;

        /// <summary>
        /// 프로젝트 서브클래스에서 게임 상태 전환 등 커스텀 BGM 로직을 구현할 때 조회용으로 사용한다
        /// (fade 없이 즉시 전환하는 등, PlayBgm이 제공하지 않는 세밀한 제어가 필요한 경우)
        /// </summary>
        protected Dictionary<TBgm, BgmEntry<TBgm>> bgmMap;
        protected Dictionary<TSfx, SfxEntry<TSfx>> sfxMap;

        protected float bgmVolume;
        private Tween _bgmFadeTween;

        private const string PREF_BGM = "Setting_BGM";
        private const string PREF_SFX = "Setting_SFX";

        public bool IsBgmEnabled { get; private set; } = true;
        public bool IsSfxEnabled { get; private set; } = true;

        public event Action<bool> OnBgmEnabledChanged;
        public event Action<bool> OnSfxEnabledChanged;

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _bgmFadeTween?.Kill();
        }

        // ============== ... ==============

        public void Initialize()
        {
            bgmMap = new Dictionary<TBgm, BgmEntry<TBgm>>();
            foreach (var entry in _bgmEntries)
                bgmMap.TryAdd(entry.type, entry);

            sfxMap = new Dictionary<TSfx, SfxEntry<TSfx>>();
            foreach (var entry in _sfxEntries)
                sfxMap.TryAdd(entry.type, entry);

            bgmVolume = bgmSource.volume;

            // 등록된 모든 BGM 클립을 미리 로드
            foreach (var entry in _bgmEntries)
                entry.clip?.LoadAudioData();

            SetBgmEnabled(PlayerPrefs.GetInt(PREF_BGM, 1) == 1);
            SetSfxEnabled(PlayerPrefs.GetInt(PREF_SFX, 1) == 1);
        }

        // ============== ... ==============

        public void SetBgmEnabled(bool enabled)
        {
            IsBgmEnabled = enabled;
            bgmSource.mute = !enabled;
            PlayerPrefs.SetInt(PREF_BGM, enabled ? 1 : 0);
            OnBgmEnabledChanged?.Invoke(enabled);
        }

        public void SetSfxEnabled(bool enabled)
        {
            IsSfxEnabled = enabled;
            sfxSource.mute = !enabled;
            PlayerPrefs.SetInt(PREF_SFX, enabled ? 1 : 0);
            OnSfxEnabledChanged?.Invoke(enabled);
        }

        // ============== ... ==============

        public void PlayBgm(TBgm type, float volume = -1f)
        {
            if (!bgmMap.TryGetValue(type, out var entry) || entry.clip == null) return;
            if (bgmSource.clip == entry.clip) return;

            if (volume >= 0f) bgmVolume = volume;

            FadeBgmTo(0f);
            _bgmFadeTween.OnComplete(() =>
            {
                bgmSource.clip = entry.clip;
                bgmSource.Play();
                FadeBgmTo(bgmVolume);
            });
        }

        public void StopBgm()
        {
            FadeBgmTo(0f);
            _bgmFadeTween.OnComplete(() =>
            {
                bgmSource.Stop();
                bgmSource.clip = null;
            });
        }

        public void PlaySfx(TSfx type, float volume = 0.5f)
        {
            if (sfxMap.TryGetValue(type, out var entry) && entry.clip != null)
            {
                sfxSource.PlayOneShot(entry.clip, volume);
            }
        }

        // ============== ... ==============

        private void FadeBgmTo(float targetVolume)
        {
            _bgmFadeTween?.Kill();
            _bgmFadeTween = bgmSource.DOFade(targetVolume, _bgmFadeDuration).SetUpdate(true);
        }

        // ============== ... ==============

        public void SetGamePaused(bool paused)
        {
            var sources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var source in sources)
            {
                if (source == sfxSource) continue; // PlayOneShot 전용 소스 — pause 불필요, UI 클릭음 오작동 방지
                if (paused) source.Pause();
                else source.UnPause();
            }
        }
    }
}
