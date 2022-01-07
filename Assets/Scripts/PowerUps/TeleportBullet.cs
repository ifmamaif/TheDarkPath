
namespace TheDarkPath
{
    public class TeleportBullet : ProjectileLogic
    {
        protected override void OnExpire()
        {
            TeleportPlayer();
            base.OnExpire();
        }

        protected override void OnTargetHit()
        {
            TeleportPlayer();
            Destroy(this.gameObject);
        }

        protected override void IHitSomething()
        {
            TeleportPlayer();
        }

        private void TeleportPlayer()
        {
            unit.transform.position = this.transform.position;
        }
    }
}