﻿using Nest;

namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class ElasticSearchInsertManyModel : ElasticSearchModel
{
    public object[] Items { get; set; }

    public ElasticSearchInsertManyModel(object[] items)
    {
        Items = items;
    }

    public ElasticSearchInsertManyModel(Id elasticId, string indexName, object[] items)
        : base(elasticId, indexName)
    {
        Items = items;
    }
}
