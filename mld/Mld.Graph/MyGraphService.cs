using Mld.Domain;
using Neo4j.Driver;

namespace Mld.Graph;

public class MyGraphService
{
    private readonly IAsyncSession _session;
    public MyGraphService(string uri, string user, string password)
    {
        _session = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password)).AsyncSession();
    }

    public async Task CreatePerson(Person person)
    {
        var query = "MERGE (:PERSON {FirstName: $FirstName, LastName: $LastName, Age: $Age, AccountNumber: $AccountNumber})";
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(query, person);
        });
    }

    public async Task CreateEmployment(Person person, string companyName, int salary)
    {
        var query = @"MATCH (p:PERSON{FirstName: $PersonFirstName, LastName: $PersonLastName})
                      MATCH (c:COMPANY{Name: $CompanyName})
                      CREATE (p)-[:EMPLOYMENT{Salary: $Salary}]->(c)";
        var param = new 
        { 
            PersonFirstName = person.FirstName, 
            PersonLastName = person.LastName,
            CompanyName = companyName,
            Salary = salary
        };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(query, param);
        });
    }

    public async Task CreateCompany(string name)
    {
        var query = "MERGE (:COMPANY {Name: $name})";
        var param = new { name = name };
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(query, param);
        });
    }
}