using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHanger
{
    public static class HangerFactory
    {
        public static void Create(Document doc,FamilySymbol symbol, XYZ location, Level level, double bottonHeight,
            double height, double width)
        {

            var familyInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                    .Cast<FamilySymbol>();

            var copyHangerSybol = symbol.Duplicate($"{location.X}*{location.Y}") as FamilySymbol;

            var hangerElement = doc.Create.NewFamilyInstance(location, copyHangerSybol, level, StructuralType.NonStructural);

            hangerElement.SetParameter("标高中的高程", bottonHeight.mmToft());
            hangerElement.SetParameter("宽度", width.mmToft());
            hangerElement.SetParameter("支吊架高度", height.mmToft());
        }

        private static void SetParameter(this Element ele, string name, double value)
        {
            ele.LookupParameter(name)?.Set(value);
        }

        public static double mmToft(this double mm)
        {
            return mm * 0.0032808;
        }

        public static double ftTomm(this double ft)
        {
            return ft * 304.8;
        }
    }
}
