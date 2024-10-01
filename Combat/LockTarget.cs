
using System;
using System.Collections.Generic;
using System.Linq;
using BLINK.RPGBuilder.Managers;
using BLINK.RPGBuilder.Characters;
using BLINK.RPGBuilder.Combat;
using BLINK.RPGBuilder.LogicMono;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace BLINK.RPGBuilder.Managers
{
    public class Target_system : MonoBehaviour
    {
        private CombatEntity currentLockedTarget;
        public CinemachineVirtualCamera cinemachineCamera;  // Referência à CinemachineVirtualCamera
        public float maxLockDistance = 20f;  // Distância máxima para manter o lock no alvo
        private bool isLockedOn;  // Estado de lock-on

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            // Somente tentar anexar a câmera se a cena ativa não for "MainMenu"
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                AttachVirtualCamera();
            }
        }

        private void AttachVirtualCamera()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                GameObject[] virtualCameraObjects = Resources.FindObjectsOfTypeAll<GameObject>()
                    .Where(obj => obj.CompareTag("VirtualCamera"))
                    .ToArray();

                if (virtualCameraObjects.Length > 0)
                {
                    GameObject virtualCameraObject = virtualCameraObjects[0];
                    cinemachineCamera = virtualCameraObject.GetComponent<CinemachineVirtualCamera>();
                    if (cinemachineCamera == null)
                    {
                        Debug.LogError("O objeto com a tag 'VirtualCamera' não contém um componente CinemachineVirtualCamera!");
                    }
                    else
                    {
                        Debug.Log("VirtualCamera anexada ao script Target_system.");
                    }
                }
                else
                {
                    Debug.LogError("Nenhum objeto com a tag 'VirtualCamera' foi encontrado na cena!");
                }
            }
            else
            {
                Invoke("AttachVirtualCamera", 0.5f);
            }
        }

        private Transform FindFollowTarget()
        {
            if (GameState.playerEntity != null)
            {
                Transform followTarget = GameState.playerEntity.transform.Find("Follow");
                if (followTarget != null && followTarget.gameObject.layer == LayerMask.NameToLayer("Follow") && followTarget.CompareTag("Follow"))
                {
                    return followTarget;
                }
            }
            return GameState.playerEntity.transform;
        }

        private Transform FindLookAtTarget()
        {
            if (GameState.playerEntity != null)
            {
                Transform lookAtTarget = GameState.playerEntity.transform.Find("LookAt");
                if (lookAtTarget != null && lookAtTarget.gameObject.layer == LayerMask.NameToLayer("LookAt") && lookAtTarget.CompareTag("LookAt"))
                {
                    return lookAtTarget;
                }
            }
            return GameState.playerEntity.transform;
        }

        public void SetCinemachineTarget(Transform followTarget, Transform lookAtTarget)
        {
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Follow = followTarget;
                cinemachineCamera.LookAt = lookAtTarget;
            }
        }

        private void Update()
        {
            if (GameState.playerEntity == null) return;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                HandleTabTarget();
            }

            if (currentLockedTarget != null)
            {
                UpdateTargetLock(currentLockedTarget);
            }
            else
            {
                if (cinemachineCamera != null && isLockedOn)
                {
                    cinemachineCamera.gameObject.SetActive(false);
                    isLockedOn = false;
                }

                if (isLockedOn)
                {
                    GameState.playerEntity.controllerEssentials.anim.SetBool("Target", false);
                }
            }
        }

        void HandleTabTarget()
        {
            float maxAngle = 90;
            float maxDist = 50;

            CombatEntity newTarget = null;
            float lastDist = 1000;
            int validTargets = 0;

            foreach (var cbtNode in GameState.combatEntities)
            {
                if (cbtNode == GameState.playerEntity) continue;
                CombatData.EntityAlignment thisNodeAlignment = FactionManager.Instance.GetAlignment(cbtNode.GetFaction(),
                    FactionManager.Instance.GetEntityStanceToFaction(GameState.playerEntity, cbtNode.GetFaction()));

                if (thisNodeAlignment == CombatData.EntityAlignment.Ally) continue;
                if (cbtNode == GameState.playerEntity.GetTarget()) continue;
                float thisDist = Vector3.Distance(cbtNode.transform.position,
                    GameState.playerEntity.transform.position);
                if (thisDist > maxDist) continue;
                var pointDirection = cbtNode.transform.position - GameState.playerEntity.transform.position;
                var angle = Vector3.Angle(GameState.playerEntity.transform.forward, pointDirection);
                if (!(angle < maxAngle)) continue;
                validTargets++;
                if (lastDist > thisDist)
                {
                    newTarget = cbtNode;
                    lastDist = thisDist;
                }
            }

            if (newTarget != null)
            {
                GameState.playerEntity.SetTarget(newTarget);
                currentLockedTarget = newTarget;

                isLockedOn = true;
                GameState.playerEntity.controllerEssentials.anim.SetBool("Target", true);

                if (cinemachineCamera != null)
                {
                    cinemachineCamera.gameObject.SetActive(true);
                    Transform followTarget = FindFollowTarget();
                    Transform lookAtTarget = currentLockedTarget.transform;
                    SetCinemachineTarget(followTarget, lookAtTarget);
                }
            }
        }

        private void UpdateTargetLock(CombatEntity target)
        {
            if (target == null || GameState.playerEntity == null || cinemachineCamera == null || target.IsDead())  // Verificação adicional para alvo destruído ou morto
            {
                // Desativar o lock-on e o parâmetro "Target"
                currentLockedTarget = null;
                isLockedOn = false;
                cinemachineCamera.gameObject.SetActive(false);
                GameState.playerEntity.controllerEssentials.anim.SetBool("Target", false);
                return;
            }

            float distance = Vector3.Distance(GameState.playerEntity.transform.position, target.transform.position);

            if (distance > maxLockDistance)
            {
                currentLockedTarget = null;
                isLockedOn = false;
                cinemachineCamera.gameObject.SetActive(false);
                GameState.playerEntity.controllerEssentials.anim.SetBool("Target", false);
                return;
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            if (!(moveX == -1 && moveY == 1) && !(moveX == 1 && moveY == 1))
            {
                Vector3 directionToTarget = target.transform.position - GameState.playerEntity.transform.position;
                directionToTarget.y = 0;
                GameState.playerEntity.transform.rotation = Quaternion.LookRotation(directionToTarget);
            }

            Vector3 strafeMovement = new Vector3(moveX, 0, moveY);
            Vector3 movement = Quaternion.Euler(0, GameState.playerEntity.transform.eulerAngles.y, 0) * strafeMovement;
            GameState.playerEntity.transform.Translate(movement * Time.deltaTime, Space.World);
        }

        public static Target_system Instance { get; private set; }
    }
}
