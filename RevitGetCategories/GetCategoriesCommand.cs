using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitGetCategories
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetCategoriesCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;

            var categoryNames = GetAllCategoriesFromDocument(doc);

            TaskDialog.Show("Categories in File", string.Join("\n", categoryNames));

            return Result.Succeeded;
        }

        private static List<string> GetAllCategoriesFromDocument(Document doc)
        {
            var collector = new FilteredElementCollector(doc);
            var categories = collector.WhereElementIsNotElementType()
                .Select(x => x.Category?.Name)
                .Where(x => x != null)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return categories;
        }
    }
}