# Acesso

Tecnologias utilizadas

.net core 5.0
rabbitmq
elasticsearch
MongoDb

Patterns
Cqrs
Solid


# como testar?

- no appSettings.json substitua o valor do mongoConnections:connectionString (****)Pela connection string do mongo

## Docker
Executando local com docker-compose

```
docker-compose up --build -d
```

##Criando a imagem

```
dotnet clean

dotnet build

dotnet restore

docker build --rm -t acesso-v1.0 .
```