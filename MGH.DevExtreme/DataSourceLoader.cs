using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExpressDataSourceLoader = DevExtreme.AspNet.Data.DataSourceLoader;

namespace MGH.DevExtreme
{
    public static class DataSourceLoader
    {
        public static LoadResult Load<T>(IEnumerable<T> source, DataSourceLoadOptionsBase options)
        {
            CheckMaximumTake(options);

            return DevExpressDataSourceLoader.Load(source, options);
        }
        public static LoadResult Load<T>(IQueryable<T> source, DataSourceLoadOptionsBase options)
        {
            CheckMaximumTake(options);

            return DevExpressDataSourceLoader.Load(source, options);
        }
        public static Task<LoadResult> LoadAsync<T>(IQueryable<T> source, DataSourceLoadOptionsBase options, CancellationToken cancellationToken = default)
        {
            CheckMaximumTake(options);

            return DevExpressDataSourceLoader.LoadAsync(source, options);
        }

        public static int MaximumTake = 100;

        private static void CheckMaximumTake(DataSourceLoadOptionsBase options)
        {
            if (options.Take > MaximumTake)
                options.Take = MaximumTake;
        }
    }
}
