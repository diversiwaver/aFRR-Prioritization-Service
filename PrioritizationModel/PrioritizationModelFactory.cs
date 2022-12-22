namespace PrioritizationModel;

public static class PrioritizationModelFactory
{
    public static IPrioritizationModel GetPrioritizationModel(string version)
    {
        switch(version)
        {
            case "v1": return new AssetPrioritizationModel();
            default:
                throw new ArgumentException($"Unknown prioritization model version {version}");
        }
    }

}
