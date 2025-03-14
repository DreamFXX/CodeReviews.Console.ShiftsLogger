# Introduction

<p>If you’ve been learning to code for more than five minutes, you probably heard about 'APIs'. They’re at the core of back-end programming. In web development, their main job will be to communicate with databases using “endpoints”. From now on you’ll be creating APIs all the time and this acronym will forever be part of your vocabulary.

<p> In the Drinks Info App we have created a program that consumed an external API. This time we will track a worker’s “shifts”. We’ll create an API and the console app that will consume it. When using Microsoft’s Documentation, often times you’ll come across Web APIs being developed with Entity Framework, which acts as layer between the “endpoints” and the actual database.

<p> This stack (Web API/EF) is very common in enterprise applications, but it’s important to know that Web Apis can be developed using ADO.NET and Dapper. These data access solutions have the advantage of giving the developers more control over the SQL queries. In the end, all solutions are valid. Each has pros and cons and it comes down to personal preference. For this project, we will be using Entity Framework, as it takes care the basic CRUD operations we need as beginners.

<p> This project has as a requirement the use of Web APIs testing tools. In development you need a way to quickly test your endpoints without having to create a UI. These days, Swagger is already scaffolded into the .NET Core Web Api project so you can start using it as soon as you run your project. Postman is very complete external tool with an user interface that makes it very easy to store tests for later use, which saves a lot of development time. The learning curve for Postman is slightly steeper, but don’t skip it, it will pay off.</p>

## Requirements

1. This is an application where you should record a worker's shifts.

2. You need to create two applications: the Web API and the UI that will call it.

3. All validation and user input should happen in the UI app.

4. Your API's controller should be lean. Any logic should be handled in a separate "service".

5. You should use SQL Server, not SQLite

6. You should use the "code first" approach to create your database, using Entity Framework's migrations tool.

7. Your front-end project needs to have try-catch blocks around the API calls so it handles unexpected errors (i.e. the API isn't running or returns a 500 error.)