\c dados_geograficos

CREATE EXTENSION IF NOT EXISTS pg_stat_statements;

CREATE USER alloy_o11y WITH PASSWORD 'alloy_o11y_pw';
GRANT CONNECT ON DATABASE dados_geograficos TO alloy_o11y;
GRANT pg_monitor TO alloy_o11y;
GRANT pg_read_all_stats TO alloy_o11y;
GRANT USAGE ON SCHEMA public TO alloy_o11y;
GRANT SELECT ON ALL TABLES IN SCHEMA public TO alloy_o11y;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT ON TABLES TO alloy_o11y;
