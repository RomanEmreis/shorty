-- Postgres init script

-- Create the Short URLs table
CREATE TABLE IF NOT EXISTS shorty_urls
(
    token      text UNIQUE NOT NULL PRIMARY KEY,
    url        text UNIQUE NOT NULL,
    created_at timestamp   NOT NULL
);

-- Create the additional index for url field
CREATE INDEX shorty_urls_url_index ON shorty_urls (url);