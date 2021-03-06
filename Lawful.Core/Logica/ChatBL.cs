﻿using Lawful.Core.Datos.Interfaces;
using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Logica
{
    public class ChatBL
    {
        private IMensajeDAO mensajeDAO;
        public ChatBL()
        {
            mensajeDAO = new Datos.DAO.MensajeDAO_SqlServer();
        }
        public List<Mensaje> ObtenerChat(int userID1, int userID2)
        {
            try
            {
                var retorno = mensajeDAO.ObtenerMensajesUsuarios(userID1, userID2);
                if (retorno == null)
                {
                    return new List<Mensaje>();
                }
                else
                {
                    return retorno;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Mensaje> ObtenerChat(int temaID)
        {
            try
            {
                var retorno = mensajeDAO.ObtenerMensajesTema(temaID);
                if (retorno == null)
                {
                    return new List<Mensaje>();
                }
                else
                {
                    return retorno;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void EnviarMensaje(Mensaje mensaje)
        {
            try
            {
                if (mensaje.GetType().Name == "MensajeATema")
                {
                    mensajeDAO.EnviarMensajeTema((MensajeATema)mensaje);
                }
                else
                {
                    mensajeDAO.EnviarMensajeUsuario((MensajeAUsuario)mensaje);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
