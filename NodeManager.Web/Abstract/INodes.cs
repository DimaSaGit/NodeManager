using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;

namespace NodeManager.Web.Abstract
{
    public interface INodes
    {
        NodeManagerDBEntities dbContext { get; }
        //IEnumerable<Node> Nodes { get; }
        IEnumerable<FamilySymbol> FamilySymbols { get; }
        IEnumerable<RevitParameter> RevParameters { get; }
        IEnumerable<Categories> Categories { get; }
        IEnumerable<Sections> Sections { get; }
        IEnumerable<Tags> Tags { get; }
        IEnumerable<FSTags> FSTags { get; }
        IEnumerable<Users> Users { get; }
        IEnumerable<Files> Files { get; }
    }
}