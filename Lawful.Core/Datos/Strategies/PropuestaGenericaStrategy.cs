﻿using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Lawful.Core.Modelo.Iniciativas;
using System.Text;

namespace Lawful.Core.Datos.Strategies
{
    public class PropuestaGenericaStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            PropuestaGenerica propuestaGenerica = (PropuestaGenerica)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo, descripcion, fecha_creacion, icon_name, everyone_can_edit, usuario_id, iniciativa_tipo_id, tema_id)" +
                " VALUES " +
                "(@titulo, @descripcion, @fecha_creacion, @icon_name, @everyone_can_edit, @usuario_id, @iniciativa_tipo_id, @tema_id);";

            command.Parameters.AddWithValue("@titulo", propuestaGenerica.Titulo);
            command.Parameters.AddWithValue("@descripcion", propuestaGenerica.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", propuestaGenerica.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", propuestaGenerica.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", propuestaGenerica.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", propuestaGenerica.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", propuestaGenerica.Tema.ID);


            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            PropuestaGenerica propuestaGenerica = (PropuestaGenerica)iniciativa;
            command.CommandText = "UPDATE iniciativas " +
                "titulo=@titulo, " +
                "descripcion=@descripcion, " +
                "fecha_creacion=@fecha_creacion, " +
                "icon_name=@icon_name, " +
                "everyone_can_edit=@everyone_can_edit, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "tema_id=@tema_id " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", propuestaGenerica.Titulo);
            command.Parameters.AddWithValue("@descripcion", propuestaGenerica.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", propuestaGenerica.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", propuestaGenerica.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", propuestaGenerica.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", propuestaGenerica.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", propuestaGenerica.Tema.ID);

            return command;
        }
    }
}
