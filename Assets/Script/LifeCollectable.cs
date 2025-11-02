using UnityEngine;

public class LifeCollectable : Collectable
{
    public override void Trigger()
    {
        GameManager.Ins.player.currentLife = Mathf.Clamp(GameManager.Ins.player.currentLife + bonus,0,5);

        GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.player.currentLife);

        AudioController.Ins.PlaySound(AudioController.Ins.lifePickup);
    }
}
