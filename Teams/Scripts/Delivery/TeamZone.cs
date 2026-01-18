using UnityEngine;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Teams.Scripts.Delivery
{
    [RequireComponent(typeof(Collider))]
    public class TeamZone : MonoBehaviour
    {
        [Tooltip("El equipo al que pertenece esta zona.")]
        [SerializeField]
        private Team _team;
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        public void Initialize(Team team)
        {
            _team = team;
        }

        public Team GetTeam() => _team;
    }
}