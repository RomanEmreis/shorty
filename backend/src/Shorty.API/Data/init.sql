-- Postgres init script

-- Create the Todos table
CREATE TABLE IF NOT EXISTS shorty_urls
(
    url text UNIQUE NOT NULL PRIMARY KEY,
    token text UNIQUE NOT NULL
);