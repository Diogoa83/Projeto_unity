
using AdventureCore;
using AdventureSouls;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureExtras
{
    public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
    {
        public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

        [Header("Actions")]
        public CharacterActionBase Chase;
        public CharacterActionBase PunchLeft;
        public CharacterActionBase PunchRight;
        public CharacterActionBase Slam;
        public CharacterActionBase Rest;
        public CharacterActionBase Wag;
        public CharacterActionBase Shout;
        public CharacterActionBase Hurt;

        public CharacterActionBase Attack1;
        public CharacterActionBase Attack2;
        public CharacterActionBase Attack3;
        public CharacterActionBase Attack4;
        public CharacterActionBase Attack5;
        public CharacterActionBase Attack6;
        public CharacterActionBase Attack7;
        public CharacterActionBase Attack8;
        public CharacterActionBase Attack9;
        public CharacterActionBase Attack10;
        public CharacterActionBase Attack11;
        public CharacterActionBase Attack12;
        public CharacterActionBase Attack13;
        public CharacterActionBase Attack14;
        public CharacterActionBase Attack15;

        private bool _dead;
        private int _energy = 10;
        private IEnumerator<CharacterActionBase> _currentActions;
        private Transform _playerTransform;

        private bool isPlayerInMeleeRange = false;

        protected override void Awake()
        {
            base.Awake();
            Actor.Idling += think;
            ValidateActions();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
                Movement.SetTarget(_playerTransform);
            }
        }

        private void Update()
        {
            Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
            Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
        }

        public override bool CheckGuardBreak(IDamageSender damageSender)
        {
            return PunchLeft != null && (PunchLeft.IsHappening || (PunchRight != null && PunchRight.IsHappening));
        }

        public override void StartGuardBreak()
        {
            _energy = 0;
            _currentActions = null;
            if (Hurt != null) Hurt.StartAction(force: true);
        }

        protected override void die(Vector3 force)
        {
            _dead = true;

            SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(
                SoulsCommons.Instance.Experience,
                AttributePool.GetValue(SoulsCommons.Instance.Experience)
            );

            GameObject disabler = new GameObject("disabler");
            disabler.transform.SetParent(transform.parent, false);
            disabler.SetActive(false);

            var ragdoll = Instantiate(RagdollPrefab, disabler.transform);
            AdventureHelper.CopyTransforms(ragdoll, Model);
            AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

            ragdoll.position = Model.position;
            ragdoll.rotation = Model.rotation;
            ragdoll.SetParent(transform.parent, false);

            Destroy(disabler);
            Destroy(gameObject);
        }

        private void think()
        {
            if (_dead) return;

            if (_currentActions == null || !_currentActions.MoveNext())
            {
                _currentActions = getActions();
                _currentActions?.MoveNext();
            }

            if (_currentActions?.Current != null)
                _currentActions.Current.StartAction(Actor);
        }

        private IEnumerator<CharacterActionBase> getActions()
        {
            if (_energy <= 0)
            {
                foreach (var action in rest())
                    yield return action;
                yield break;
            }

            if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
            {
                foreach (var action in wag())
                    yield return action;
                yield break;
            }

            _energy -= 7;

            List<CharacterActionBase> meleeActions = new List<CharacterActionBase>
            {
                Attack1, Attack2, Attack3, Attack4, Attack5,
                Attack6, Attack7, PunchLeft, Slam, Shout, Hurt, Attack8, Attack9, Attack10,
                Attack11, Attack12, Attack13, Attack14, Attack15
            };

            List<CharacterActionBase> rangedActions = new List<CharacterActionBase>
            {
                Chase, Wag, Chase, PunchRight
            };

            List<CharacterActionBase> possibleActions = new List<CharacterActionBase>();

            if (isPlayerInMeleeRange)
            {
                Debug.Log("[SoulsHeroApe] Player is in melee range (trigger)");
                possibleActions.AddRange(meleeActions);
            }
            else
            {
                Debug.Log("[SoulsHeroApe] Player is in ranged range (trigger)");
                possibleActions.AddRange(rangedActions);
            }

            possibleActions.RemoveAll(a => a == null);

            if (possibleActions.Count == 0)
                yield break;

            for (int i = 0; i < possibleActions.Count; i++)
            {
                var temp = possibleActions[i];
                int randomIndex = Random.Range(i, possibleActions.Count);
                possibleActions[i] = possibleActions[randomIndex];
                possibleActions[randomIndex] = temp;
            }

            int actionsToExecute = Mathf.Min(Random.Range(1, 25), possibleActions.Count);

            for (int i = 0; i < actionsToExecute; i++)
            {
                yield return possibleActions[i];
            }
        }

        private IEnumerable<CharacterActionBase> rest()
        {
            if (Rest != null) yield return Rest;
            _energy = 10;
        }

        private IEnumerable<CharacterActionBase> wag()
        {
            if (Wag != null) yield return Wag;
            _energy += 3;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInMeleeRange = true;
                _currentActions = null; // Força repensar imediatamente
                think();                // Chama a lógica de decisão na hora
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInMeleeRange = false;
                _currentActions = null; // Força repensar imediatamente
                think();                // Chama a lógica de decisão na hora
            }
        }



        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         isPlayerInMeleeRange = true;
        //         //Debug.Log("[SoulsHeroApe] Player ENTERED melee range (trigger)");
        //     }
        // }

        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         isPlayerInMeleeRange = false;
        //         //Debug.Log("[SoulsHeroApe] Player EXITED melee range (trigger)");
        //     }
        // }

        private void ValidateActions()
        {
            if (Chase == null) Debug.LogWarning($"{name}: Chase action está nula!");
            if (PunchLeft == null) Debug.LogWarning($"{name}: PunchLeft action está nula!");
            if (PunchRight == null) Debug.LogWarning($"{name}: PunchRight action está nula!");
            if (Slam == null) Debug.LogWarning($"{name}: Slam action está nula!");
            if (Rest == null) Debug.LogWarning($"{name}: Rest action está nula!");
            if (Wag == null) Debug.LogWarning($"{name}: Wag action está nula!");
            if (Shout == null) Debug.LogWarning($"{name}: Shout action está nula!");
            if (Hurt == null) Debug.LogWarning($"{name}: Hurt action está nula!");

            for (int i = 1; i <= 15; i++)
            {
                var atk = GetType().GetField($"Attack{i}")?.GetValue(this);
                if (atk == null)
                    Debug.LogWarning($"{name}: Attack{i} action está nula!");
            }
        }
    }
}
