// using AdventureCore;
// using AdventureSouls;
// using System.Collections.Generic;
// using UnityEngine;

// namespace AdventureExtras
// {
//     public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
//     {
//         public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

//         [Header("Actions")]
//         public CharacterActionBase Chase;
//         public CharacterActionBase PunchLeft;
//         public CharacterActionBase PunchRight;
//         public CharacterActionBase Slam;
//         public CharacterActionBase Rest;
//         public CharacterActionBase Wag;
//         public CharacterActionBase Shout;
//         public CharacterActionBase Hurt;

//         private bool _dead;
//         private int _energy = 10;
//         private IEnumerator<CharacterActionBase> _currentActions;

//         protected override void Awake()
//         {
//             base.Awake();

//             Actor.Idling += think;
//         }

//         private void Update()
//         {
//             Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
//             Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
//         }

//         public override bool CheckGuardBreak(IDamageSender damageSender)
//         {
//             return PunchLeft.IsHappening || PunchRight.IsHappening;
//         }
//         public override void StartGuardBreak()
//         {
//             _energy = 0;
//             _currentActions = null;
//             Hurt.StartAction(force: true);
//         }

//         protected override void die(Vector3 force)
//         {
//             _dead = true;

//             SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(SoulsCommons.Instance.Experience, AttributePool.GetValue(SoulsCommons.Instance.Experience));
            
//             //disables ragdoll until it is scaled
//             //otherwise joint configure to the bigger original
//             GameObject disabler = new GameObject("disabler");
//             disabler.transform.SetParent(transform.parent, false);
//             disabler.SetActive(false);

//             var ragdoll = Instantiate(RagdollPrefab, disabler.transform);

//             AdventureHelper.CopyTransforms(ragdoll, Model);
//             AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

//             ragdoll.position = Model.position;
//             ragdoll.rotation = Model.rotation;

//             ragdoll.SetParent(transform.parent, false);

//             Destroy(disabler);
//             Destroy(gameObject);
//         }

//         private void think()
//         {
//             if (_dead)
//                 return;

//             if (_currentActions == null || !_currentActions.MoveNext())
//             {
//                 _currentActions = getActions();
//                 _currentActions.MoveNext();
//             }

//             _currentActions.Current.StartAction(Actor);
//         }

//         private IEnumerator<CharacterActionBase> getActions()
//         {
//             if (_energy <= 0)
//                 return rest().GetEnumerator();
//             if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
//                 return wag().GetEnumerator();

//             switch (Random.Range(0, 6))
//             {
//                 case 0:
//                     return rage().GetEnumerator();
//                 case 1:
//                     return slam().GetEnumerator();
//                 case 2:
//                 case 3:
//                     return punchLeft().GetEnumerator();
//                 case 4:
//                 case 5:
//                     return punchRight().GetEnumerator();
//             }

//             return punchLeft().GetEnumerator();
//         }

//         private IEnumerable<CharacterActionBase> punchLeft()
//         {
//             _energy -= 3;
//             yield return Chase;
//             yield return PunchLeft;
//         }

//         private IEnumerable<CharacterActionBase> punchRight()
//         {
//             _energy -= 3;
//             yield return Chase;
//             yield return PunchRight;
//         }

//         private IEnumerable<CharacterActionBase> slam()
//         {
//             _energy -= 5;
//             yield return Chase;
//             yield return Slam;
//         }

//         private IEnumerable<CharacterActionBase> rage()
//         {
//             _energy -= 8;
//             yield return Shout;
//             yield return Chase;
//             yield return PunchLeft;
//             yield return PunchRight;
//             yield return Slam;
//         }

//         private IEnumerable<CharacterActionBase> rest()
//         {
//             yield return Rest;
//             _energy = 10;
//         }

//         private IEnumerable<CharacterActionBase> wag()
//         {
//             yield return Wag;
//             _energy += 3;
//         }
//     }
// }


// using AdventureCore;
// using AdventureSouls;
// using System.Collections.Generic;
// using UnityEngine;

// namespace AdventureExtras
// {
//     public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
//     {
//         public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

//         [Header("Actions")]
//         public CharacterActionBase Chase;
//         public CharacterActionBase PunchLeft;
//         public CharacterActionBase PunchRight;
//         public CharacterActionBase Slam;
//         public CharacterActionBase Rest;
//         public CharacterActionBase Wag;
//         public CharacterActionBase Shout;
//         public CharacterActionBase Hurt;

//         // Novos ataques extras
//         public CharacterActionBase Attack1;
//         public CharacterActionBase Attack2;
//         public CharacterActionBase Attack3;
//         public CharacterActionBase Attack4;
//         public CharacterActionBase Attack5;

//         private bool _dead;
//         private int _energy = 10;
//         private IEnumerator<CharacterActionBase> _currentActions;

//         protected override void Awake()
//         {
//             base.Awake();
//             Actor.Idling += think;

//             // Validação para evitar que ações nulas causem erro
//             ValidateActions();
//         }

//         private void Update()
//         {
//             Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
//             Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
//         }

//         public override bool CheckGuardBreak(IDamageSender damageSender)
//         {
//             return PunchLeft != null && (PunchLeft.IsHappening || (PunchRight != null && PunchRight.IsHappening));
//         }

//         public override void StartGuardBreak()
//         {
//             _energy = 0;
//             _currentActions = null;
//             if (Hurt != null) Hurt.StartAction(force: true);
//         }

//         protected override void die(Vector3 force)
//         {
//             _dead = true;

//             SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(
//                 SoulsCommons.Instance.Experience, 
//                 AttributePool.GetValue(SoulsCommons.Instance.Experience)
//             );

//             GameObject disabler = new GameObject("disabler");
//             disabler.transform.SetParent(transform.parent, false);
//             disabler.SetActive(false);

//             var ragdoll = Instantiate(RagdollPrefab, disabler.transform);
//             AdventureHelper.CopyTransforms(ragdoll, Model);
//             AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

//             ragdoll.position = Model.position;
//             ragdoll.rotation = Model.rotation;
//             ragdoll.SetParent(transform.parent, false);

//             Destroy(disabler);
//             Destroy(gameObject);
//         }

//         private void think()
//         {
//             if (_dead)
//                 return;

//             if (_currentActions == null || !_currentActions.MoveNext())
//             {
//                 _currentActions = getActions();
//                 _currentActions?.MoveNext();
//             }

//             // Proteção contra null
//             if (_currentActions?.Current != null)
//                 _currentActions.Current.StartAction(Actor);
//         }

//         private IEnumerator<CharacterActionBase> getActions()
//         {
//             if (_energy <= 0)
//                 return rest().GetEnumerator();
//             if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
//                 return wag().GetEnumerator();

//             switch (Random.Range(0, 8)) // Adicionado caso para ataque combo
//             {
//                 case 0:
//                     return rage().GetEnumerator();
//                 case 1:
//                     return slam().GetEnumerator();
//                 case 2:
//                 case 3:
//                     return punchLeft().GetEnumerator();
//                 case 4:
//                 case 5:
//                     return punchRight().GetEnumerator();
//                 case 6:
//                 case 7:
//                     return attackCombo().GetEnumerator(); // Novo ataque combo
//             }

//             return punchLeft().GetEnumerator();
//         }

//         private IEnumerable<CharacterActionBase> punchLeft()
//         {
//             _energy -= 3;
//             if (Chase != null) yield return Chase;
//             if (PunchLeft != null) yield return PunchLeft;
//         }

//         private IEnumerable<CharacterActionBase> punchRight()
//         {
//             _energy -= 3;
//             if (Chase != null) yield return Chase;
//             if (PunchRight != null) yield return PunchRight;
//         }

//         private IEnumerable<CharacterActionBase> slam()
//         {
//             _energy -= 5;
//             if (Chase != null) yield return Chase;
//             if (Slam != null) yield return Slam;
//         }

//         private IEnumerable<CharacterActionBase> rage()
//         {
//             _energy -= 8;
//             if (Shout != null) yield return Shout;
//             if (Chase != null) yield return Chase;
//             if (PunchLeft != null) yield return PunchLeft;
//             if (PunchRight != null) yield return PunchRight;
//             if (Slam != null) yield return Slam;
//         }

//         private IEnumerable<CharacterActionBase> rest()
//         {
//             if (Rest != null) yield return Rest;
//             _energy = 10;
//         }

//         private IEnumerable<CharacterActionBase> wag()
//         {
//             if (Wag != null) yield return Wag;
//             _energy += 3;
//         }

//         // Combo de ataques com checagem de null
//         private IEnumerable<CharacterActionBase> attackCombo()
//         {
//             _energy -= 7;
//             if (Chase != null) yield return Chase;
//             if (Attack1 != null) yield return Attack1;
//             if (Attack2 != null) yield return Attack2;
//             if (Attack3 != null) yield return Attack3;
//             if (Attack4 != null) yield return Attack4;
//             if (Attack5 != null) yield return Attack5;
//         }

//         // Validação no Awake para alertar se algo está nulo
//         private void ValidateActions()
//         {
//             if (Chase == null) Debug.LogWarning($"{name}: Chase action está nula!");
//             if (PunchLeft == null) Debug.LogWarning($"{name}: PunchLeft action está nula!");
//             if (PunchRight == null) Debug.LogWarning($"{name}: PunchRight action está nula!");
//             if (Slam == null) Debug.LogWarning($"{name}: Slam action está nula!");
//             if (Rest == null) Debug.LogWarning($"{name}: Rest action está nula!");
//             if (Wag == null) Debug.LogWarning($"{name}: Wag action está nula!");
//             if (Shout == null) Debug.LogWarning($"{name}: Shout action está nula!");
//             if (Hurt == null) Debug.LogWarning($"{name}: Hurt action está nula!");

//             if (Attack1 == null) Debug.LogWarning($"{name}: Attack1 action está nula!");
//             if (Attack2 == null) Debug.LogWarning($"{name}: Attack2 action está nula!");
//             if (Attack3 == null) Debug.LogWarning($"{name}: Attack3 action está nula!");
//             if (Attack4 == null) Debug.LogWarning($"{name}: Attack4 action está nula!");
//             if (Attack5 == null) Debug.LogWarning($"{name}: Attack5 action está nula!");
//         }
//     }
// }


// using AdventureCore;
// using AdventureSouls;
// using System.Collections.Generic;
// using UnityEngine;

// namespace AdventureExtras
// {
//     public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
//     {
//         public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

//         [Header("Actions")]
//         public CharacterActionBase Chase;
//         public CharacterActionBase PunchLeft;
//         public CharacterActionBase PunchRight;
//         public CharacterActionBase Slam;
//         public CharacterActionBase Rest;
//         public CharacterActionBase Wag;
//         public CharacterActionBase Shout;
//         public CharacterActionBase Hurt;

//         // Novos ataques extras
//         public CharacterActionBase Attack1;
//         public CharacterActionBase Attack2;
//         public CharacterActionBase Attack3;
//         public CharacterActionBase Attack4;
//         public CharacterActionBase Attack5;
//         public CharacterActionBase Attack6;
//         public CharacterActionBase Attack7;
//         public CharacterActionBase Attack8;
//         public CharacterActionBase Attack9;
//         public CharacterActionBase Attack10;
//         public CharacterActionBase Attack11;
//         public CharacterActionBase Attack12;
//         public CharacterActionBase Attack13;
//         public CharacterActionBase Attack14;
//         public CharacterActionBase Attack15;

//         private bool _dead;
//         private int _energy = 10;
//         private IEnumerator<CharacterActionBase> _currentActions;

//         protected override void Awake()
//         {
//             base.Awake();
//             Actor.Idling += think;
//             ValidateActions();
//         }

//         private void Update()
//         {
//             Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
//             Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
//         }

//         public override bool CheckGuardBreak(IDamageSender damageSender)
//         {
//             return PunchLeft != null && (PunchLeft.IsHappening || (PunchRight != null && PunchRight.IsHappening));
//         }

//         public override void StartGuardBreak()
//         {
//             _energy = 0;
//             _currentActions = null;
//             if (Hurt != null) Hurt.StartAction(force: true);
//         }

//         protected override void die(Vector3 force)
//         {
//             _dead = true;

//             SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(
//                 SoulsCommons.Instance.Experience,
//                 AttributePool.GetValue(SoulsCommons.Instance.Experience)
//             );

//             GameObject disabler = new GameObject("disabler");
//             disabler.transform.SetParent(transform.parent, false);
//             disabler.SetActive(false);

//             var ragdoll = Instantiate(RagdollPrefab, disabler.transform);
//             AdventureHelper.CopyTransforms(ragdoll, Model);
//             AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

//             ragdoll.position = Model.position;
//             ragdoll.rotation = Model.rotation;
//             ragdoll.SetParent(transform.parent, false);

//             Destroy(disabler);
//             Destroy(gameObject);
//         }

//         private void think()
//         {
//             if (_dead) return;

//             if (_currentActions == null || !_currentActions.MoveNext())
//             {
//                 _currentActions = getActions();
//                 _currentActions?.MoveNext();
//             }

//             if (_currentActions?.Current != null)
//                 _currentActions.Current.StartAction(Actor);
//         }

//         private IEnumerator<CharacterActionBase> getActions()
//         {
//             if (_energy <= 0)
//             {
//                 foreach (var action in rest())
//                     yield return action;
//                 yield break;
//             }

//             if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
//             {
//                 foreach (var action in wag())
//                     yield return action;
//                 yield break;
//             }

//             _energy -= 7;

//             List<CharacterActionBase> possibleActions = new List<CharacterActionBase>
//             {
//                 Chase, PunchLeft, PunchRight, Slam, Shout,
//                 Attack1, Attack2, Attack3, Attack4, Attack5,
//                 Attack6, Attack7, Attack8, Attack9, Attack10,
//                 Attack11, Attack12, Attack13, Attack14, Attack15
//             };

//             possibleActions.RemoveAll(action => action == null);

//             for (int i = 0; i < possibleActions.Count; i++)
//             {
//                 var temp = possibleActions[i];
//                 int randomIndex = Random.Range(i, possibleActions.Count);
//                 possibleActions[i] = possibleActions[randomIndex];
//                 possibleActions[randomIndex] = temp;
//             }

//             int actionsToExecute = Random.Range(1, 23);

//             for (int i = 0; i < actionsToExecute && i < possibleActions.Count; i++)
//             {
//                 yield return possibleActions[i];
//             }
//         }

//         private IEnumerable<CharacterActionBase> rest()
//         {
//             if (Rest != null) yield return Rest;
//             _energy = 10;
//         }

//         private IEnumerable<CharacterActionBase> wag()
//         {
//             if (Wag != null) yield return Wag;
//             _energy += 3;
//         }

//         private void ValidateActions()
//         {
//             if (Chase == null) Debug.LogWarning($"{name}: Chase action está nula!");
//             if (PunchLeft == null) Debug.LogWarning($"{name}: PunchLeft action está nula!");
//             if (PunchRight == null) Debug.LogWarning($"{name}: PunchRight action está nula!");
//             if (Slam == null) Debug.LogWarning($"{name}: Slam action está nula!");
//             if (Rest == null) Debug.LogWarning($"{name}: Rest action está nula!");
//             if (Wag == null) Debug.LogWarning($"{name}: Wag action está nula!");
//             if (Shout == null) Debug.LogWarning($"{name}: Shout action está nula!");
//             if (Hurt == null) Debug.LogWarning($"{name}: Hurt action está nula!");

//             for (int i = 1; i <= 15; i++)
//             {
//                 var atk = GetType().GetField($"Attack{i}")?.GetValue(this);
//                 if (atk == null)
//                     Debug.LogWarning($"{name}: Attack{i} action está nula!");
//             }
//         }
//     }
// }






// using AdventureCore;
// using AdventureSouls;
// using System.Collections.Generic;
// using UnityEngine;

// namespace AdventureExtras
// {
//     public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
//     {
//         public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

//         [Header("Actions")]
//         public CharacterActionBase Chase;
//         public CharacterActionBase PunchLeft;
//         public CharacterActionBase PunchRight;
//         public CharacterActionBase Slam;
//         public CharacterActionBase Rest;
//         public CharacterActionBase Wag;
//         public CharacterActionBase Shout;
//         public CharacterActionBase Hurt;

//         // Novos ataques extras
//         public CharacterActionBase Attack1;
//         public CharacterActionBase Attack2;
//         public CharacterActionBase Attack3;
//         public CharacterActionBase Attack4;
//         public CharacterActionBase Attack5;
//         public CharacterActionBase Attack6;
//         public CharacterActionBase Attack7;
//         public CharacterActionBase Attack8;
//         public CharacterActionBase Attack9;
//         public CharacterActionBase Attack10;
//         public CharacterActionBase Attack11;
//         public CharacterActionBase Attack12;
//         public CharacterActionBase Attack13;
//         public CharacterActionBase Attack14;
//         public CharacterActionBase Attack15;

//         private bool _dead;
//         private int _energy = 10;
//         private IEnumerator<CharacterActionBase> _currentActions;
//         private Transform _playerTransform;
//         public float attackTriggerDistance = 3f;

//         protected override void Awake()
//         {
//             base.Awake();
//             Actor.Idling += think;
//             ValidateActions();

//             GameObject player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null)
//             {
//                 _playerTransform = player.transform;
//                 Movement.SetTarget(_playerTransform);
//             }
//         }

//         private void Update()
//         {
//             Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
//             Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
//         }

//         public override bool CheckGuardBreak(IDamageSender damageSender)
//         {
//             return PunchLeft != null && (PunchLeft.IsHappening || (PunchRight != null && PunchRight.IsHappening));
//         }

//         public override void StartGuardBreak()
//         {
//             _energy = 0;
//             _currentActions = null;
//             if (Hurt != null) Hurt.StartAction(force: true);
//         }

//         protected override void die(Vector3 force)
//         {
//             _dead = true;

//             SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(
//                 SoulsCommons.Instance.Experience,
//                 AttributePool.GetValue(SoulsCommons.Instance.Experience)
//             );

//             GameObject disabler = new GameObject("disabler");
//             disabler.transform.SetParent(transform.parent, false);
//             disabler.SetActive(false);

//             var ragdoll = Instantiate(RagdollPrefab, disabler.transform);
//             AdventureHelper.CopyTransforms(ragdoll, Model);
//             AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

//             ragdoll.position = Model.position;
//             ragdoll.rotation = Model.rotation;
//             ragdoll.SetParent(transform.parent, false);

//             Destroy(disabler);
//             Destroy(gameObject);
//         }

//         private void think()
//         {
//             if (_dead) return;

//             if (_currentActions == null || !_currentActions.MoveNext())
//             {
//                 _currentActions = getActions();
//                 _currentActions?.MoveNext();
//             }

//             if (_currentActions?.Current != null)
//                 _currentActions.Current.StartAction(Actor);
//         }

//         private IEnumerator<CharacterActionBase> getActions()
//         {
//             if (_energy <= 0)
//             {
//                 foreach (var action in rest())
//                     yield return action;
//                 yield break;
//             }

//             if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
//             {
//                 foreach (var action in wag())
//                     yield return action;
//                 yield break;
//             }

//             _energy -= 7;

//             float distanceToPlayer = Mathf.Infinity;
//             if (_playerTransform != null)
//                 distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

//             List<CharacterActionBase> meleeActions = new List<CharacterActionBase>
//             {
//                 PunchLeft, Slam, Shout, Hurt,
//                 Attack1, Attack2, Attack3, Attack4, Attack5,
//                 Attack6, Attack7, Attack8, Attack9, Attack10,
//                 Attack11, Attack12, Attack13, Attack14, Attack15
//             };

//             List<CharacterActionBase> rangedActions = new List<CharacterActionBase>
//             {
//                 Chase, PunchRight, Rest, Wag
//             };

//             List<CharacterActionBase> possibleActions = distanceToPlayer <= attackTriggerDistance
//                 ? meleeActions
//                 : rangedActions;

//             possibleActions.RemoveAll(a => a == null);

//             if (possibleActions.Count == 0)
//                 yield break;

//             // Embaralha
//             for (int i = 0; i < possibleActions.Count; i++)
//             {
//                 var temp = possibleActions[i];
//                 int randomIndex = Random.Range(i, possibleActions.Count);
//                 possibleActions[i] = possibleActions[randomIndex];
//                 possibleActions[randomIndex] = temp;
//             }

//             // Executa até 2 ações aleatórias
//             int actionsToExecute = Mathf.Min(Random.Range(1, 23), possibleActions.Count);

//             for (int i = 0; i < actionsToExecute; i++)
//             {
//                 yield return possibleActions[i];
//             }
//         }

//         private IEnumerable<CharacterActionBase> rest()
//         {
//             if (Rest != null) yield return Rest;
//             _energy = 10;
//         }

//         private IEnumerable<CharacterActionBase> wag()
//         {
//             if (Wag != null) yield return Wag;
//             _energy += 3;
//         }

//         private void ValidateActions()
//         {
//             if (Chase == null) Debug.LogWarning($"{name}: Chase action está nula!");
//             if (PunchLeft == null) Debug.LogWarning($"{name}: PunchLeft action está nula!");
//             if (PunchRight == null) Debug.LogWarning($"{name}: PunchRight action está nula!");
//             if (Slam == null) Debug.LogWarning($"{name}: Slam action está nula!");
//             if (Rest == null) Debug.LogWarning($"{name}: Rest action está nula!");
//             if (Wag == null) Debug.LogWarning($"{name}: Wag action está nula!");
//             if (Shout == null) Debug.LogWarning($"{name}: Shout action está nula!");
//             if (Hurt == null) Debug.LogWarning($"{name}: Hurt action está nula!");

//             for (int i = 1; i <= 15; i++)
//             {
//                 var atk = GetType().GetField($"Attack{i}")?.GetValue(this);
//                 if (atk == null)
//                     Debug.LogWarning($"{name}: Attack{i} action está nula!");
//             }
//         }
//     }
// }



// using AdventureCore;
// using AdventureSouls;
// using System.Collections.Generic;
// using UnityEngine;

// namespace AdventureExtras
// {
//     public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
//     {
//         public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

//         [Header("Actions")]
//         public CharacterActionBase Chase;
//         public CharacterActionBase PunchLeft;
//         public CharacterActionBase PunchRight;
//         public CharacterActionBase Slam;
//         public CharacterActionBase Rest;
//         public CharacterActionBase Wag;
//         public CharacterActionBase Shout;
//         public CharacterActionBase Hurt;

//         public CharacterActionBase Attack1;
//         public CharacterActionBase Attack2;
//         public CharacterActionBase Attack3;
//         public CharacterActionBase Attack4;
//         public CharacterActionBase Attack5;
//         public CharacterActionBase Attack6;
//         public CharacterActionBase Attack7;
//         public CharacterActionBase Attack8;
//         public CharacterActionBase Attack9;
//         public CharacterActionBase Attack10;
//         public CharacterActionBase Attack11;
//         public CharacterActionBase Attack12;
//         public CharacterActionBase Attack13;
//         public CharacterActionBase Attack14;
//         public CharacterActionBase Attack15;

//         private bool _dead;
//         private int _energy = 10;
//         private IEnumerator<CharacterActionBase> _currentActions;
//         private Transform _playerTransform;
//         public float attackTriggerDistance = 3f;

//         protected override void Awake()
//         {
//             base.Awake();
//             Actor.Idling += think;
//             ValidateActions();

//             GameObject player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null)
//             {
//                 _playerTransform = player.transform;
//                 Movement.SetTarget(_playerTransform);
//             }
//         }

//         private void Update()
//         {
//             Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
//             Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
//         }

//         public override bool CheckGuardBreak(IDamageSender damageSender)
//         {
//             return PunchLeft != null && (PunchLeft.IsHappening || (PunchRight != null && PunchRight.IsHappening));
//         }

//         public override void StartGuardBreak()
//         {
//             _energy = 0;
//             _currentActions = null;
//             if (Hurt != null) Hurt.StartAction(force: true);
//         }

//         protected override void die(Vector3 force)
//         {
//             _dead = true;

//             SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(
//                 SoulsCommons.Instance.Experience,
//                 AttributePool.GetValue(SoulsCommons.Instance.Experience)
//             );

//             GameObject disabler = new GameObject("disabler");
//             disabler.transform.SetParent(transform.parent, false);
//             disabler.SetActive(false);

//             var ragdoll = Instantiate(RagdollPrefab, disabler.transform);
//             AdventureHelper.CopyTransforms(ragdoll, Model);
//             AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

//             ragdoll.position = Model.position;
//             ragdoll.rotation = Model.rotation;
//             ragdoll.SetParent(transform.parent, false);

//             Destroy(disabler);
//             Destroy(gameObject);
//         }

//         private void think()
//         {
//             if (_dead) return;

//             if (_currentActions == null || !_currentActions.MoveNext())
//             {
//                 _currentActions = getActions();
//                 _currentActions?.MoveNext();
//             }

//             if (_currentActions?.Current != null)
//                 _currentActions.Current.StartAction(Actor);
//         }

//         private IEnumerator<CharacterActionBase> getActions()
//         {
//             if (_energy <= 0)
//             {
//                 foreach (var action in rest())
//                     yield return action;
//                 yield break;
//             }

//             if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
//             {
//                 foreach (var action in wag())
//                     yield return action;
//                 yield break;
//             }

//             _energy -= 7;

//             float distanceToPlayer = Mathf.Infinity;
//             if (_playerTransform != null)
//                 distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

//              // distancia do Player <= que do Boss attackTriggerDistance ativar as animações rangedActions

//             List<CharacterActionBase> meleeActions = new List<CharacterActionBase>
//             {
//                 PunchLeft, Slam, Shout, Hurt,
//                 Attack1, Attack2, Attack3, Attack4, Attack5,
//                 Attack6, Attack7, Attack8, Attack9, Attack10,
//                 Attack11, Attack12, Attack13, Attack14, Attack15
//             };

//              // distancia do Player >= que do Boss attackTriggerDistance ativar as animações rangedActions

//             List<CharacterActionBase> rangedActions = new List<CharacterActionBase>
//             {
//                 Chase, PunchRight, Rest, Wag
//             };

//             List<CharacterActionBase> possibleActions = new List<CharacterActionBase>();

//             if (distanceToPlayer <= attackTriggerDistance)
//             {
//                 possibleActions.AddRange(meleeActions);
//             }
//             else
//             {
//                 possibleActions.AddRange(rangedActions);
//             }

//             possibleActions.RemoveAll(a => a == null);

//             if (possibleActions.Count == 0)
//                 yield break;

//             // Embaralha a lista
//             for (int i = 0; i < possibleActions.Count; i++)
//             {
//                 var temp = possibleActions[i];
//                 int randomIndex = Random.Range(i, possibleActions.Count);
//                 possibleActions[i] = possibleActions[randomIndex];
//                 possibleActions[randomIndex] = temp;
//             }

//             // Executa até 3 ações aleatórias do grupo selecionado
//             int actionsToExecute = Mathf.Min(Random.Range(1, 24), possibleActions.Count);

//             for (int i = 0; i < actionsToExecute; i++)
//             {
//                 yield return possibleActions[i];
//             }
//         }

//         private IEnumerable<CharacterActionBase> rest()
//         {
//             if (Rest != null) yield return Rest;
//             _energy = 10;
//         }

//         private IEnumerable<CharacterActionBase> wag()
//         {
//             if (Wag != null) yield return Wag;
//             _energy += 3;
//         }

//         private void ValidateActions()
//         {
//             if (Chase == null) Debug.LogWarning($"{name}: Chase action está nula!");
//             if (PunchLeft == null) Debug.LogWarning($"{name}: PunchLeft action está nula!");
//             if (PunchRight == null) Debug.LogWarning($"{name}: PunchRight action está nula!");
//             if (Slam == null) Debug.LogWarning($"{name}: Slam action está nula!");
//             if (Rest == null) Debug.LogWarning($"{name}: Rest action está nula!");
//             if (Wag == null) Debug.LogWarning($"{name}: Wag action está nula!");
//             if (Shout == null) Debug.LogWarning($"{name}: Shout action está nula!");
//             if (Hurt == null) Debug.LogWarning($"{name}: Hurt action está nula!");

//             for (int i = 1; i <= 15; i++)
//             {
//                 var atk = GetType().GetField($"Attack{i}")?.GetValue(this);
//                 if (atk == null)
//                     Debug.LogWarning($"{name}: Attack{i} action está nula!");
//             }
//         }
//     }
// }




// using AdventureCore;
// using AdventureSouls;
// using System.Collections.Generic;
// using UnityEngine;

// namespace AdventureExtras
// {
//     public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
//     {
//         public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

//         [Header("Actions")]
//         public CharacterActionBase Chase;
//         public CharacterActionBase PunchLeft;
//         public CharacterActionBase PunchRight;
//         public CharacterActionBase Slam;
//         public CharacterActionBase Rest;
//         public CharacterActionBase Wag;
//         public CharacterActionBase Shout;
//         public CharacterActionBase Hurt;

//         public CharacterActionBase Attack1;
//         public CharacterActionBase Attack2;
//         public CharacterActionBase Attack3;
//         public CharacterActionBase Attack4;
//         public CharacterActionBase Attack5;
//         public CharacterActionBase Attack6;
//         public CharacterActionBase Attack7;
//         public CharacterActionBase Attack8;
//         public CharacterActionBase Attack9;
//         public CharacterActionBase Attack10;
//         public CharacterActionBase Attack11;
//         public CharacterActionBase Attack12;
//         public CharacterActionBase Attack13;
//         public CharacterActionBase Attack14;
//         public CharacterActionBase Attack15;

//         private bool _dead;
//         private int _energy = 10;
//         private IEnumerator<CharacterActionBase> _currentActions;
//         private Transform _playerTransform;
//         public float attackTriggerDistance = 2.55f;

//         protected override void Awake()
//         {
//             base.Awake();
//             Actor.Idling += think;
//             ValidateActions();

//             GameObject player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null)
//             {
//                 _playerTransform = player.transform;
//                 Movement.SetTarget(_playerTransform);
//             }
//         }

//         private void Update()
//         {
//             Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
//             Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
//         }

//         public override bool CheckGuardBreak(IDamageSender damageSender)
//         {
//             return PunchLeft != null && (PunchLeft.IsHappening || (PunchRight != null && PunchRight.IsHappening));
//         }

//         public override void StartGuardBreak()
//         {
//             _energy = 0;
//             _currentActions = null;
//             if (Hurt != null) Hurt.StartAction(force: true);
//         }

//         protected override void die(Vector3 force)
//         {
//             _dead = true;

//             SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(
//                 SoulsCommons.Instance.Experience,
//                 AttributePool.GetValue(SoulsCommons.Instance.Experience)
//             );

//             GameObject disabler = new GameObject("disabler");
//             disabler.transform.SetParent(transform.parent, false);
//             disabler.SetActive(false);

//             var ragdoll = Instantiate(RagdollPrefab, disabler.transform);
//             AdventureHelper.CopyTransforms(ragdoll, Model);
//             AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

//             ragdoll.position = Model.position;
//             ragdoll.rotation = Model.rotation;
//             ragdoll.SetParent(transform.parent, false);

//             Destroy(disabler);
//             Destroy(gameObject);
//         }

//         private void think()
//         {
//             if (_dead) return;

//             if (_currentActions == null || !_currentActions.MoveNext())
//             {
//                 _currentActions = getActions();
//                 _currentActions?.MoveNext();
//             }

//             if (_currentActions?.Current != null)
//                 _currentActions.Current.StartAction(Actor);
//         }

//         private IEnumerator<CharacterActionBase> getActions()
//         {
//             if (_energy <= 0)
//             {
//                 foreach (var action in rest())
//                     yield return action;
//                 yield break;
//             }

//             if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
//             {
//                 foreach (var action in wag())
//                     yield return action;
//                 yield break;
//             }

//             _energy -= 7;

//             if (_playerTransform == null)
//             {
//                 GameObject player = GameObject.FindGameObjectWithTag("Player");
//                 if (player != null)
//                     _playerTransform = player.transform;
//             }

//             // float distanceToPlayer = Mathf.Infinity;
//             // if (_playerTransform != null)
//             //     distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);


//             float distanceToPlayer = Vector3.Distance(
//                 new Vector3(transform.position.x, 0f, transform.position.z),
//                 new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z)
//             );

//             Debug.Log($"[SoulsHeroApe] Distance to Player: {distanceToPlayer:F2}");

//             List<CharacterActionBase> meleeActions = new List<CharacterActionBase>
//             {
//                 Attack1, Attack2, Attack3, Attack4, Attack5,
//                 Attack6, Attack7, PunchLeft, Slam, Shout, Hurt, Attack8, Attack9, Attack10,
//                 Attack11, Attack12, Attack13, Attack14, Attack15
//             };

//             List<CharacterActionBase> rangedActions = new List<CharacterActionBase>
//             {
//                 Chase, Wag, Chase, PunchRight
//             };

//             List<CharacterActionBase> possibleActions = new List<CharacterActionBase>();

//             if (distanceToPlayer <= attackTriggerDistance)
//             {
//                 Debug.Log("[SoulsHeroApe] Player is in melee range");
//                 possibleActions.AddRange(meleeActions);
//             }
//             else
//             {
//                 Debug.Log("[SoulsHeroApe] Player is in ranged range");
//                 possibleActions.AddRange(rangedActions);
//             }

//             possibleActions.RemoveAll(a => a == null);

//             if (possibleActions.Count == 0)
//                 yield break;

//             for (int i = 0; i < possibleActions.Count; i++)
//             {
//                 var temp = possibleActions[i];
//                 int randomIndex = Random.Range(i, possibleActions.Count);
//                 possibleActions[i] = possibleActions[randomIndex];
//                 possibleActions[randomIndex] = temp;
//             }

//             int actionsToExecute = Mathf.Min(Random.Range(1, 25), possibleActions.Count);

//             for (int i = 0; i < actionsToExecute; i++)
//             {
//                 yield return possibleActions[i];
//             }
//         }

//         private IEnumerable<CharacterActionBase> rest()
//         {
//             if (Rest != null) yield return Rest;
//             _energy = 10;
//         }

//         private IEnumerable<CharacterActionBase> wag()
//         {
//             if (Wag != null) yield return Wag;
//             _energy += 3;
//         }

//         private void ValidateActions()
//         {
//             if (Chase == null) Debug.LogWarning($"{name}: Chase action está nula!");
//             if (PunchLeft == null) Debug.LogWarning($"{name}: PunchLeft action está nula!");
//             if (PunchRight == null) Debug.LogWarning($"{name}: PunchRight action está nula!");
//             if (Slam == null) Debug.LogWarning($"{name}: Slam action está nula!");
//             if (Rest == null) Debug.LogWarning($"{name}: Rest action está nula!");
//             if (Wag == null) Debug.LogWarning($"{name}: Wag action está nula!");
//             if (Shout == null) Debug.LogWarning($"{name}: Shout action está nula!");
//             if (Hurt == null) Debug.LogWarning($"{name}: Hurt action está nula!");

//             for (int i = 1; i <= 15; i++)
//             {
//                 var atk = GetType().GetField($"Attack{i}")?.GetValue(this);
//                 if (atk == null)
//                     Debug.LogWarning($"{name}: Attack{i} action está nula!");
//             }
//         }
//     }
// }



// using AdventureCore;
// using AdventureSouls;
// using System.Collections.Generic;
// using UnityEngine;

// namespace AdventureExtras
// {
//     public class SoulsHeroApe : SoulsCharacterBase<SerialCharacterActor, MovementBase, InventoryBase>, IGetGuardBroken
//     {
//         public static int SPEED_SIDE_HASH = Animator.StringToHash("SpeedSideways");

//         [Header("Actions")]
//         public CharacterActionBase Chase;
//         public CharacterActionBase PunchLeft;
//         public CharacterActionBase PunchRight;
//         public CharacterActionBase Slam;
//         public CharacterActionBase Rest;
//         public CharacterActionBase Wag;
//         public CharacterActionBase Shout;
//         public CharacterActionBase Hurt;

//         public CharacterActionBase Attack1;
//         public CharacterActionBase Attack2;
//         public CharacterActionBase Attack3;
//         public CharacterActionBase Attack4;
//         public CharacterActionBase Attack5;
//         public CharacterActionBase Attack6;
//         public CharacterActionBase Attack7;
//         public CharacterActionBase Attack8;
//         public CharacterActionBase Attack9;
//         public CharacterActionBase Attack10;
//         public CharacterActionBase Attack11;
//         public CharacterActionBase Attack12;
//         public CharacterActionBase Attack13;
//         public CharacterActionBase Attack14;
//         public CharacterActionBase Attack15;

//         private bool _dead;
//         private int _energy = 10;
//         private IEnumerator<CharacterActionBase> _currentActions;
//         private Transform _playerTransform;
//         public float attackTriggerDistance = 2.55f;

//         protected override void Awake()
//         {
//             base.Awake();
//             Actor.Idling += think;
//             ValidateActions();

//             GameObject player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null)
//             {
//                 _playerTransform = player.transform;
//                 Movement.SetTarget(_playerTransform); // mantém o movimento
//             }
//         }

//         private void Update()
//         {
//             Animator.SetFloat(SPEED_HASH, Movement.SpeedFactorForward);
//             Animator.SetFloat(SPEED_SIDE_HASH, Movement.SpeedFactorSideways);
//         }

//         public override bool CheckGuardBreak(IDamageSender damageSender)
//         {
//             return PunchLeft != null && (PunchLeft.IsHappening || (PunchRight != null && PunchRight.IsHappening));
//         }

//         public override void StartGuardBreak()
//         {
//             _energy = 0;
//             _currentActions = null;
//             if (Hurt != null) Hurt.StartAction(force: true);
//         }

//         protected override void die(Vector3 force)
//         {
//             _dead = true;

//             SoulsCommons.Instance.PlayerCharacter.AttributePool.Add(
//                 SoulsCommons.Instance.Experience,
//                 AttributePool.GetValue(SoulsCommons.Instance.Experience)
//             );

//             GameObject disabler = new GameObject("disabler");
//             disabler.transform.SetParent(transform.parent, false);
//             disabler.SetActive(false);

//             var ragdoll = Instantiate(RagdollPrefab, disabler.transform);
//             AdventureHelper.CopyTransforms(ragdoll, Model);
//             AdventureHelper.PushAllRigidbodies(ragdoll.gameObject, force + Movement.Velocity);

//             ragdoll.position = Model.position;
//             ragdoll.rotation = Model.rotation;
//             ragdoll.SetParent(transform.parent, false);

//             Destroy(disabler);
//             Destroy(gameObject);
//         }

//         private void think()
//         {
//             if (_dead) return;

//             if (_currentActions == null || !_currentActions.MoveNext())
//             {
//                 _currentActions = getActions();
//                 _currentActions?.MoveNext();
//             }

//             if (_currentActions?.Current != null)
//                 _currentActions.Current.StartAction(Actor);
//         }

//         private IEnumerator<CharacterActionBase> getActions()
//         {
//             if (_energy <= 0)
//             {
//                 foreach (var action in rest())
//                     yield return action;
//                 yield break;
//             }

//             if (_energy < 5 && Random.Range(0f, 1f) > 0.5f)
//             {
//                 foreach (var action in wag())
//                     yield return action;
//                 yield break;
//             }

//             _energy -= 7;

//             if (_playerTransform == null)
//             {
//                 GameObject player = GameObject.FindGameObjectWithTag("Player");
//                 if (player != null)
//                     _playerTransform = player.transform;
//             }

//             float distanceToPlayer = Vector3.Distance(
//                 new Vector3(transform.position.x, 0f, transform.position.z),
//                 new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z)
//             );

//             Debug.Log($"[SoulsHeroApe] Distance to Player: {distanceToPlayer:F2}");

//             List<CharacterActionBase> meleeActions = new List<CharacterActionBase>
//             {
//                 Attack1, Attack2, Attack3, Attack4, Attack5,
//                 Attack6, Attack7, PunchLeft, Slam, Shout, Hurt, Attack8, Attack9, Attack10,
//                 Attack11, Attack12, Attack13, Attack14, Attack15
//             };

//             List<CharacterActionBase> rangedActions = new List<CharacterActionBase>
//             {
//                 Chase, Wag, Chase, PunchRight
//             };

//             List<CharacterActionBase> possibleActions = distanceToPlayer <= attackTriggerDistance
//                 ? meleeActions
//                 : rangedActions;

//             possibleActions.RemoveAll(a => a == null);

//             if (possibleActions.Count == 0)
//                 yield break;

//             // Embaralha as ações
//             for (int i = 0; i < possibleActions.Count; i++)
//             {
//                 var temp = possibleActions[i];
//                 int randomIndex = Random.Range(i, possibleActions.Count);
//                 possibleActions[i] = possibleActions[randomIndex];
//                 possibleActions[randomIndex] = temp;
//             }

//             int actionsToExecute = Mathf.Min(Random.Range(1, 25), possibleActions.Count);

//             for (int i = 0; i < actionsToExecute; i++)
//             {
//                 yield return possibleActions[i];
//             }
//         }

//         private IEnumerable<CharacterActionBase> rest()
//         {
//             if (Rest != null) yield return Rest;
//             _energy = 10;
//         }

//         private IEnumerable<CharacterActionBase> wag()
//         {
//             if (Wag != null) yield return Wag;
//             _energy += 3;
//         }

//         private void ValidateActions()
//         {
//             if (Chase == null) Debug.LogWarning($"{name}: Chase action está nula!");
//             if (PunchLeft == null) Debug.LogWarning($"{name}: PunchLeft action está nula!");
//             if (PunchRight == null) Debug.LogWarning($"{name}: PunchRight action está nula!");
//             if (Slam == null) Debug.LogWarning($"{name}: Slam action está nula!");
//             if (Rest == null) Debug.LogWarning($"{name}: Rest action está nula!");
//             if (Wag == null) Debug.LogWarning($"{name}: Wag action está nula!");
//             if (Shout == null) Debug.LogWarning($"{name}: Shout action está nula!");
//             if (Hurt == null) Debug.LogWarning($"{name}: Hurt action está nula!");

//             for (int i = 1; i <= 15; i++)
//             {
//                 var atk = GetType().GetField($"Attack{i}")?.GetValue(this);
//                 if (atk == null)
//                     Debug.LogWarning($"{name}: Attack{i} action está nula!");
//             }
//         }
//     }
// }

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
