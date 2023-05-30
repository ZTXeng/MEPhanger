using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHanger
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;

            var tran = new Transaction(doc,"start");

            tran.Start();


            var refElement = uidoc.Selection.PickObject(ObjectType.Element,"请选择一条管道");

            var pipe = doc.GetElement(refElement) as Pipe;
           
            if (pipe != null)
            {
                var point = uidoc.Selection.PickPoint();
                var pipeWidth =double.Parse( string.Join("", pipe.LookupParameter("直径")
                                           .AsValueString().Where(Char.IsDigit)));

                var pipeHeight = double.Parse( pipe.LookupParameter("底部高程")
                                           .AsValueString());


                var familyInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
               .Cast<FamilySymbol>();

                var hangerSymbol2 = familyInstance.FirstOrDefault(x => x.FamilyName == "族2");

                var hangerSymbol3 = familyInstance.FirstOrDefault(x => x.FamilyName == "族3");
                var hangerSymbol4 = familyInstance.FirstOrDefault(x => x.FamilyName == "族4");


                var level = new FilteredElementCollector(doc).OfClass(typeof(Level))
                    .Cast<Level>().FirstOrDefault(x => x.Elevation == 0);

                var floor = new FilteredElementCollector(doc).OfClass(typeof(Floor))
                    .Cast<Floor>().FirstOrDefault();

                var floorHeight = double.Parse(floor.LookupParameter("底部高程")
                                            .AsValueString());

                HangerFactory.Create(doc, hangerSymbol2, new XYZ(point.X,point.Y,0), level, pipeHeight-100, floorHeight- pipeHeight+100, pipeWidth+200+300);
                HangerFactory.Create(doc, hangerSymbol3, new XYZ(point.X,point.Y,0), level, pipeHeight-100, floorHeight- pipeHeight+100, pipeWidth+200+300);
                HangerFactory.Create(doc, hangerSymbol4, new XYZ(point.X,point.Y,0), level, pipeHeight-100, floorHeight- pipeHeight+100, pipeWidth+200+300);

            }
           

            tran.Commit();

            return Result.Succeeded;
        }

    }
}
