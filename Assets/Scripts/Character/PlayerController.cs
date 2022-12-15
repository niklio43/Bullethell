using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PlayerController : Character
{
    [SerializeField] VisualEffect heal, takeDamage;
    public PlayerStats stats { get { return (PlayerStats)characterStats; } }

    protected override void Init()
    {
        //heal.Stop();
        //takeDamage.Stop();
    }

    public override void TakeDamage(float value)
    {
        float mitigatedDamage = value / (stats.Defense / 10);
        base.TakeDamage(mitigatedDamage);
        takeDamage.Play();
        AudioManager.instance.PlayOnce("PlayerDamage");
        //CameraShake.Shake(0.05f, 0.1f);
    }

    protected override void OnDeath()
    {
        SceneManager.LoadScene("GameOver");
    }

    public override void Heal(float value)
    {
        base.Heal(value);
        heal.Play();
        AudioManager.instance.PlayOnce("PlayerHeal");
    }
}