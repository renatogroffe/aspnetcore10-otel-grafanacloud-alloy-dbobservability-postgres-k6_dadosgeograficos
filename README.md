# aspnetcore10-otel-grafanacloud-alloy-dbobservability-postgres_contagemacessos
Exemplo de uso de OpenTelemetry + Grafana Cloud + Alloy (collector) + Tempo (trace) + Loki (logs) + Prometheus (métricas) em uma API REST de dados geográficos baseada em .NET 10 + ASP.NET Core e que utiliza PostgreSQL. Inclui script do Docker Compose para criação de instância do PostgreSQL e do Grafana Alloy (com Database Observability).

## Testes

Visualizando métricas e logs coletados através da funcionalidade **Queries overview (Database Observability)**:

![Queries overview - métricas](img/queries-01.png)

Algumas queries que foram executadas:

![Queries overview - métricas](img/queries-02.png)

Consultando detalhes de uma dessas queries:

![Detalhes de uma query](img/queries-03.png)

A funcionalidade **Query samples**:

![Query samples](img/queries-04.png)

Visualizando detalhes de uma query de exemplo, com o link para o trace correspondente:

![Detalhes de uma query de exemplo](img/queries-05.png)

Visualizando o trace de uma query de exemplo no Grafana Tempo:

![Trace de uma query de exemplo](img/tempo-01.png)


