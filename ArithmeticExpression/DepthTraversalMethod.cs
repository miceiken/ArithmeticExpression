using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression
{
    public enum DepthTraversalMethod
    {
        PreOrder,   // Node, Left, Right
        InOrder,    // Left, Node, Right
        PostOrder   // Left, Right, Node
    }
}
