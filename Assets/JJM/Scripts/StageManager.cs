using System.Collections.Generic;
using Publics.Scripts;
using UnityEngine;

namespace JJM.Scripts
{
    public class StageManager : MonoSingleton<StageManager>
    {
        [field: SerializeField]
        public int CurrentStageNumber { get; private set; }

        [field: SerializeField]
        public int CurrentMapIndex { get; private set; } = -1;

        [SerializeField] private Transform maps;

        private readonly List<int> _remainingMapIndices = new();

        private int _cachedMapCount;
        private int _lastMapIndex = -1;

        [ContextMenu("Start Test")]
        public void StageStart()
        {
            if (maps == null)
            {
                Debug.LogError($"{name}: Maps가 연결되지 않았습니다.");
                return;
            }

            int mapCount = maps.childCount;

            if (mapCount == 0)
            {
                Debug.LogError($"{name}: Maps 아래에 맵이 없습니다.");
                return;
            }

            DisableAllMaps();

            if (_remainingMapIndices.Count == 0 ||
                _cachedMapCount != mapCount)
            {
                RefillMapIndices(mapCount);
            }

            int randomListIndex =
                Random.Range(0, _remainingMapIndices.Count);

            int selectedMapIndex =
                _remainingMapIndices[randomListIndex];
            
            if (_remainingMapIndices.Count > 1 &&
                selectedMapIndex == _lastMapIndex)
            {
                randomListIndex =
                    (randomListIndex + 1) %
                    _remainingMapIndices.Count;

                selectedMapIndex =
                    _remainingMapIndices[randomListIndex];
            }

            _remainingMapIndices.RemoveAt(randomListIndex);

            maps.GetChild(selectedMapIndex)
                .gameObject
                .SetActive(true);

            CurrentMapIndex = selectedMapIndex;
            CurrentStageNumber++;

            _lastMapIndex = selectedMapIndex;
        }

        public void StageEnd()
        {
            if (maps == null)
            {
                return;
            }

            DisableAllMaps();
            CurrentMapIndex = -1;
        }

        private void DisableAllMaps()
        {
            for (int i = 0; i < maps.childCount; i++)
            {
                maps.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void RefillMapIndices(int mapCount)
        {
            _remainingMapIndices.Clear();

            for (int i = 0; i < mapCount; i++)
            {
                _remainingMapIndices.Add(i);
            }

            _cachedMapCount = mapCount;
        }

        public void ResetStageProgress()
        {
            DisableAllMaps();

            CurrentStageNumber = 0;
            CurrentMapIndex = -1;

            _lastMapIndex = -1;
            _cachedMapCount = 0;

            _remainingMapIndices.Clear();
        }
    }
}