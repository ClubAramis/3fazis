/*
' Copyright (c) 2023 Adam Halassy
'  All rights reserved.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
*/

using DotNetNuke.Collections;
using DotNetNuke.UI.Modules;
using RF.Modules.UIElements.Properties;
using RF.Modules.UIElements.Util;

namespace RF.Modules.UIElements.Models
{
    public class SpotlightSettings : SettingsModel
    {
        public static SpotlightSettings Fetch(ModuleInstanceContext context)
        {
            var result = new SpotlightSettings();
            result.ImagePath = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.ImagePath), string.Empty);
            result.Title = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.Title), Resources.SpotlightDefaultTitle);
            result.Detail = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.Detail), Resources.SpotlightDefaultDetail);
            result.Vertically = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.Vertically), "top");
            result.Horizontally = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.Horizontally), "right");
            result.Invert = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.Invert), false);

            return result;
        }

        public string ImagePath { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public string Vertically { get; set; }

        public string Horizontally { get; set; }

        public bool Invert { get; set; }

        public void UpdateContext(ModuleInstanceContext context)
        {
            context.Configuration.ModuleSettings[this.GetKey(s => s.ImagePath)] = ImagePath;
            context.Configuration.ModuleSettings[this.GetKey(s => s.Title)] = Title;
            context.Configuration.ModuleSettings[this.GetKey(s => s.Detail)] = Detail;
            context.Configuration.ModuleSettings[this.GetKey(s => s.Horizontally)] = Horizontally;
            context.Configuration.ModuleSettings[this.GetKey(s => s.Vertically)] = Vertically;
            context.Configuration.ModuleSettings[this.GetKey(s => s.Invert)] = Invert.ToString();
        }
    }
}