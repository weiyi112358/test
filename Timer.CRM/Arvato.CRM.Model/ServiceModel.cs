using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    //树图节点
    public class TreeNode
    {
        public string key;
        public string title;
        public bool isFolder;
        public bool isLazy;
        public bool unselectable;
        public bool expand;
        public List<TreeNode> children;
    }

    //树图数据源模板
    public class TreeDataSource
    {
        public int nodeGrade;//节点等级
        public string nodeId;//节点ID
        public string nodePId;//节点父ID
        public string nodeName;//节点名称
    }
}
