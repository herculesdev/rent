
# RentSystem
Uma aplicação de locação de motos

## 🚀 Começando
Essas instruções permitirão que você obtenha uma cópia do projeto em operação na sua máquina local para fins de desenvolvimento e teste.

### 📋 Pré-requisitos
* [Docker & Docker Compose](https://docs.docker.com/get-docker/)
* [Visual Studio](https://visualstudio.microsoft.com/pt-br/launch/)
* [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
* [Git](https://git-scm.com/downloads)

### 🔧 Compilando
Abra um terminal e clone este repositório em qualquer diretório da sua máquina utilizando o comando:
```
git clone https://github.com/herculesdev/rent.git
```

Acesse a pasta com:
```
cd rent
```
Restaure as dependências:
```
dotnet restore
```

Compile utilizando:
```
dotnet build
```

Rode os testes de unidade (caso queira):
```
dotnet test
```
### 🐋 Serviços de infra com o Docker & Docker Compose (Rabbit, Postgres e Mongo)
Abra o terminal, acesse o pasta raiz do repositório que clonou na etapa anterior e execute o seguinte comando

```
docker compose up -d
```
**OBS 1:** use `docker compose down` para encerrar os serviços de infra.

**OBS 2:** os containers não guardam dados, sempre que reiniciados iniciam-se do zero

### 💻 Rodando os multiplos projetos
Abra 3 instâncias de um terminal (abas ou janelas) e em cada um deles, entre na pasta raiz do projeto (repositório que você clonou na etapa anterior)

#### 1. Rent.Backoffice.Api
```
dotnet run --project Rent.Backoffice.Api
```
Um resultado parecido com este será exibido
```
[INF] Now listening on: "http://localhost:5140"
[INF] Application started. Press Ctrl+C to shut down.
[INF] Hosting environment: "Development"
```

Após isto, a API estará em funcionamento. Acesse http://localhost:5140/swagger para visualizar a documentação dos endpoints


#### 1. Rent.Renter.Api
```
dotnet run --project Rent.Renter.Api
```
Um resultado parecido com este será exibido
```
[INF] Now listening on: "http://localhost:5007"
[INF] Application started. Press Ctrl+C to shut down.
[INF] Hosting environment: "Development"
```

Após isto, a API estará em funcionamento. Acesse http://localhost:5007/swagger para visualizar a documentação dos endpoints

## 🛠️ Construído com
Ferramentas/tecnologias utilizadas para construção deste projeto

* [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) - Backend
* [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core/) - Mapeamento Objeto-Relacional
* [Fluent Validation](https://github.com/FluentValidation/FluentValidation) - Validações de domínio
* [NSubstitute](https://github.com/nsubstitute/NSubstitute) - Mockagem de dependencias nos testes de unidade
* [Serilog](https://serilog.net/) -  Log estruturado
* [Visual Studio Code](https://code.visualstudio.com/) - Editor de Código
* [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/launch/) - IDE C# / .NET
* [Swagger](https://swagger.io/) - Documentação e teste da API
* [RabbitMQ](https://www.rabbitmq.com/) - Mensageria
* [MongoDB](https://www.mongodb.com/) - Banco de dados não relacional
* [PostgreSQL](https://www.postgresql.org/) - Banco de dados relacional

## ☑️ O que eu adicionaria se tivesse mais tempo
* AutoMapper para mapeamento automático entre os commands, queries, responses e models
* Migrations para versionamento do banco de dados
* Camada extra "Application" para conter ViewModels, Services e o que mais fosse necessário para coordenar as chamadas ao domínio
* Promoveria alguns dados primitivos para ValueObject afim de melhorar a expressividade do modelo e isolar suas validações (CPF, RA, Email...)
* Extrair o acesso a dados da camada de infraestrutura
* Implementar o Unit Of Work para controle transacional
* Tornar os endpoints e algumas operações assíncronas
* Aumentar a cobertura dos testes (extendo-os aos entities, commands e queries e até mesmo aos repositories com banco de dados In-Memory

Obs: a princípio o projeto utilizaria o MySQL mas devido diversos problemas por eu ter utilizado o .NET 6 Preview, me vi obrigado a optar pelo SQLite, porém numa ocasião normal em que tivesse utilizando um produto estável isso não ocorreria. Tenho outro projeto utilizado o EF + MySQL que pode ser conferido [aqui](https://github.com/herculesdev/covid-app)

## 📚 Arquitetura
O arquitetura foi baseada em parte na Onion Architecture respeitando o princípio de que a modelagem é voltada ao domínio, dessa forma o mesmo é independente e não faz referência a recursos externos, muito pelo contrário, os dependências são invertidas e sempre partem da borda em direção ao centro (domínio).

![Onion Architecture](https://camo.githubusercontent.com/07832a2276c948e197784ba3d53a91b70da3906520b61e7488f70e0f9a6e9ddc/68747470733a2f2f7465616d736d696c65792e6769746875622e696f2f6173736574732f636c65616e2d6172636869746563747572652d646f746e65742e706e67)

Em conjunto também foi empregado o CQRS (Command Query Responsibility Segregation) para separar as operações de leitura e escrita na aplicação.

![enter image description here](https://miro.medium.com/max/1200/1*Fo70HYchxk2q2uEiHoV6Cw.png)
