# 🚀 Projeto C# - API - SolidarMap

![Static Badge](https://img.shields.io/badge/build-passing-brightgreen) ![Static Badge](https://img.shields.io/badge/Version-1.0.0-black) ![License](https://img.shields.io/badge/license-MIT-lightgrey)

## 🧑‍🤝‍🧑 Informações dos Contribuintes

| Nome | Matricula | Turma |
| :------------: | :------------: | :------------: |
| Pedro Herique Vasco Antonieti | 556253 | 2TDSPH |
<p align="right"><a href="#readme-top">Voltar ao topo</a></p>

## 🚩 Características

Este repositório contém a entrega da disciplina **ADVANCED BUSINESS DEVELOPMENT WITH .NET**, com a criação da API Restful para utilização no projeto.
<p align="right"><a href="#readme-top">Voltar ao topo</a></p>

## 🧩 Tecnologias Utilizadas

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Visual Studio 2022](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=Swagger&logoColor=white)
![Oracle](https://img.shields.io/badge/Oracle-F80000?style=for-the-badge&logo=oracle&logoColor=black)

## 📊 Diagrama de Database

![Diagrama MER](https://i.imgur.com/KxO6q6E.png)

## 🧱 Estrutura do Projeto

A arquitetura da aplicação segue uma divisão em camadas:

- **Models**: Representações das entidades do banco de dados.
- **DTOs**: Camada intermediária para entrada/saída de dados.
- **Controllers**: Responsáveis por lidar com as requisições HTTP.
- **Connection (AppDbContext)**: Responsável pela conexão e mapeamento com o banco.

## 📚 Endpoints da API

| Método | Rota                              | Descrição                                                                 |
|--------|-----------------------------------|---------------------------------------------------------------------------|
| GET    | /api/Usuario                      | Lista todos os usuários                                                  |
| GET    | /api/Usuario/{id}                 | Retorna um usuário pelo ID                                               |
| GET    | /api/Usuario/email?email={email}  | Retorna um usuário pelo e-mail                                           |
| POST   | /api/Usuario                      | Cria um novo usuário                                                     |
| PUT    | /api/Usuario/{id}                 | Atualiza os dados de um usuário                                          |
| DELETE | /api/Usuario/{id}                 | Deleta um usuário                                                        |
|        |                                   |                                                                           |
| GET    | /api/Ajuda                        | Lista todas as ajudas                                                    |
| GET    | /api/Ajuda/{id}                   | Retorna uma ajuda específica                                             |
| POST   | /api/Ajuda                        | Cadastra uma nova ajuda                                                  |
| PUT    | /api/Ajuda/{id}                   | Atualiza uma ajuda                                                       |
| DELETE | /api/Ajuda/{id}                   | Remove uma ajuda                                                         |
|        |                                   |                                                                           |
| GET    | /api/Mensagem                     | Lista todas as mensagens                                                 |
| GET    | /api/Mensagem/{id}                | Retorna uma mensagem pelo ID                                             |
| GET    | /api/Mensagem/ajuda/{ajudaId}     | Lista mensagens relacionadas a uma ajuda                                 |
| POST   | /api/Mensagem                     | Envia uma nova mensagem                                                  |
| PUT    | /api/Mensagem/{id}                | Edita uma mensagem existente                                             |
| DELETE | /api/Mensagem/{id}                | Remove uma mensagem                                                      |
|        |                                   |                                                                           |
| GET    | /api/TipoUsuario                  | Lista todos os tipos de usuário                                          |
| GET    | /api/TipoUsuario/{id}             | Retorna um tipo de usuário específico                                    |
| POST   | /api/TipoUsuario                  | Cadastra um novo tipo de usuário                                         |
| PUT    | /api/TipoUsuario/{id}             | Atualiza um tipo de usuário                                              |
| DELETE | /api/TipoUsuario/{id}             | Remove um tipo de usuário                                                |
|        |                                   |                                                                           |
| GET    | /api/TipoRecurso                  | Lista todos os tipos de recurso                                          |
| GET    | /api/TipoRecurso/{id}             | Retorna um tipo de recurso específico                                    |
| POST   | /api/TipoRecurso                  | Cadastra um novo tipo de recurso                                         |
| PUT    | /api/TipoRecurso/{id}             | Atualiza um tipo de recurso                                              |
| DELETE | /api/TipoRecurso/{id}             | Remove um tipo de recurso                                                |
|        |                                   |                                                                           |
| GET    | /api/TipoZona                     | Lista todos os tipos de zona                                             |
| GET    | /api/TipoZona/{id}                | Retorna um tipo de zona específico                                       |
| POST   | /api/TipoZona                     | Cadastra um novo tipo de zona                                            |
| PUT    | /api/TipoZona/{id}                | Atualiza um tipo de zona                                                 |
| DELETE | /api/TipoZona/{id}                | Remove um tipo de zona                                                   |


## 🔁 Padrões e Boas Práticas Aplicadas

- Uso de DTOs para evitar exposição direta das entidades
- Validações com tratamento de erros e códigos HTTP específicos
- Documentação via anotações XML para integração com Swagger
- Injeção de dependência com `AppDbContext` no construtor dos controllers
- Operações assíncronas (`async/await`) para melhorar desempenho

## 📄 Documentação da API

A API foi documentada com o Swagger e contém descrições completas dos endpoints, parâmetros esperados e possíveis respostas HTTP. 

A documentação pode ser acessada em tempo de execução via: https://localhost:{porta}/swagger

## 🧪 Testes Manuais Realizados

Embora testes automatizados não tenham sido implementados nesta fase, os endpoints foram validados manualmente via:

- Swagger UI
- Postman
- Insomnia
