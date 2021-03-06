﻿using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Lawful.Core.Datos.DAO
{
    public class TemaDAO_SqlServer : ConexionDB, Interfaces.ITemaDAO
    {
        public Tema Consultar(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Consulta Tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT id, descripcion, estado, fecha_creacion, fecha_cierre, everyone_can_edit,titulo, temas.usuario_id FROM temas WHERE id = {id}; SELECT usuarios.id, nombre, apellido FROM usuarios INNER JOIN usuarios_temas ON usuarios.id = usuarios_temas.usuario_id WHERE tema_id = {id};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            var tema = new Tema(new Usuario() { ID = response.GetInt32(7) });
                            var usuarios = new List<Usuario>();
                            tema.Usuarios = usuarios;
                            tema.ID = response.GetInt32(0);
                            tema.Descripcion = response.GetString(1);
                            tema.Estado = response.GetBoolean(2);
                            tema.FechaCreacion = response.GetDateTime(3);
                            tema.FechaCierre = response.GetDateTime(4);
                            tema.EveryoneCanEdit = response.GetBoolean(5);
                            tema.Titulo = response.GetString(6);

                            response.NextResult();
                            while (response.Read())
                            {
                                var user = new Usuario
                                {
                                    ID = response.GetInt32(0),
                                    Nombre = response.GetString(1),
                                    Apellido = response.GetString(2)
                                };
                                tema.Usuarios.Add(user);
                            }

                            return tema;
                        }
                    }
                    throw new Exception("No se ha podido encontrar el tema");
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Eliminar(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Eliminar tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE temas set estado=0 WHERE id = {id}";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    return;
        }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Insertar(Tema tema)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insertar Tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO temas VALUES (@descripcion, @estado, @fecha_creacion, @fecha_cierre, @everyone_can_edit, @titulo, {tema.Owner.ID});SELECT CAST(scope_identity() AS int);";
                    command.Parameters.AddWithValue("@descripcion", tema.Descripcion);
                    command.Parameters.AddWithValue("@estado", 1);
                    command.Parameters.AddWithValue("@fecha_creacion", tema.FechaCreacion);
                    command.Parameters.AddWithValue("@fecha_cierre", tema.FechaCierre);
                    command.Parameters.AddWithValue("@everyone_can_edit", tema.EveryoneCanEdit);
                    command.Parameters.AddWithValue("@titulo", tema.Titulo);
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            tema.ID = response.GetInt32(0);
                        }
                    }

                    if (tema.Usuarios.Count>0)
                    {
                        string accionesQuery = "INSERT INTO usuarios_temas VALUES";
                        foreach (var usuario in tema.Usuarios)
                        {
                            accionesQuery += $"({usuario.ID}, {tema.ID}),";
                        }
                        accionesQuery = accionesQuery.Remove(accionesQuery.Length - 1);
                        accionesQuery += ";";
                        command.CommandText = accionesQuery;
                        command.ExecuteNonQuery();
                    }
                    
                    
                    transaction.Commit();
                    return;
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<Tema> Listar()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar temas");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT temas.id, descripcion, estado, fecha_creacion, fecha_cierre, everyone_can_edit, titulo FROM temas WHERE estado = 1";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var temas = new List<Tema>();
                            while (response.Read())
                            {
                                var tema = new Tema()
                                {
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1),
                                    Estado = response.GetBoolean(2),
                                    FechaCreacion = response.GetDateTime(3),
                                    FechaCierre = response.GetDateTime(4),
                                    EveryoneCanEdit = response.GetBoolean(5),
                                    Titulo = response.GetString(6)
                                };

                                temas.Add(tema);
                            }
                            return temas;
                        }
                        return null;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<Tema> ListarPorUsuario(int userId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar temas");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT temas.id, descripcion, estado, fecha_creacion, fecha_cierre, everyone_can_edit, titulo, temas.usuario_id FROM temas INNER JOIN usuarios_temas ON temas.id = usuarios_temas.tema_id WHERE estado = 1 AND usuarios_temas.usuario_id = {userId}";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var temas = new List<Tema>();
                            while (response.Read())
                            {
                                var tema = new Tema( new Usuario() { ID = response.GetInt32(7) })
                                {
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1),
                                    Estado = response.GetBoolean(2),
                                    FechaCreacion = response.GetDateTime(3),
                                    FechaCierre = response.GetDateTime(4),
                                    EveryoneCanEdit = response.GetBoolean(5),
                                    Titulo = response.GetString(6)
                                };

                                temas.Add(tema);
                            }
                            return temas;
                        }
                        return null;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Modificar(Tema tema)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Modificar Tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {

                    command.CommandText = $"UPDATE temas SET descripcion=@descripcion, estado=@estado, fecha_creacion=@fecha_creacion, fecha_cierre=@fecha_cierre, everyone_can_edit=@everyone_can_edit, titulo=@titulo WHERE id = {tema.ID};";
                    command.Parameters.AddWithValue("@descripcion", tema.Descripcion);
                    command.Parameters.AddWithValue("@estado", tema.Estado);
                    command.Parameters.AddWithValue("@fecha_creacion", tema.FechaCreacion);
                    command.Parameters.AddWithValue("@fecha_cierre", tema.FechaCierre);
                    command.Parameters.AddWithValue("@everyone_can_edit", tema.EveryoneCanEdit);
                    command.Parameters.AddWithValue("@titulo", tema.Titulo);
                  
                    command.CommandText += $"DELETE FROM usuarios_temas WHERE tema_id = {tema.ID};";
                    string usuariosQuery = "";
                    foreach (var usuario in tema.Usuarios)
                    {
                        usuariosQuery += $"INSERT INTO usuarios_temas VALUES({usuario.ID},{tema.ID});";
                    }
                    command.CommandText += usuariosQuery;
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    return;
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }
        public List<ParticipacionInforme> ObtenerParticipacionTemas()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("obtener participacion");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "select count(distinct iniciativas.id) as iniciativas,temas.titulo, count(distinct usuarios_temas.usuario_id) as usuarios, count(distinct votos.id) as votos from iniciativas"
                                           + " inner join usuarios_temas on usuarios_temas.tema_id = iniciativas.tema_id"
                                            + " inner join temas on temas.id = iniciativas.tema_id"
                                            + " inner join opciones on opciones.iniciativa_id = iniciativas.id"
                                            + " left join votos on votos.opcion_id = opciones.id"
                                            + " where iniciativa_tipo_id not between 4 and 5 and temas.estado != 0"
                                            + " group by iniciativas.tema_id, temas.titulo";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        List<ParticipacionInforme> participacion = new List<ParticipacionInforme>();
                        while (response.Read())
                        {
                            var part = new ParticipacionInforme();
                            part.Iniciativas = response.GetInt32(0);
                            part.Tema = response.GetString(1);
                            part.Usuarios = response.GetInt32(2);
                            part.Votos = response.GetInt32(3);
                            participacion.Add(part);
                        }
                        return participacion;
                    }
                    throw new Exception("No se ha podido encontrar resultados");
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }
    }
}
