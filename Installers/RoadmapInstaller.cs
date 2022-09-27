public class RoadmapInstaller : Installer
{
    public override void Install(IBinder context)
    {
        //Data
        context.BindInterfaces<RoadmapRewardProviderMock>();
        context.BindInterfaces<RatingProviderMock>();
        context.BindInterfaces<RoadmapModel>();
        context.BindInterfaces<RoadmapRatingProvider>();

    }
}