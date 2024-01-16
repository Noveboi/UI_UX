﻿using Caliburn.Micro;
using Ergasia_Final.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ergasia_Final
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
            
        }
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void Configure()
        {
            _container
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IWindowManager, WindowManager>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.ToString().EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModel => _container.RegisterPerRequest(viewModel, viewModel.ToString(), viewModel));
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }
    }
}
