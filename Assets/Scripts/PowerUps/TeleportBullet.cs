
namespace TheDarkPath
{
    public class TeleportBullet : ProjectileLogic
    {
        protected override void OnExpire()
        {
            unit.transform.position = this.transform.position;
            base.OnExpire();
        }
        protected override void OnTargetHit()
        {
            unit.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}