using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.Models
{
    /// <summary>
    /// bootstrap-treeview
    /// </summary>
    public class Treeview
    {
        private string _id = "";
        private string _text = "";
        private string _icon = "";
        private string _selectedIcon = "";
        private string _href = "";
        private bool _selectable = true;
        private string _color = "";
        private string _backColor = "";
        private object _attributes = "";

        /// <summary>
        /// String. Optional	节点的前景色，覆盖全局的前景色选项。
        /// </summary>
        public string id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// String(必选项)	列表树节点上的文本，通常是节点右边的小图标。
        /// </summary>
        public string text { get { return _text; } set { _text = value; } }
        /// <summary>
        /// String(可选项)	列表树节点上的图标，通常是节点左边的图标。
        /// </summary>
        public string icon { get { return _icon; } set { _icon = value; } }
        /// <summary>
        /// String(可选项)	当某个节点被选择后显示的图标，通常是节点左边的图标。
        /// </summary>
        public string selectedIcon { get { return _selectedIcon; } set { _selectedIcon = value; } }
        /// <summary>
        /// String(可选项)	结合全局enableLinks选项为列表树节点指定URL。
        /// </summary>
        public string href { get { return _href; } set { _href = value; } }
        /// <summary>
        /// Boolean. Default: true	指定列表树的节点是否可选择。设置为false将使节点展开，并且不能被选择。
        /// </summary>
        public bool selectable { get { return _selectable; } set { _selectable = value; } }
        /// <summary>
        /// String. Optional	节点的前景色，覆盖全局的前景色选项。
        /// </summary>
        public string color { get { return _color; } set { _color = value; } }
        /// <summary>
        /// String. Optional	节点的背景色，覆盖全局的背景色选项。
        /// </summary>
        public string backColor { get { return _backColor; } set { _backColor = value; } }
        /// <summary>
        /// Array of Strings. Optional	通过结合全局showTags选项来在列表树节点的右边添加额外的信息。
        /// </summary>
        public List<string> tags { get; set; }
        /// <summary>
        /// String(可选项)	列表树节点上的数据ID号，通常是数据数据ID号(自家加的)
        /// </summary>
        public object attributes { get { return _attributes; } set { _attributes = value; } }
        /// <summary>
        /// 子项
        /// </summary>
        public List<Treeview> nodes { get; set; }
    }
}
