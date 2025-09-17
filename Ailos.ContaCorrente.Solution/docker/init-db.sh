#!/bin/bash
set -e

# Create database directory if it doesn't exist
mkdir -p /data

# Initialize SQLite database
sqlite3 /data/ailos.db < /docker-entrypoint-initdb.d/init.sql

echo "SQLite database initialized successfully"
