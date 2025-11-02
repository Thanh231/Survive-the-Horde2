
using UnityEngine;

public class PlayerVisual : ActorVisual
{
    [SerializeField] private GameObject m_deathVfx;
    public Player player;
    // private PlayerStates playerStates;
    private void Start()
    {
        // player = (Player)m_actor;
        // playerStates = player.PlayerStates;
    }
    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        GUIManager.Ins.UpdateHpInfo(player.currentHP,player.maxHP);
    }
    public void OnLostLife()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.lostLife);
    }
    public void OnDead()
    {
        if(m_deathVfx)
        {
            Instantiate(m_deathVfx,transform.position, Quaternion.identity);
        }

        AudioController.Ins.PlaySound(AudioController.Ins.playerDeath);
        // GUIManager.Ins.ShowGameOverDialog();

    }

}
