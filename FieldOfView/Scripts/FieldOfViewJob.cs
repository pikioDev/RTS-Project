using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Xenocode.Features.FieldOfView.Scripts
{
    [BurstCompile]
    public struct FieldOfViewJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> AgentPositions;
        [ReadOnly] public NativeArray<float3> AgentForwards;
        [ReadOnly] public NativeArray<FieldOfViewConfig> AgentConfigs;
        [ReadOnly] public NativeArray<float3> EnemyPositions;
        [WriteOnly] public NativeArray<int> PotentialTargetIndices;
        
        [ReadOnly] public NativeArray<int> AgentTeams;
        [ReadOnly] public NativeArray<int> TargetTeams;
        
        public void Execute(int index)
        {
            if (!EnemyPositions.IsCreated || EnemyPositions.Length == 0)
            {
                PotentialTargetIndices[index] = -1;
                return;
            }
            var agentPos = AgentPositions[index];
            var agentForward = AgentForwards[index];
            var config = AgentConfigs[index];
            var agentTeam = AgentTeams[index]; 
            float bestTargetDistSq = float.MaxValue;
            int bestTargetIndex = -1;

            for (int i = 0; i < EnemyPositions.Length; i++)
            {
                var targetTeam = TargetTeams[i];
                if (agentTeam == targetTeam)
                {
                    continue; 
                }

                var enemyPos = EnemyPositions[i];
                
                if (!IsPotentialTarget(agentPos, agentForward, enemyPos, config)) continue;

                float distSq = math.lengthsq(enemyPos - agentPos);
                if (distSq < bestTargetDistSq)
                {
                    bestTargetDistSq = distSq;
                    bestTargetIndex = i;
                }
            }
            PotentialTargetIndices[index] = bestTargetIndex;
        }

        private bool IsPotentialTarget(float3 agentPos, float3 agentForward, float3 enemyPos, FieldOfViewConfig config)
        {
            float3 toEnemy = enemyPos - agentPos;
            float distSq = math.lengthsq(toEnemy);
            
            if (distSq > config.viewRadius * config.viewRadius) return false;

            float3 toEnemyDir = math.normalize(toEnemy);
            float minDotProduct = math.cos(math.radians(config.viewAngle / 2f));
            float dotProduct = math.dot(agentForward, toEnemyDir);
            return dotProduct >= minDotProduct;
        }
    }
}