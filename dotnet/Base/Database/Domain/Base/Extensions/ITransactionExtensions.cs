namespace Allors.Database.Domain
{
    using System;
    using Allors.Database.Derivations;
    using Allors.Database.Domain.Derivations;

    public static partial class ITransactionExtensions
    {
        public static ICaches Caches(this ITransaction @this) => @this.Services.Get<ICaches>();

        public static IValidation Derive(this ITransaction @this, bool throwExceptionOnError = true, bool continueOnError = false)
        {
            var derivationFactory = @this.Database.Services.Get<IDerivationService>();
            var derivation = derivationFactory.CreateDerivation(@this, continueOnError);
            var validation = derivation.Derive();
            if (throwExceptionOnError && validation.HasErrors)
            {
                throw new DerivationException(validation);
            }

            return validation;
        }

        public static DateTime Now(this ITransaction @this)
        {
            var now = DateTime.UtcNow;

            var time = @this.Database.Services.Get<ITime>();
            var timeShift = time.Shift;
            if (timeShift != null)
            {
                now = now.Add((TimeSpan)timeShift);
            }

            return now;
        }
    }
}
