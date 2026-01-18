using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Xenocode.Features.FieldOfView.Scripts
{
    public class FieldOfViewJobManager : MonoBehaviour
    {
        public static FieldOfViewJobManager Instance { get; private set; }

        private readonly List<FieldOfViewAgent> _agents = new();
        private readonly List<FieldOfViewTarget> _targets = new();

        public void RegisterAgent(FieldOfViewAgent agent) => _agents.Add(agent);
        public void UnregisterAgent(FieldOfViewAgent agent) => _agents.Remove(agent);
        public void RegisterTarget(FieldOfViewTarget target) => _targets.Add(target);
        public void UnregisterTarget(FieldOfViewTarget target) => _targets.Remove(target);

        private void LateUpdate()
        {
            if (_agents.Count == 0) return;
            
            if (_targets.Count == 0)
            {
                foreach (var agent in _agents) agent.PotentialTarget = null;
                return;
            }
            
            var agentPositions = new NativeArray<float3>(_agents.Count, Allocator.TempJob);
            var agentForwards = new NativeArray<float3>(_agents.Count, Allocator.TempJob);
            var agentConfigs = new NativeArray<FieldOfViewConfig>(_agents.Count, Allocator.TempJob);
            var agentTeams = new NativeArray<int>(_agents.Count, Allocator.TempJob); 
            var enemyPositions = new NativeArray<float3>(_targets.Count, Allocator.TempJob);
            var targetTeams = new NativeArray<int>(_targets.Count, Allocator.TempJob); 
            var potentialTargetIndices = new NativeArray<int>(_agents.Count, Allocator.TempJob);

            for (int i = 0; i < _agents.Count; i++)
            {
                agentPositions[i] = _agents[i].transform.position;
                agentForwards[i] = _agents[i].transform.forward;
                agentTeams[i] = (int)_agents[i].Agent().GetTeam(); 
                agentConfigs[i] = new FieldOfViewConfig
                {
                    viewRadius = _agents[i].ViewRadius,
                    viewAngle = _agents[i].ViewAngle
                };
            }
            for (int i = 0; i < _targets.Count; i++)
            {
                enemyPositions[i] = _targets[i].transform.position;
                targetTeams[i] = (int)_targets[i].Target().GetTeam(); 
            }
            
            var job = new FieldOfViewJob
            {
                AgentPositions = agentPositions,
                AgentForwards = agentForwards,
                AgentConfigs = agentConfigs,
                EnemyPositions = enemyPositions,
                AgentTeams = agentTeams,             
                TargetTeams = targetTeams,           
                PotentialTargetIndices = potentialTargetIndices
            };

            JobHandle jobHandle = job.Schedule(_agents.Count, 32);
            jobHandle.Complete();
            
            for (int i = 0; i < _agents.Count; i++)
            {
                int targetIndex = potentialTargetIndices[i];
                _agents[i].PotentialTarget = targetIndex != -1 ? _targets[targetIndex].transform : null;
            }
            
            agentPositions.Dispose();
            agentForwards.Dispose();
            agentConfigs.Dispose();
            agentTeams.Dispose(); 
            enemyPositions.Dispose();
            targetTeams.Dispose();
            potentialTargetIndices.Dispose();
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}