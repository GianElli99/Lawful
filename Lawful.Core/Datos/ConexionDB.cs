﻿using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos
{
    public class ConexionDB
    {
        protected SqlConnection Conexion = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Lawful;Integrated Security=SSPI;");
    }
}
