// <copyright file="Countries.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;

    public partial class Countries
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, this.M.Currency.Composite);
        }

        protected override void BaseSetup(Setup setup)
        {
            base.BaseSetup(setup);

            var currencyCodeByCountryCode = new Dictionary<string, string>
            {
                { "AF", "AFN" }, { "AL", "ALL" }, { "DZ", "DZD" }, { "AS", "USD" }, { "AD", "EUR" }, { "AO", "AOA" }, { "AI", "XCD" }, { "AG", "XCD" }, { "AR", "ARP" }, { "AM", "AMD" }, { "AW", "AWG" }, { "AU", "AUD" }, { "AT", "EUR" }, { "AZ", "AZN" }, { "BS", "BSD" }, { "BH", "BHD" }, { "BD", "BDT" }, { "BB", "BBD" }, { "BY", "BYR" }, { "BE", "EUR" }, { "BZ", "BZD" }, { "BJ", "XOF" }, { "BM", "BMD" }, { "BT", "BTN" }, { "BO", "BOV" }, { "BA", "BAM" }, { "BW", "BWP" }, { "BV", "NOK" }, { "BR", "BRL" }, { "IO", "USD" }, { "BN", "BND" }, { "BG", "BGL" }, { "BF", "XOF" }, { "BI", "BIF" }, { "KH", "KHR" }, { "CM", "XAF" }, { "CA", "CAD" }, { "CV", "CVE" }, { "KY", "KYD" }, { "CF", "XAF" }, { "TD", "XAF" }, { "CL", "CLF" }, { "CN", "CNY" }, { "CX", "AUD" }, { "CC", "AUD" }, { "CO", "COU" }, { "KM", "KMF" }, { "CG", "XAF" }, { "CD", "CDF" }, { "CK", "NZD" }, { "CR", "CRC" }, { "HR", "HRK" }, { "CU", "CUP" }, { "CY", "EUR" }, { "CZ", "CZK" }, { "CI", "XOF" }, { "DK", "DKK" }, { "DJ", "DJF" }, { "DM", "XCD" }, { "DO", "DOP" }, { "EC", "USD" }, { "EG", "EGP" }, { "SV", "USD" }, { "GQ", "EQE" }, { "ER", "ERN" }, { "EE", "EEK" }, { "ET", "ETB" }, { "FK", "FKP" }, { "FO", "DKK" }, { "FJ", "FJD" }, { "FI", "FIM" }, { "FR", "XFO" }, { "GF", "EUR" }, { "PF", "XPF" }, { "TF", "EUR" }, { "GA", "XAF" }, { "GM", "GMD" }, { "GE", "GEL" }, { "DE", "EUR" }, { "GH", "GHC" }, { "GI", "GIP" }, { "GR", "GRD" }, { "GL", "DKK" }, { "GD", "XCD" }, { "GP", "EUR" }, { "GU", "USD" }, { "GT", "GTQ" }, { "GN", "GNE" }, { "GW", "GWP" }, { "GY", "GYD" }, { "HT", "USD" }, { "HM", "AUD" }, { "VA", "EUR" }, { "HN", "HNL" }, { "HK", "HKD" }, { "HU", "HUF" }, { "IS", "ISJ" }, { "IN", "INR" }, { "ID", "IDR" }, { "IR", "IRR" }, { "IQ", "IQD" }, { "IE", "IEP" }, { "IL", "ILS" }, { "IT", "ITL" }, { "JM", "JMD" }, { "JP", "JPY" }, { "JO", "JOD" }, { "KZ", "KZT" }, { "KE", "KES" }, { "KI", "AUD" }, { "KP", "KPW" }, { "KR", "KRW" }, { "KW", "KWD" }, { "KG", "KGS" }, { "LA", "LAJ" }, { "LV", "LVL" }, { "LB", "LBP" }, { "LS", "ZAR" }, { "LR", "LRD" }, { "LY", "LYD" }, { "LI", "CHF" }, { "LT", "LTL" }, { "LU", "LUF" }, { "MO", "MOP" }, { "MK", "MKN" }, { "MG", "MGF" }, { "MW", "MWK" }, { "MY", "MYR" }, { "MV", "MVR" }, { "ML", "MAF" }, { "MT", "MTL" }, { "MH", "USD" }, { "MQ", "EUR" }, { "MR", "MRO" }, { "MU", "MUR" }, { "YT", "EUR" }, { "MX", "MXV" }, { "FM", "USD" }, { "MD", "MDL" }, { "MC", "MCF" }, { "MN", "MNT" }, { "ME", "EUR" }, { "MS", "XCD" }, { "MA", "MAD" }, { "MZ", "MZM" }, { "MM", "MMK" }, { "NA", "ZAR" }, { "NR", "AUD" }, { "NP", "NPR" }, { "NL", "EUR" }, { "NC", "XPF" }, { "NZ", "NZD" }, { "NI", "NIO" }, { "NE", "XOF" }, { "NG", "NGN" }, { "NU", "NZD" }, { "NF", "AUD" }, { "MP", "USD" }, { "NO", "NOK" }, { "OM", "OMR" }, { "PK", "PKR" }, { "PW", "USD" }, { "PA", "USD" }, { "PG", "PGK" }, { "PY", "PYG" }, { "PE", "PEH" }, { "PH", "PHP" }, { "PN", "NZD" }, { "PL", "PLN" }, { "PT", "TPE" }, { "PR", "USD" }, { "QA", "QAR" }, { "RO", "ROK" }, { "RU", "RUB" }, { "RW", "RWF" }, { "RE", "EUR" }, { "SH", "SHP" }, { "KN", "XCD" }, { "LC", "XCD" }, { "PM", "EUR" }, { "VC", "XCD" }, { "WS", "WST" }, { "SM", "EUR" }, { "ST", "STD" }, { "SA", "SAR" }, { "SN", "XOF" }, { "RS", "CSD" }, { "SC", "SCR" }, { "SL", "SLL" }, { "SG", "SGD" }, { "SK", "SKK" }, { "SI", "SIT" }, { "SB", "SBD" }, { "SO", "SOS" }, { "ZA", "ZAL" }, { "ES", "ESB" }, { "LK", "LKR" }, { "SD", "SDG" }, { "SR", "SRG" }, { "SJ", "NOK" }, { "SZ", "SZL" }, { "SE", "SEK" }, { "CH", "CHW" }, { "SY", "SYP" }, { "TW", "TWD" }, { "TJ", "TJR" }, { "TZ", "TZS" }, { "TH", "THB" }, { "TL", "USD" }, { "TG", "XOF" }, { "TK", "NZD" }, { "TO", "TOP" }, { "TT", "TTD" }, { "TN", "TND" }, { "TR", "TRL" }, { "TM", "TMM" }, { "TC", "USD" }, { "TV", "AUD" }, { "UG", "UGS" }, { "UA", "UAK" }, { "AE", "AED" }, { "GB", "GBP" }, { "US", "USS" }, { "UM", "USD" }, { "UY", "UYI" }, { "UZ", "UZS" }, { "VU", "VUV" }, { "VE", "VEB" }, { "VN", "VNC" }, { "VG", "USD" }, { "VI", "USD" }, { "WF", "XPF" }, { "EH", "MAD" }, { "YE", "YER" }, { "ZM", "ZMK" }, { "ZW", "ZWC" },
            };

            var countryByKey = this.Transaction.Scoped<CountryByKey>();
            var currencyByKey = this.Transaction.Scoped<CurrencyByKey>();
            foreach (var entry in currencyCodeByCountryCode)
            {
                var country = countryByKey[entry.Key];
                country.Currency = currencyByKey[entry.Value];
            }
        }
    }
}
