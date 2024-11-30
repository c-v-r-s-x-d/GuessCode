#!/bin/bash
# Ожидание запуска Postgres
until psql -h $SENTRY_DB_HOST -p 1432 -U $SENTRY_DB_USER -c '\q' 2>/dev/null; do
  echo "Waiting for PostgreSQL..."
  sleep 5
done

# Инициализация базы данных
sentry upgrade --noinput

# Создание пользователя и команды
sentry createuser --email admin@guesscode.com --password admin --superuser --no-input || true
sentry init-config

# Создание команды и проекта
sentry organization create guesscode --slug=my-org --default || true
sentry team create guesscode-team --organization guesscode || true
sentry project create guesscode-project --team guesscode-team --platform dotnet || true

# Получение DSN
DSN=$(sentry exec -- python -c "import sentry_sdk; print(sentry_sdk.utils.get_default_dsn())")
echo "SENTRY_DSN=$DSN" > /etc/sentry/dsn.env
echo $DSN

exec sentry run web
