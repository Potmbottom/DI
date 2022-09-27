public class LobbyInstaller : Installer
{
    [Inject] private ComponentSizeConfig _componentSize;
    [Inject] private PrefabPath _prefabPath;
    
    public override void Install(IBinder context)
    {
    }
}
