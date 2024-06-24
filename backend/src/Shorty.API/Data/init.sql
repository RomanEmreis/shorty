-- Postgres init script

-- Create the Short URLs table
CREATE TABLE IF NOT EXISTS shorty_urls
(
    token      text UNIQUE NOT NULL PRIMARY KEY,
    url        text        NOT NULL,
    created_at timestamp   NOT NULL
);