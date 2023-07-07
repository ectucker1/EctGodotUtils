using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using EmbedIO.WebSockets;

public class LiveValuesServer : Node
{
    private const string Url = "http://localhost:9696/";
    
    private LiveValuesModel _model;
    private WebServer _webServer;

    private double _nextSave = 0.0f;
    
    public override void _Ready()
    {
        base._Ready();

        _model = new LiveValuesModel();
        
        if (OS.IsDebugBuild() && !OS.GetName().Contains("HTML"))
        {
            _webServer = CreateWebServer(Url, _model);
            _webServer.RunAsync();

            
            //OpenBrowser(Url + "index.html");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (_model.Modified && _nextSave <= 0.0f)
            _nextSave = 0.5f;
        
        if (_nextSave > 0.0f)
        {
            _nextSave -= delta;
            if (_nextSave <= 0.0f)
            {
                _model.SaveJSON();
            }
        }
    }

    private static WebServer CreateWebServer(string url, LiveValuesModel model)
    {
        var server = new WebServer(o => o
                .WithUrlPrefix(url)
                .WithMode(HttpListenerMode.EmbedIO))
            .WithLocalSessionManager()
            .WithWebApi("/api", m => m
                .WithController(() => new LiveValuesListController(model)))
            .WithModule(new LiveValuesSocketModule("/changes", model))
            .WithStaticFolder("/", ProjectSettings.GlobalizePath("res://util/live_values"), false);
        return server;
    }

    private class LiveValuesListController : WebApiController
    {
        private LiveValuesModel _model;
        
        public LiveValuesListController(LiveValuesModel model)
        {
            _model = model;
        }
        
        [Route(HttpVerbs.Get, "/list")]
        public Dictionary<string, List<ILiveValue>> ListValues()
        {
            return _model.SerializeValueList();
        }
    }
    
    public class LiveValuesSocketModule : WebSocketModule
    {
        private LiveValuesModel _model;

        public LiveValuesSocketModule(string urlPath, LiveValuesModel model) : base(urlPath, true)
        {
            _model = model;
        }
        
        protected override Task OnMessageReceivedAsync(
            IWebSocketContext context,
            byte[] rxBuffer,
            IWebSocketReceiveResult rxResult)
        {
            string text = Encoding.GetString(rxBuffer);
            var change = JSON.Parse(text).Result;
            if (change is Godot.Collections.Dictionary dict)
            {
                var value = _model.GetValue(dict["category"].ToString(), dict["name"].ToString());
                if (value is LiveValueRange rangeVal)
                {
                    rangeVal.Value = float.Parse(dict["value"].ToString());
                    rangeVal.Field.SetValue(null, rangeVal.Value);
                    _model.MarkModified();
                }
                else if (value is LiveValueSwitch switchVal)
                {
                    switchVal.Value = bool.Parse(dict["value"].ToString());
                    switchVal.Field.SetValue(null, switchVal.Value);
                    _model.MarkModified();
                }
            }
            return Task.CompletedTask;
        }
    }

    private static void OpenBrowser(string url)
    {
        var browser = new System.Diagnostics.Process()
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true }
        };
        browser.Start();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        _webServer.Dispose();
    }
}