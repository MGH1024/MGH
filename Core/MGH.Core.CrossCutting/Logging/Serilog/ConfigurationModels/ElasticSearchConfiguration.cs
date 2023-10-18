﻿namespace MGH.Core.CrossCutting.Logging.Serilog.ConfigurationModels;

public class ElasticSearchConfiguration
{
    public string ConnectionString { get; set; }

    public ElasticSearchConfiguration()
    {
        ConnectionString = string.Empty;
    }
}
