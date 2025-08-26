using UnityEngine;

namespace FPSEngine.Source.DI.Containers
{
    public readonly struct WeaponContainer
    {
        public WeaponContainer(GameObject rocketPrefab, GameObject explosionPrefab, GameObject spawnPoint, Sprite idlePistol,
            Sprite shotPistol, GameObject bulletHolePrefab)
        {
            this.rocketPrefab = rocketPrefab;
            this.explosionPrefab = explosionPrefab;
            this.spawnPoint = spawnPoint;
            this.idlePistol = idlePistol;
            this.shotPistol = shotPistol;
            this.bulletHolePrefab = bulletHolePrefab;
        }

        readonly GameObject rocketPrefab;
        readonly GameObject explosionPrefab;
        readonly GameObject spawnPoint;
        readonly Sprite idlePistol;
        readonly Sprite shotPistol;
        readonly GameObject bulletHolePrefab;

        public GameObject RocketPrefab => rocketPrefab;
        public GameObject ExplosionPrefab => explosionPrefab;
        public GameObject SpawnPoint => spawnPoint;
        public Sprite IdlePistol => idlePistol;
        public Sprite ShotPistol => shotPistol;
        public GameObject BulletHolePrefab => bulletHolePrefab;
    }
}