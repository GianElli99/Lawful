﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class Tema
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCierre { get; set; }
        public readonly Usuario Owner;
        public List<Iniciativa> Iniciativas { get; set; }
        public bool EveryoneCanEdit { get; set; }
        public Tema()//para listas todos los temas
        {
            Iniciativas = new List<Iniciativa>();
        }
        public Tema(Usuario owner)// para consultar un tema en especial
        {
            Iniciativas = new List<Iniciativa>();
            Owner = owner;
        }
    }
}