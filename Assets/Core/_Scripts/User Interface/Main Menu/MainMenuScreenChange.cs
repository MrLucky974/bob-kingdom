namespace LBB.BRA1NFvCK
{
    public class MainMenuScreenChange : UIMenuScreenChange<MainMenuScreenType>
    {
        public override void ChangeScreen()
        {
            base.ChangeScreen();
            SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        }
    }
}
