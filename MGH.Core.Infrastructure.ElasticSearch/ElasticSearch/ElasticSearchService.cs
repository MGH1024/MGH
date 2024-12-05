using Elasticsearch.Net;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using Nest;

namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;

public class ElasticSearchService(ElasticClient elasticClient) : IElasticSearch
{
    public IReadOnlyDictionary<IndexName, IndexState> GetIndexList()
    {
        return elasticClient.Indices.Get(new GetIndexRequest(Indices.All)).Indices;
    }

    public async Task<IElasticSearchResult> CreateNewIndexAsync(IndexModel indexModel)
    {
        if ((await elasticClient.Indices.ExistsAsync(indexModel.IndexName)).Exists)
            return new ElasticSearchResult(success: false, message: "Index already exists");

        var response = await elasticClient.Indices.CreateAsync(
            indexModel.IndexName,
            selector: se =>
                se.Settings(a => a.NumberOfReplicas(indexModel.NumberOfReplicas)
                        .NumberOfShards(indexModel.NumberOfShards))
                    .Aliases(x => x.Alias(indexModel.AliasName))
        );

        return new ElasticSearchResult(response.IsValid,
            message: response.IsValid ? "Success" : response.ServerError.Error.Reason);
    }

    public async Task<IElasticSearchResult> DeleteByElasticIdAsync(ElasticSearchModel model)
    {
        var response =
            await elasticClient.DeleteAsync<object>(model.ElasticId, selector: x => x.Index(model.IndexName));
        return new ElasticSearchResult(response.IsValid,
            message: response.IsValid ? "Success" : response.ServerError.Error.Reason);
    }

    public async Task<List<ElasticSearchGetModel<T>>> GetAllSearch<T>(SearchParameters parameters)
        where T : class
    {
        Type type = typeof(T);
        ISearchResponse<T> searchResponse = await elasticClient.SearchAsync<T>(
            s => s.Index(Indices.Index(parameters.IndexName))
                .From(parameters.From).Size(parameters.Size)
        );

        var list = searchResponse.Hits.Select(x => new ElasticSearchGetModel<T>
            {
                ElasticId = x.Id,
                Item = x.Source
            })
            .ToList();

        return list;
    }

    public async Task<List<ElasticSearchGetModel<T>>> GetSearchByField<T>(SearchByFieldParameters
        fieldParameters) where T : class
    {
        var searchResponse = await elasticClient.SearchAsync<T>(
            s => s.Index(fieldParameters.IndexName).From(fieldParameters.From).Size(fieldParameters.Size)
        );

        var list = searchResponse.Hits.Select(x =>
                new ElasticSearchGetModel<T>
                {
                    ElasticId = x.Id,
                    Item = x.Source
                })
            .ToList();

        return list;
    }

    public async Task<List<ElasticSearchGetModel<T>>> GetSearchBySimpleQueryString<T>(
        SearchByQueryParameters queryParameters)
        where T : class
    {
        var searchResponse = await elasticClient.SearchAsync<T>(
            s =>
                s.Index(queryParameters.IndexName)
                    .From(queryParameters.From)
                    .Size(queryParameters.Size)
                    .MatchAll()
                    .Query(
                        a =>
                            a.SimpleQueryString(
                                c =>
                                    c.Name(queryParameters.QueryName)
                                        .Boost(1.1)
                                        .Fields(queryParameters.Fields)
                                        .Query(queryParameters.Query)
                                        .Analyzer("standard")
                                        .DefaultOperator(Operator.Or)
                                        .Flags(SimpleQueryStringFlags.And | SimpleQueryStringFlags.Near)
                                        .Lenient()
                                        .AnalyzeWildcard(false)
                                        .MinimumShouldMatch("30%")
                                        .FuzzyPrefixLength(0)
                                        .FuzzyMaxExpansions(50)
                                        .FuzzyTranspositions()
                                        .AutoGenerateSynonymsPhraseQuery(false)
                            )
                    )
        );

        var list = searchResponse.Hits.Select(x => new ElasticSearchGetModel<T> { ElasticId = x.Id, Item = x.Source })
            .ToList();

        return list;
    }

    public async Task<IElasticSearchResult> InsertAsync(ElasticSearchInsertUpdateModel model)
    {
        var response = await elasticClient.IndexAsync(
            model.Item,
            selector: i => i.Index(model.IndexName).Id(model.ElasticId).Refresh(Refresh.True)
        );

        return new ElasticSearchResult(response.IsValid,
            message: response.IsValid ? "Success" : response.ServerError.Error.Reason);
    }

    public async Task<IElasticSearchResult> InsertManyAsync(string indexName, object[] items)
    {
        var response = await elasticClient
            .BulkAsync(a => a.Index(indexName).IndexMany(items));

        return new ElasticSearchResult();
    }

    public async Task<IElasticSearchResult> UpdateByElasticIdAsync(ElasticSearchInsertUpdateModel
        model)
    {
        var response = await elasticClient.UpdateAsync<object>(
            model.ElasticId,
            selector: u => u.Index(model.IndexName).Doc(model.Item)
        );
        return new ElasticSearchResult(response.IsValid,
            message: response.IsValid ? "Success" : response.ServerError.Error.Reason);
    }
}