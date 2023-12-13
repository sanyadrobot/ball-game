using System;
using System.Collections;
using System.Reflection.Emit;
using System.Threading;
using DOTS.ComponentsAndTags;
using DOTS.Systems;
using Lean.Touch;
using Mono.Services;
using Mono.Services.Game;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Utils;
using Zenject;

namespace Mono
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private float squeezeSpeed = 10f;
        [SerializeField] private GameObject spawner;
        [SerializeField] private LeanFingerDown leanFingerDown;
        [SerializeField] private LeanFingerUp leanFingerUp;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private BoxCollider boxCollider;
        
        private IGameService _gameService;
        private IProjectileSpawnerService _projectileSpawnerSystem;

        [Inject]
        public void Constructor(IGameService gameService, IProjectileSpawnerService projectileSpawnerService)
        {
            _gameService = gameService;
            _projectileSpawnerSystem = projectileSpawnerService;
        }

        private float _playerVolume = 200f;
        private float _minSqueezeVolume = 4.2f;
        private float _spawnerProjectileVolume = 0;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _gameIsLost = false;
        private bool _gameIsWon = false;

        private float GetSphereScale(float volume)
        {
            return Mathf.Pow(volume * 3f / (4 * Mathf.PI), 1f / 3f);
        }

        private void SetPlayerTransform()
        {
            if (_playerVolume > 0)
            {
                var size = GetSphereScale(_playerVolume);
                var playerTransform = transform;
                playerTransform.localScale = new Vector3(size, size, size);
                var position = playerTransform.position;
                position.y = size / 2;
                playerTransform.position = position;
                lineRenderer.startWidth = size;
                lineRenderer.endWidth = size > 1.6f ? 1.6f : size;
                lineRenderer.SetPosition(0, position);
                lineRenderer.SetPosition(1, new Vector3(_gameService.ExitPosition.x, 0.5f, _gameService.ExitPosition.z));
            }
            else
            {
                if (_gameIsLost) return;
                Dispose();
                _gameService.Fire(GameTrigger.Lost);
                _gameIsLost = true;
            }
        }
        
        private void SetSpawnerTransform()
        {
            if (_spawnerProjectileVolume > 0)
            {
                var size = GetSphereScale(_spawnerProjectileVolume);
                var spawnerTransform = spawner.transform;
                var localPlayerScale = transform.localScale;
                spawnerTransform.localScale = new Vector3(size/localPlayerScale.x, size/localPlayerScale.y, size/localPlayerScale.z);
                var position = spawnerTransform.localPosition;
                position.z = (size / 2f + transform.position.y)/localPlayerScale.x;
                spawnerTransform.localPosition = position;
            }
            else
            {
                spawner.transform.localScale = Vector3.zero;
            }
        }

        private IEnumerator ProjectileSqueezeCoroutine(CancellationToken cancellationToken)
        {
            _playerVolume -= _minSqueezeVolume;
            _spawnerProjectileVolume = _minSqueezeVolume;
            SetPlayerTransform();
            SetSpawnerTransform();
            while (!cancellationToken.IsCancellationRequested)
            {
                var diff = Time.deltaTime * squeezeSpeed;
                _playerVolume -= diff;
                _spawnerProjectileVolume += diff;
                SetPlayerTransform();
                SetSpawnerTransform();
                yield return null;
            }
        }

        public void Awake()
        {
            _playerVolume = Mathf.Lerp(75, 300, (_gameService.ObstaclesToSpawnCount - 100) / 1000f);
            
            StartCoroutine(SetSpawnerPositionCoroutine());

            if (leanFingerDown == null)
                leanFingerDown = GetComponent<LeanFingerDown>();
            
            if (leanFingerUp == null)
                leanFingerUp = GetComponent<LeanFingerUp>();
            
            leanFingerDown.OnFinger.AddListener(GrowProjectile);
            leanFingerUp.OnFinger.AddListener(PushProjectile);
            DestroySystem.OnEntitiesDestroy += CheckWinState;
            SetPlayerTransform();
            SetSpawnerTransform();
            SetLookRotation(_gameService.ExitPosition);
        }

        private void PushProjectile(LeanFinger arg0)
        {
            if(_spawnerProjectileVolume == 0) return;
            _cancellationTokenSource.Cancel();
            _projectileSpawnerSystem.SpawnProjectile(GetSphereScale(_spawnerProjectileVolume));
            _spawnerProjectileVolume = 0;
            SetPlayerTransform();
            SetSpawnerTransform();
        }

        private void GrowProjectile(LeanFinger arg0)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartCoroutine(ProjectileSqueezeCoroutine(_cancellationTokenSource.Token));
        }

        public void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            StopAllCoroutines();
            spawner.gameObject.transform.localScale = Vector3.zero;
            leanFingerDown.OnFinger.RemoveAllListeners();
            leanFingerUp.OnFinger.RemoveAllListeners();
            _cancellationTokenSource?.Cancel();
            DestroySystem.OnEntitiesDestroy -= CheckWinState;
        }

        private IEnumerator SetSpawnerPositionCoroutine()
        {
            while (true)
            {
                _projectileSpawnerSystem.SetSpawnerPosition(spawner.transform);
                yield return null;
            }
        }

        private void SetLookRotation(Vector3 lookPosition)
        {
            var targetPosition = new Vector3(lookPosition.x, transform.position.y, lookPosition.z);
            transform.LookAt(targetPosition);
        }

        public void CheckWinState()
        {
            StartCoroutine(CheckWinStateCoroutine());
        }

        private bool CheckPointInsideBox (Vector3 point, BoxCollider box )
        {
            point = box.transform.InverseTransformPoint( point ) - box.center;

            var size = box.size;
            var halfX = size.x * 0.5f + 0.1f;
            var halfY = size.y * 0.5f + 0.1f;;
            var halfZ = size.z * 0.5f + 0.1f;;

            return point.x < halfX && point.x > -halfX && 
                   point.y < halfY && point.y > -halfY && 
                   point.z < halfZ && point.z > -halfZ;
        }
        
        IEnumerator CheckWinStateCoroutine()
        {
           
            boxCollider.gameObject.transform.position =
                (new Vector3(transform.position.x, 0, transform.position.z) + _gameService.ExitPosition) / 2f;

            boxCollider.gameObject.transform.localScale = new Vector3(1, 10,
                Vector3.Distance(transform.position, _gameService.ExitPosition) / transform.localScale.x);

            yield return null; // update bounds
            
            var entities = Helper.GetSystemsManaged<ObstacleTag>();
            foreach (var entity in entities)
            {
                var componentData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(
                    entity);
                
                if (CheckPointInsideBox(componentData.Position, boxCollider))
                {
                    yield break;
                }
            }

            Dispose();
            if(!_gameIsWon)
                StartCoroutine(WinCoroutine());
        }

        IEnumerator WinCoroutine()
        {
            var rb = gameObject.AddComponent<Rigidbody>();
            lineRenderer.enabled = false;
            boxCollider.enabled = false;
            _gameIsWon = true;

            while (true)
            {
                SetLookRotation(_gameService.ExitPosition);
                rb.AddForce(transform.up * 200f);
                rb.AddForce(transform.forward * 200f);
                yield return new WaitForSeconds(1);
            }
        }
    }
}