using UnityEngine;

namespace JoonyleGameDevKit
{
    public class EffectParticle : EffectBase
    {
        private ParticleSystem[] _particles;
        private float[] _baseScaleXs;

        private void Awake() => EnsureInit();

        private void EnsureInit()
        {
            if (_particles != null) return;
            _particles = GetComponentsInChildren<ParticleSystem>();
            _baseScaleXs = new float[_particles.Length];
            for (int i = 0; i < _particles.Length; i++)
                _baseScaleXs[i] = _particles[i].transform.localScale.x;
        }

        protected override void OnPlay()
        {
            EnsureInit();
            var xSign = transform.localScale.x >= 0f ? 1f : -1f;
            for (int i = 0; i < _particles.Length; i++)
            {
                var scale = _particles[i].transform.localScale;
                _particles[i].transform.localScale = new Vector3(_baseScaleXs[i] * xSign, scale.y, scale.z);
                _particles[i].Clear();
                if (_simulateTime > 0f)
                    _particles[i].Simulate(_simulateTime, false, true);
                _particles[i].Play();
            }
            _simulateTime = 0f;
        }

        protected override void OnStop()
        {
            foreach (var particle in _particles)
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // ParticleSystem Main 모듈의 Stop Action을 Callback으로 설정해야 호출됨
        private void OnParticleSystemStopped() => OnComplete();
    }
}
