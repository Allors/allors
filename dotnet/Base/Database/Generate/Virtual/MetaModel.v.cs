namespace Allors.Meta.Generation.Model;

public partial class MetaModel
{
    public void Init()
    {
        this.CoreInit();
        this.BaseInit();
        this.CustomInit();
    }
}
