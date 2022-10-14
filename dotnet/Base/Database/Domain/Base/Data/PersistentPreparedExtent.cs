// <copyright file="PersistentPreparedExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Allors.Protocol.Json.SystemText;
    using Allors.Database.Data;
    using Allors.Database.Protocol.Json;
    using Extent = Allors.Protocol.Json.Data.Extent;

    public partial class PersistentPreparedExtent
    {
        public IExtent Extent
        {
            get
            {
                using TextReader reader = new StringReader(this.Content);
                var protocolExtent = (Extent)XmlSerializer.Deserialize(reader);
                return protocolExtent.FromJson(this.Strategy.Transaction, new UnitConvert());
            }

            set
            {
                var stringBuilder = new StringBuilder();
                using TextWriter writer = new StringWriter(stringBuilder);
                XmlSerializer.Serialize(writer, value.ToJson(new UnitConvert()));
                this.Content = stringBuilder.ToString();
            }
        }

        private static XmlSerializer XmlSerializer => new XmlSerializer(typeof(Extent));
    }
}
