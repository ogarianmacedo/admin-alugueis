# admin-alugueis

Projeto de estudo - área administrativa com ASP.NET CORE 2.1

#### Instruções
    1- Configuração de banco de dados em appsettings.json
    2- Executar para gerar os Migrations: 
        dotnet ef migrations add InitialCreate --project ProjetoAlugar
    3- Executar para atualizar o banco de dados:
        dotnet ef database update --project ProjetoAlugar
    4- Executar no banco de dados: 
        INSERT INTO NiveisAcessos (Id, Name, NormalizedName, Descricao) 
        VALUES (1, 'Administrador', 'ADMINISTRADOR', 'Administrador...')
    4- Executar: F5 ou ctrl + F5