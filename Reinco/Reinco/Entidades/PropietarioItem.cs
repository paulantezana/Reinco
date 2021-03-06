﻿using Reinco.Interfaces.Propietario;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class PropietarioItem
    {
        WebService Servicio = new WebService();
        
        public int idPropietario { get; set; }
        public string nombre { get; set; }
        public string fotoPerfil { get; set; }

        public ICommand Eliminar { get; private set; }
        public ICommand editarPropietario { get; private set; }

        public PropietarioItem()
        {
            // Eliminar
            Eliminar = new Command(() =>
            {
                eliminar();
            });

            // Modificar
            editarPropietario = new Command(() =>
            {
                App.ListarPropietarios.Navigation.PushAsync(new AgregarPropietario(this));
            });
        }

        #region ================================ Eliminar Propietario ================================
        private async void eliminar()
        {
            try
            {
                bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar el propietario: " + this.nombre + "?", "Aceptar", "Cancelar");
                if (respuesta)
                {
                    object[,] variables = new object[,] { { "idPropietario", this.idPropietario } };
                    dynamic result = await Servicio.MetodoGetString("ServicioPropietario.asmx", "EliminarPropietario", variables);
                    string Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Eliminar Usuario", Mensaje, "Aceptar");
                        App.ListarPropietarios.PropietarioItems.Clear();
                        App.ListarPropietarios.CargarPropietarioItem(App.nroElementos,100000);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar Propietario", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
            }
        } 
        #endregion

    }
}
