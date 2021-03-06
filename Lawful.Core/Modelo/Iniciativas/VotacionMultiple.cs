﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class VotacionMultiple : Iniciativa
    {
        public int MaxOpcionesSeleccionables { get; set; }
        public List<Opcion> Opciones { get; set; }
        public VotacionMultiple(Usuario owner)
        : base(owner)
        {
            Opciones = new List<Opcion>();
        }
        public override string GetIniciativaType()
        {
            return "Votación múltiple";
        }
        public override bool UserHasVoted(int userId)
        {
            foreach (var item in Opciones)
            {
                if (item.Votantes.FindIndex(x => x.ID == userId) != -1)
                {
                    return true;
                }
            }
            return false;
        }
        public override List<Opcion> OptionsVoted(int userId)
        {
            var opciones = new List<Opcion>();
            foreach (var item in Opciones)
            {
                if (item.Votantes.FindIndex(x => x.ID == userId) != -1)
                {
                    opciones.Add(item);
                }
            }
            return opciones;
        }
    }
}
