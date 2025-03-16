using AntDesign;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BlazorSample.BlazorServer.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        private MenuDataItem[] _menuData;

        [Inject] private ReuseTabsService TabService { get; set; }

        [Inject] private HttpClient HttpClient { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _menuData = JsonConvert.DeserializeObject<MenuDataItem[]>("""
                                [
                  {
                    "path": "",
                    "name": "welcome",
                    "key": "welcome",
                    "icon": "smile"
                  },
                  {
                    "path": "admin",
                    "name": "admin",
                    "key": "admin",
                    "icon": "crown",
                    "children": [
                      {
                        "path": "admin/sub-page",
                        "name": "sub-page",
                        "key": "admin.sub-page",
                        "icon": "crown"
                      },
                      {
                        "path": "admin/sub-page2",
                        "name": "sub-page2",
                        "key": "admin.sub-page2",
                        "icon": "crown"
                      },
                      {
                        "path": "admin/sub-page3",
                        "name": "sub-page3",
                        "key": "admin.sub-page3",
                        "icon": "crown"
                      }
                    ]
                  },
                  {
                    "path": "list",
                    "name": "list.table-list",
                    "key": "list.table-list",
                    "icon": "table",
                    "children": [
                      {
                        "path": "list/sub-page",
                        "name": "sub-page",
                        "key": "list.sub-page",
                        "icon": "crown"
                      },
                      {
                        "path": "list/sub-page2",
                        "name": "sub-page2",
                        "key": "list.sub-page2",
                        "icon": "crown"
                      }
                    ]
                  }
                ]
                """);
            await Task.Delay(1);
        }

        void Reload()
        {
            TabService.ReloadPage();
        }

        public void Dispose()
        {

        }

    }
}
