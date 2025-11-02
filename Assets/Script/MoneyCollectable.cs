public class MoneyCollectable : Collectable
{
    public override void Trigger()
    {
        GameManager.Ins.player.currentCoin += bonus;
        GUIManager.Ins.UpdateCoinCounting((int)GameManager.Ins.player.currentCoin);
        AudioController.Ins.PlaySound(AudioController.Ins.coinPickup);
    }
}
