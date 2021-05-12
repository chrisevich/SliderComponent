using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace SliderComponent
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class ExampleJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public ExampleJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/SliderComponent/JavaScript1.js").AsTask());
        }

        public async ValueTask<int> GetScrollWidth(ElementReference element)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("getScrollWidth", element);
        }

        public async ValueTask<int> GetSliderWidth(ElementReference element)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("getSliderWidth", element);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
