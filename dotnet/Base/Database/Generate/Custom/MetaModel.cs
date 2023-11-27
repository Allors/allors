namespace Allors.Meta.Generation.Model;

public partial class MetaModel
{
    public void CustomInit()
    {
        var gender = this.MetaPopulation.Gender;
        var key = gender.UniqueId;
    }
}
