﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pdf : ContentPage
    {
        public Pdf()
        {
            InitializeComponent();

        }
       
        //public string Uri
        //{
        //    get { return (string)GetValue(UriProperty); }
        //    set { SetValue(UriProperty, value); }
        //}
    }
}
