using UnityEngine;

public class HealthCollectable : Collectable
{
    public override void Trigger()
    {
        if(player != null)
        {
            player.currentHP += bonus;

            GUIManager.Ins.UpdateHpInfo(player.currentHP,player.maxHP);

            AudioController.Ins.PlaySound(AudioController.Ins.healthPickup);
        }
    }
}
