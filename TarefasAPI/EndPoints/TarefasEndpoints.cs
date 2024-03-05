using Dapper.Contrib.Extensions;
using TarefasAPI.Data;
using static TarefasAPI.Data.TarefaContext;

namespace TarefasAPI.EndPoints;

public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => $"Bem vindo a Api de Tarefas, {DateTime.Now}");

        app.MapGet("/tarefas", async (GetConnection connectionGeter) =>
        {
            using var con = await connectionGeter();
            var tarefas = con.GetAll<Tarefa>().ToList();
            return Results.Ok(tarefas);
        });

        app.MapGet("/tarefas/{id}", async (GetConnection connectionGeter, int id ) =>
        {
            using var con = await connectionGeter();
            var tarefa = con.Get<Tarefa>(id);
            return Results.Ok(tarefa);
        });

        app.MapPost("/tarefas", async (GetConnection connectionGeter, Tarefa tarefa) =>
        {
            using var con = await connectionGeter();
            var id = con.Insert(tarefa);
            return Results.Created($"/tarefas/{id}",tarefa);
        });

        app.MapPut("/tarefas", async (GetConnection connectionGeter, Tarefa tarefa) =>
        {
            using var con = await connectionGeter();
            var id = con.Update(tarefa);
            return Results.Ok();
        });

        app.MapDelete("/tarefas/{id}", async (GetConnection connectionGeter, int id) =>
        {
            using var con = await connectionGeter();

            var deleted = con.Get<Tarefa>(id);

            if (deleted is null)
                return Results.NotFound();

            con.Delete(deleted);
            return Results.Ok();
        });
    }
}
