using System;
using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// PlayerPrefs + JsonUtility 기반 세이브 데이터 로드/저장 골격.
    /// 프로젝트에서는 UserDataManager : UserDataManagerBase&lt;TUserData, UserDataManager&gt; 형태로 상속해서
    /// Data 프로퍼티로 접근하고, 코인/인벤토리 같은 도메인 로직은 서브클래스에서 구현한다.
    /// TSelf는 Instance가 서브클래스 타입으로 반환되도록 하기 위한 CRTP 파라미터.
    /// </summary>
    public abstract class UserDataManagerBase<TUserData, TSelf> : Singleton<TSelf>, IManager
        where TUserData : class, new()
        where TSelf : UserDataManagerBase<TUserData, TSelf>
    {
        public int Priority => 0;

        [SerializeField] private string _saveKey = "UserData";

        private TUserData _data;
        private bool _loaded;

        protected TUserData Data
        {
            get { EnsureLoaded(); return _data; }
        }

        public void Initialize() => EnsureLoaded();

        private void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;

            var json = PlayerPrefs.GetString(_saveKey, string.Empty);
            if (!string.IsNullOrEmpty(json))
            {
                try { _data = JsonUtility.FromJson<TUserData>(json); }
                catch (Exception e)
                {
                    Debug.LogWarning($"[{typeof(TSelf).Name}] 세이브 파싱 실패 — 진행도를 초기화합니다. ({e.Message})\n원본: {json}");
                    _data = null;
                }
            }

            _data ??= CreateDefaultData();

            OnDataLoaded(_data);
        }

        protected void Save()
        {
            PlayerPrefs.SetString(_saveKey, JsonUtility.ToJson(_data));
            PlayerPrefs.Save();
        }

        /// <summary>세이브가 없거나 파싱에 실패했을 때 사용할 기본 데이터.</summary>
        protected virtual TUserData CreateDefaultData() => new();

        /// <summary>
        /// 로드(또는 기본값 생성) 직후 호출된다.
        /// 서브클래스에서 컬렉션 null 정리, 손상/조작된 값 검증(새니타이즈) 등에 사용한다.
        /// </summary>
        protected virtual void OnDataLoaded(TUserData data) { }
    }
}
