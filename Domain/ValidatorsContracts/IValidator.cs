namespace Domain.ValidatorsContracts
{
    public interface IValidator<in T> where T : class
    {
        public static void Validate(T obj) { }
    }
}