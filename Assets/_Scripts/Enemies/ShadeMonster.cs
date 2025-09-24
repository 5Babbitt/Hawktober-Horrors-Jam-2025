using System.Collections;
using _Scripts.SOAP.Variables;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Enemies
{
    public class ShadeMonster : Singleton<ShadeMonster>
    {
        private NavMeshAgent agent;
        
        [SerializeField] private GameObject visuals;
        [SerializeField] private EnemyStates state;
        
        [Space(20)]
        [SerializeField] private float moveSpeed = 2;
        [SerializeField] private float delayBeforeHide = 0.33f;

        [Space(20)]
        public BoolVariable playerCanSeeMonster;
        public Vector3Variable playerPosition;
        public Vector3Variable enemyPosition;
        
        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            agent.speed = moveSpeed;
        }

        private void OnEnable()
        {
            playerCanSeeMonster.OnValueChanged += OnPlayerSeenMonster;
        }

        private void OnDisable()
        {
            playerCanSeeMonster.OnValueChanged -= OnPlayerSeenMonster;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (state == EnemyStates.Chasing)
            {
                enemyPosition.Value = transform.position;
                agent.SetDestination(playerPosition.Value);
            }
        }

        private void SetState(EnemyStates newState)
        {
            if (newState == EnemyStates.Chasing)
            {
                agent.SetDestination(playerPosition.Value);
            }

            if (newState == EnemyStates.Watching)
            {
                agent.isStopped = true;
                enemyPosition.Value = transform.position;
            }
            
            state = newState;
        }

        public void StartChase()
        {
            Appear();
            SetState(EnemyStates.Chasing);
        }

        public void EndChase()
        {
            Disappear();
            SetState(EnemyStates.Watching);
        }
        
        private void OnPlayerSeenMonster(bool value)
        {
            if (!value || state != EnemyStates.Watching) return;

            Disappear();
        }

        public void Appear()
        {
            Appear(transform.position);
        }
        
        public void Appear(Vector3 position)
        {
            transform.position = position;
            visuals.SetActive(true);
        }

        private void Disappear()
        {
            StartCoroutine(HideMonster());
        }

        private IEnumerator HideMonster()
        {
            yield return new WaitForSeconds(delayBeforeHide);
            visuals.SetActive(false);
            transform.position = new Vector3(0, -100, 0);
            playerCanSeeMonster.Value = false;
        }
    }

    public enum EnemyStates
    {
        Watching,
        Chasing
    }
}
