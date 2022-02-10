using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI.Selection;

namespace Parametr_proekta
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();

            foreach (Wall wall in walls)
            {
                var dlinaParametr = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                var obemParametr = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();


                using (Transaction ts = new Transaction(doc, "Set parameter"))
                {
                    ts.Start();
                    Parameter typeCommentParameter = wall.LookupParameter("Объем/площадь");
                    typeCommentParameter.Set($"{obemParametr}/{dlinaParametr}");
                    ts.Commit();
                }
            }
            return Result.Succeeded;
        }
    }
}
