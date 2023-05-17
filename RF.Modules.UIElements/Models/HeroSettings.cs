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
using RF.Modules.UIElements.Util;

namespace RF.Modules.UIElements.Models
{
    public class HeroSettings : SettingsModel
    {
        public static HeroSettings Fetch(ModuleInstanceContext context)
        {
            var result = new HeroSettings();
            result.ImagePath = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.ImagePath), string.Empty);
            result.Title = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.Title), string.Empty);
            result.TitleColor = context.Configuration.ModuleSettings.GetValueOrDefault(result.GetKey(s => s.TitleColor), "#ffffff");

            return result;
        }

        public string ImagePath { get; set; }

        public string Title { get; set; }

        public string TitleColor { get; set; }

        public void UpdateContext(ModuleInstanceContext context)
        {
            context.Configuration.ModuleSettings[this.GetKey(s => s.ImagePath)] = ImagePath;
            context.Configuration.ModuleSettings[this.GetKey(s => s.Title)] = Title;
            context.Configuration.ModuleSettings[this.GetKey(s => s.TitleColor)] = TitleColor;
        }
    }
}