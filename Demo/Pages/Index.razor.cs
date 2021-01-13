﻿using System.Threading.Tasks;
using Aptacode.AppFramework.Utilities;
using Microsoft.AspNetCore.Components;

namespace Demo.Pages
{
    public class IndexBase : ComponentBase
    {
        public DemoSceneController SceneController { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Scene
            var scene = new SceneBuilder().SetWidth(200).SetHeight(100).Build();
            SceneController = new DemoSceneController(scene);
            await base.OnInitializedAsync();
        }
    }
}