namespace Allors.Database.Domain
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
   
    public partial class Medias
    {
        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().MediaByUniqueId().Merger().Action(); ;

            merge(Media.AvatarId, v =>
            {
                v.InData = this.GetResourceBytes("avatar.png");
                v.InFileName = "avatar.png";
            });

            merge(Media.AboutId, v =>
            {
                v.InData = this.GetResourceBytes("about.md");
                v.InFileName = "about.md";
            });

            merge(Media.MadeliefjeId, v =>
            {
                v.InData = this.GetResourceBytes("madeliefje.jpg");
                v.InFileName = "madeliefje.jpg";
            });
        }

        private byte[] GetResourceBytes(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var manifestResourceName = assembly.GetManifestResourceNames().First(v => v.ToLower().Contains(name.ToLower()));
            var resource = assembly.GetManifestResourceStream(manifestResourceName);
            if (resource != null)
            {
                using (var ms = new MemoryStream())
                {
                    resource.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }
    }
}
